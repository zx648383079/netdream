using NetDream.Shared.Extensions;
using NetDream.Shared.Helpers;
using NetDream.Shared.Interfaces;
using NetDream.Shared.Interfaces.Forms;
using NetDream.Shared.Migrations;
using NetDream.Shared.Models;
using NetDream.Modules.MessageService.Entities;
using NetDream.Modules.MessageService.Forms;
using NPoco;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace NetDream.Modules.MessageService.Repositories
{
    public partial class MessageRepository(IDatabase db, 
        IGlobeOption option, IClientContext environment,
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

        public Page<TemplateEntity> TemplateList(string keywords = "", int type = -1, long page = 1)
        {
            var sql = new Sql();
            sql.Select("id", "type", "name", "title", "target_no", "status", MigrationTable.COLUMN_CREATED_AT);
            sql.From<TemplateEntity>(db);
            SearchHelper.Where(sql, ["name", "title"], keywords);
            if (type > 0)
            {
                sql.Where("type=@0", type);
            }
            sql.OrderBy("id DESC");
            return db.Page<TemplateEntity>(page, 20, sql);
        }

        public TemplateEntity Template(int id)
        {
            return db.SingleById<TemplateEntity>(id);
        }

        public TemplateEntity TemplateSave(TemplateForm data)
        {
            var model = data.Id > 0 ? db.SingleById<TemplateEntity>(data.Id) :
                new TemplateEntity();
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
            if (model.CreatedAt == 0)
            {
                model.CreatedAt = environment.Now;
            }
            model.UpdatedAt = environment.Now;
            db.TrySave(model);
            return model;
        }
        public void TemplateRemove(int id)
        {
            TemplateRemove([id]);
        }
        public void TemplateRemove(int[] items)
        {
            if (items.Length == 0)
            {
                return;
            }
            var sql = new Sql();
            sql.Select("id", "name");
            sql.From<TemplateEntity>(db);
            sql.WhereIn("id", items);
            var exist = db.Fetch<TemplateEntity>(sql);
            var del = new List<int>();
            foreach (var item in exist)
            {
                if (!IsSystemTemplate(item.Name))
                {
                    del.Add(item.Id);
                    continue;
                }
                if (db.FindCount<TemplateEntity>("id<>@0 AND name=@1", item.Id, item.Name) > 0)
                {
                    del.Add(item.Id);
                    continue;
                }
            }
            if (del.Count == 0)
            {
                return;
            }
            sql = new Sql();
            sql.WhereIn("id", [..del]);
            db.Delete<TemplateEntity>(sql);
        }

        public TemplateEntity? TemplateChange(int id, string[] data)
        {
            return ModelHelper.BatchToggle<TemplateEntity>(db, id, data, ["status"]);
        }

        public Page<LogEntity> LogList(string keywords = "", int status = -1, long page = 1)
        {
            var sql = new Sql();
            sql.Select("id", "target", "template_name", "status", "message", MigrationTable.COLUMN_CREATED_AT);
            sql.From<LogEntity>(db);
            SearchHelper.Where(sql, ["target", "template_name"], keywords);
            if (status > 0)
            {
                sql.Where("status=@0", status);
            }
            sql.OrderBy("id DESC");
            return db.Page<LogEntity>(page, 20, sql);
        }

        public void LogRemove(int id)
        {
            db.Delete<LogEntity>(id);
        }

        public void LogClear()
        {
            db.Delete<LogEntity>(string.Empty);
        }

        public void InsertIf(string name, string title, string content, byte type = MessageProtocol.TYPE_TEXT)
        {
            if (db.FindCount<TemplateEntity>("name=@0", name) > 0 ||
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
            db.TrySave(model);
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
