using NetDream.Shared.Helpers;
using NetDream.Shared.Interfaces;
using NetDream.Shared.Interfaces.Forms;
using NetDream.Shared.Models;
using NetDream.Modules.MessageService.Entities;
using NetDream.Modules.MessageService.Forms;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Text.RegularExpressions;
using NetDream.Shared.Providers;
using Microsoft.EntityFrameworkCore;

namespace NetDream.Modules.MessageService.Repositories
{
    public partial class MessageRepository(MessageServiceContext db, 
        IGlobeOption option, IClientContext client,
        MessageProtocol protocol)
    {
        protected bool IsSystemTemplate(string name)
        {
            return name switch {
                MessageProtocol.EVENT_LOGIN_CODE or
                MessageProtocol.EVENT_FIND_CODE or
                MessageProtocol.EVENT_REGISTER_CODE => true,
                _ => false
            };
        }

        public IFormInput[] OptionInput(bool isMail = true)
        {
            if (isMail)
            {
                return [
                    FormInput.Text("host", "服务器"),
                    FormInput.Number("port", "端口"),
                    FormInput.Text("name", "发送者"),
                    FormInput.Text("user", "账号", false),
                    FormInput.Password("password", "密码", false),
                ];
            }
            return [
                FormInput.Select("protocol", "接口", [
                    new("IHuYi", "ihuiyi"),
                    new("阿里云短信", "aliyun"),
                    new("阿里大于", "alidayu"),
                ]),
                FormInput.Text("app_key", "APP KEY"),
                FormInput.Text("secret", "SECRET", false),
                FormInput.Text("sign_name", "签名", false),
            ];
        }

        public IFormInput[] OptionForm(bool isMail = true)
        {
            return InputHelper.Patch(OptionInput(isMail), Option(isMail));
        }

        public IProtocolSetting? Option(bool isMail = true)
        {
            if (isMail)
            {
                return option.Get<MailProtocolSetting>(MessageProtocol.OPTION_MAIL_KEY);
            }
            return option.Get<SmsProtocolSetting>(MessageProtocol.OPTION_SMS_KEY);
        }

        public void OptionSave(IProtocolSetting data, bool isMail = true)
        {
            // data = InputHelper.value(static.optionInput(isMail), data);
            option.InsertOrUpdate(isMail ? MessageProtocol.OPTION_MAIL_KEY : MessageProtocol.OPTION_SMS_KEY,
                data, isMail ? "Mail Smtp 配置" : "SMS配置");
        }

        public IPage<TemplateEntity> TemplateList(string keywords = "", int type = -1, 
            int page = 1)
        {
            return db.Templates.Search(keywords, "name", "title")
                .When(type > 0, i => i.Type == type)
                .OrderByDescending(i => i.Id)
                .Select(i => new TemplateEntity()
                {
                    Id = i.Id,
                    Name = i.Name,
                    Type = i.Type,
                    TargetNo = i.TargetNo,
                    Status = i.Status,
                    CreatedAt = i.CreatedAt,
                }).ToPage(page);
        }

        public TemplateEntity? Template(int id)
        {
            return db.Templates.Where(i => i.Id == id).Single();
        }

        public IOperationResult<TemplateEntity> TemplateSave(TemplateForm data)
        {
            var model = data.Id > 0 ? db.Templates.Where(i => i.Id == data.Id).Single() :
                new TemplateEntity();
            if (model is null)
            {
                return OperationResult.Fail<TemplateEntity>("id is error");
            }
            model.Content = data.Content;
            model.Title = data.Title;
            model.Name = data.Name;
            model.Type = data.Type;
            model.Status = data.Status;
            model.TargetNo = data.TargetNo;
            if (data.Data is not null && data.Data.Length > 0)
            {
                model.Data = JsonSerializer.Serialize(data.Data);
            }
            db.Templates.Save(model, client.Now);
            db.SaveChanges();
            return OperationResult.Ok(model);
        }
        public void TemplateRemove(params int[] items)
        {
            if (items.Length == 0)
            {
                return;
            }
            var exist = db.Templates.Where(i => items.Contains(i.Id))
                .Select(i => new TemplateEntity()
                {
                    Id = i.Id,
                    Name = i.Name,
                }).ToArray();
            var del = new List<int>();
            foreach (var item in exist)
            {
                if (!IsSystemTemplate(item.Name))
                {
                    del.Add(item.Id);
                    continue;
                }
                if (db.Templates.Where(i => i.Id != item.Id && i.Name == item.Name).Any())
                {
                    del.Add(item.Id);
                    continue;
                }
            }
            if (del.Count == 0)
            {
                return;
            }
            db.Templates.Where(i => del.Contains(i.Id)).ExecuteDelete();
        }

        public TemplateEntity? TemplateChange(int id, string[] data)
        {
            var res = db.Templates.BatchToggle(id, data, "status");
            if (res is not null)
            {
                db.SaveChanges();
            }
            return res;
        }

        public IPage<LogEntity> LogList(string keywords = "", 
            int status = -1, int page = 1)
        {
            return db.Logs.Search(keywords, "target", "template_name")
                .When(status > 0, i => i.Status == status)
                .OrderByDescending(i => i.Id)
                .Select(i => new LogEntity()
                {
                    Id = i.Id,
                    Target = i.Target,
                    TemplateName = i.TemplateName,
                    Status = i.Status,
                    Message = i.Message,
                    CreatedAt = i.CreatedAt,
                }).ToPage(page);
        }

        public void LogRemove(int id)
        {
            db.Logs.Where(i => i.Id == id).ExecuteDelete();
        }

        public void LogClear()
        {
            db.Logs.ExecuteDelete();
        }

        public void InsertIf(string name, string title, string content, byte type = MessageProtocol.TYPE_TEXT)
        {
            if (db.Templates.Where(i => i.Name == name).Any() ||
                string.IsNullOrWhiteSpace(content))
            {
                return;
            }
            var data = string.Empty;
            var matches = ParameterRegex().Matches(content);
            if (matches.Count > 0)
            {
                data = JsonSerializer.Serialize(matches.Select(item => item.Groups[1].Value));
            }
            var model = new TemplateEntity()
            {
                Title = title,
                Name = name,
                Content = content,
                Type = type,
                Data = data,
            };
            db.Templates.Save(model, client.Now);
            db.SaveChanges();
        }

        public void Debug(DebugForm data)
        {
            protocol.SendCustom(data.Target,
                data.Title, data.Content, data.Type > 0);
        }

        [GeneratedRegex(@"\{(\w+)\}")]
        private static partial Regex ParameterRegex();
    }
}
