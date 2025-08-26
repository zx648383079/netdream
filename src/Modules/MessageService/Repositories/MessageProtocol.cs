using NetDream.Shared.Helpers;
using NetDream.Shared.Interfaces;
using NetDream.Shared.Models;
using NetDream.Modules.MessageService.Entities;
using NetDream.Modules.MessageService.Forms;
using NetDream.Modules.MessageService.Models;
using System.Text.Json;
using System.Collections.Generic;
using System;
using System.Linq;
using NetDream.Shared.Providers;
using Microsoft.EntityFrameworkCore;

namespace NetDream.Modules.MessageService.Repositories
{
    public class MessageProtocol(MessageServiceContext db, 
        IClientContext environment,
        IGlobeOption option)
    {
        public const string SESSION_KEY = "ms_code";

        public const string OPTION_MAIL_KEY = "mail_protocol";
        public const string OPTION_SMS_KEY = "sms_protocol";
        public const byte TYPE_TEXT = 1;
        public const byte TYPE_HTML = 5;

        public const byte RECEIVE_TYPE_MOBILE = 1;
        public const byte RECEIVE_TYPE_EMAIL = 2;

        public const byte STATUS_NONE = 0;
        public const byte STATUS_SENDING = 1;

        public const byte STATUS_SEND_FAILURE = 4;

        public const byte STATUS_SENT = 6;
        public const byte STATUS_SENT_USED = 7;
        public const byte STATUS_SENT_EXPIRED = 9;

        public const string EVENT_LOGIN_CODE = "login_code";
        public const string EVENT_REGISTER_CODE = "register_code";
        public const string EVENT_REGISTER_EMAIL_CODE = "register_email";
        public const string EVENT_FIND_CODE = "find_code";

        private readonly MessageSetting _configs = new();

        /// <summary>
        /// 发送验证码
        /// </summary>
        /// <param name="target"></param>
        /// <param name="templateName"></param>
        /// <param name="code"></param>
        /// <param name="extra"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public IOperationResult<int> SendCode(string target, string templateName, string code, IDictionary<string, string>? extra = null)
        {
            if (!VerifySpace(target) || !VerifyIp() || !VerifyCount()) {
                return OperationResult<int>.Fail("发送过于频繁");
            }
            var data = new Dictionary<string, string>()
            {
                {"code", code },
            };
            if (extra is not null)
            {
                foreach (var item in extra)
                {
                    data.TryAdd(item.Key, item.Value);
                }
            }
            var res = Send(target, templateName, data);
            if (res.Succeeded && res.Result > 0)
            {
                db.Logs.Where(i => i.Target == target && i.TemplateName == templateName
                 && i.Id != res.Result && i.Status == STATUS_SENT)
                    .ExecuteUpdate(setters => setters.SetProperty(i => i.Status, STATUS_SENT_EXPIRED));
            }
            db.SaveChanges();
            //if (res && EnableSession(templateName)) {
            //    session().set(self.SESSION_KEY, [
            //        "at" => time(),
            //        "to" => target,
            //        "code" => code
            //    ]);
            //}
            return res;
        }

        
        /// <summary>
        /// 发送消息
        /// </summary>
        /// <param name="target"></param>
        /// <param name="templateName"></param>
        /// <param name="data"></param>
        /// <returns>返回记录id, 失败则为0</returns>
        /// <exception cref="Exception"></exception>
        public IOperationResult<int> Send(string target, string templateName, IDictionary<string, string> data)
        {
            var (type, option, optionKey) = TargetOption(target);
            var query = db.Templates.Where(i => i.Name == templateName && i.Status == 1);
            if (type == RECEIVE_TYPE_MOBILE)
            {
                query = query.Where(i => i.Type == TYPE_TEXT);
            } else
            {
                query = query.OrderByDescending(i => i.Type);
            }
            var template = query.SingleOrDefault();
            if (template is null)
            {
                return OperationResult<int>.Fail(string.Format("未配置相关模板[{0}:{1}]", optionKey, templateName));
            }
            var logData = new LogEntity() {
                TemplateId = template.Id,
                TargetType = type,
                Target = target,
                TemplateName = templateName,
                Type = template.Type,
                Title = template.Title,
                Content = RenderTemplate(template.Content, data),
                Status = STATUS_SENDING,
                Code = data["code"] ?? string.Empty,
                Ip = environment.Ip,
            };
            // TODO 根据模板值过滤
            data = FilterData(data, template.Data);
            db.Logs.Save(logData, TimeHelper.TimestampNow());
            db.SaveChanges();
            try
            {
                var res = type == RECEIVE_TYPE_MOBILE ? SendSMS(option as SmsProtocolSetting, target, template, data) : 
                    SendMail(option as MailProtocolSetting, target, template, data);
                logData.Status = res != false ? STATUS_SENT: STATUS_SEND_FAILURE;
                logData.Message = res ? res.Message : string.Empty;
                if (res.Succeeded)
                {
                    return OperationResult.Ok(logData.Id);
                }
                return OperationResult<int>.Fail(res.FailureReason, res.Message);
            }
            catch (Exception ex) 
            {
                logData.Status = STATUS_SEND_FAILURE;
                logData.Message = ex.Message;
                return OperationResult<int>.Fail(ex.Message);
            } finally
            {
                db.Logs.Save(logData, TimeHelper.TimestampNow());
                db.SaveChanges();
            }
        }

        /// <summary>
        /// 根据目标获取配置信息
        /// </summary>
        /// <param name="target"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        protected (byte, IProtocolSetting, string) TargetOption(string target)
        {
            if (string.IsNullOrWhiteSpace(target))
            {
                throw new Exception("无效接受者");
            }
            byte type = 0;
            if (Validator.IsEmail(target))
            {
                type = RECEIVE_TYPE_EMAIL;
            }
            else if (Validator.IsMobile(target))
            {
                type = RECEIVE_TYPE_MOBILE;
            }
            else
            {
                throw new Exception("无效接受者");
            }
            var optionKey = type == RECEIVE_TYPE_MOBILE ? OPTION_SMS_KEY: OPTION_MAIL_KEY;
            IProtocolSetting? config = type == RECEIVE_TYPE_MOBILE ? option.Get<SmsProtocolSetting>(optionKey) : option.Get<MailProtocolSetting>(optionKey);
            if (config is null)
            {
                throw new Exception(string.Format("未配置相关参数[{0}]", optionKey));
            }
            return (type, config, optionKey);
        }
        protected IDictionary<string, string> FilterData(IDictionary<string, string> data, string filterKeys)
        {
            return FilterData(data, JsonSerializer.Deserialize<string[]>(filterKeys));
        }

        protected IDictionary<string, string> FilterData(IDictionary<string, string> data, string[]? filterKeys)
        {
            if (filterKeys is null || filterKeys.Length == 0)
            {
                return data;
            }
            return data.Where(item => filterKeys.Contains(item.Key)).ToDictionary();
        }

        /// <summary>
        /// 判断是否可以保存到 SESSION 中
        /// </summary>
        /// <param name="templateName"></param>
        /// <returns></returns>
        protected bool EnableSession(string templateName)
        {
            return environment.PlatformId == 0 && templateName != EVENT_REGISTER_EMAIL_CODE;
        }

        /// <summary>
        /// 验证验证码
        /// </summary>
        /// <param name="target"></param>
        /// <param name="templateName"></param>
        /// <param name="code"></param>
        /// <param name="once"></param>
        /// <returns></returns>
        public bool VerifyCode(string target, string templateName, string code, bool once = true)
        {
            if (EnableSession(templateName)) {
                //log = session(self.SESSION_KEY);
                //if (empty(log) || empty(log["code"])
                //    || log["to"] !== target
                //    || log["code"] !== code)
                //{
                //    return false;
                //}
            }
            var log = db.Logs.Where(i => i.Target == target && i.TemplateName == templateName && i.Status == STATUS_SENT)
                .Single();
       
            if (log is null)
            {
                return false;
            }
            var res = !string.IsNullOrWhiteSpace(code) && log.Code == code;
            if (once)
            {
                log.Status = STATUS_SENT_USED;
                db.Logs.Save(log, TimeHelper.TimestampNow());
                db.SaveChanges();
            }
            if (once && EnableSession(templateName))
            {
                // session().delete(self.SESSION_KEY);
            }
            return res;
        }


        protected OperationResult SendMail(
            MailProtocolSetting option, string target, TemplateEntity template, IDictionary<string, string> data)
        {
            return SendMailOrThrow(option, target,
                data["name"] ?? target, template.Title,
                    RenderTemplate(template.Content, data),
                    template.Type > TYPE_TEXT);
        }

        protected OperationResult SendMailOrThrow(MailProtocolSetting option, string target, string targetName, string title, string content, bool isHtml = true)
        {
            // TODO
            //mail = new Mailer(option);
            //res = mail.isHtml(isHtml)
            //    .addAddress(target, targetName)
            //    .send(title, content);
            //if (!res)
            //{
            //    throw new \Exception(mail.getError() ?? "smtp send error");
            //}
            return OperationResult.Ok();
        }

        protected string RenderTemplate(string content, IDictionary<string, string> data)
        {
            foreach (var item in data)
            {
                content = content.Replace("{" + item.Key + "}", item.Value);
            }
            return content;
        }

        public void SendCustom(string target, string title, Func<string> cb, bool isHtml = true)
        {

        }
        /// <summary>
        /// 自定义方式内容
        /// </summary>
        /// <param name="target"></param>
        /// <param name="title"></param>
        /// <param name="content"></param>
        /// <param name="isHtml"></param>
        /// <exception cref="Exception"></exception>
        public void SendCustom(string target, string title, string content, bool isHtml = true)
        {
            var (type, option, _) = TargetOption(target);
            //if (is_callable(content))
            //{
            //    content = call_user_func(content);
            //}
            if (string.IsNullOrWhiteSpace(content))
            {
                throw new Exception("发送内容不能为空");
            }
            var log = new LogEntity()
            {
                TargetType = type,
                Target = target,
                Type = isHtml ? TYPE_HTML : TYPE_TEXT,
                Title = title,
                Content = content,
                Status = STATUS_SENDING,
                Ip = environment.Ip,
            };
            db.Logs.Save(log, environment.Now);
            db.SaveChanges();
            OperationResult? res = null;
            try
            {
                if (type == RECEIVE_TYPE_MOBILE) {
                    if (isHtml)
                    {
                        // content = Html.toText(content);
                    }
                    var api = SMSProtocol(option as SmsProtocolSetting);
                    //if (!api.isOnlyTemplate())
                    //{
                    //    throw new \Exception("不支持自定义内容");
                    //}
                    //res = api.send(target, content, [], option["sign_name"] ?? string.Empty);
                } else
                {
                    res = SendMailOrThrow(option as MailProtocolSetting, target, target, title, content, isHtml);
                }
                log.Status = res == false ? STATUS_SENT: STATUS_SEND_FAILURE;
                log.Message = res ? res.Message : string.Empty;
            }
            catch (Exception ex) {
                log.Status = STATUS_SEND_FAILURE;
                log.Message = ex.Message;
            }
            db.Logs.Save(log, TimeHelper.TimestampNow());
            db.SaveChanges();
        }

        protected string RenderTemplateFile(string fileName, IDictionary<string, string> data)
        {
                return "";//view().render("@root/Template/".fileName, data);
        }

        protected OperationResult SendSMS(SmsProtocolSetting option, string target, TemplateEntity template, IDictionary<string, string> data)
        {
            var api = SMSProtocol(option);
            //if (api.isOnlyTemplate())
            //{
            //    return api.send(target, template["target_no"], data, option["sign_name"] ?? string.Empty);
            //}
            //return api.send(target, static.renderTemplate(template["content"], data),
            //data, option["sign_name"] ?? string.Empty);
            return OperationResult.Ok();
        }

        protected object SMSProtocol(SmsProtocolSetting option)
        {
            return null;
            //return match(option["protocol"] ?? string.Empty) {
            //    "alidayu" => new ALiDaYu(option),
            //"aliyun" => new ALiYun(array_merge(option, ["AccessKeyId" => option["app_key"]])),
            //default => new IHuYi(array_merge(option, ["account" => option["app_key"],
            //    "password" => option["secret"]]))
            //};
        }

        protected bool VerifyCount()
        {
            if (_configs.Everyday < 1)
            {
                return true;
            }
            var time = TimeHelper.TimestampFrom(DateTime.Today);
            var count = db.Logs.Where(i => i.CreatedAt>=time).Count();
            return count < _configs.Everyday;
        }

        protected bool VerifySpace(string target)
        {
            if (environment.PlatformId == 0)
            {
                //log = session(self.SESSION_KEY);
                //return empty(log) || (time() - log["at"]) > self.configs["space"];
            }
            var last = db.Logs.Where(i => i.Target == target && i.Status == STATUS_SEND_FAILURE).Max(i => i.CreatedAt);
            if (last == 0)
            {
                return true;
            }
            return environment.Now - last > _configs.Space;
        }

        protected bool VerifyIp()
        {
            if (_configs.Everyone < 1)
            {
                return true;
            }
            var time = TimeHelper.TimestampFrom(DateTime.Today);
            var count = db.Logs.Where(i => i.Ip == environment.Ip && i.CreatedAt >= time).Count();
            return count < _configs.Everyone;
        }

        public string GenerateCode(int length, bool isNumeric = true)
        {
            return isNumeric ? StrHelper.RandomNumber(length) : StrHelper.Random(length);
        }
    }
}
