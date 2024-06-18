using NetDream.Modules.MessageService.Entities;
using NetDream.Core.Migrations;
using NPoco;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NetDream.Modules.MessageService.Repositories;

namespace NetDream.Modules.MessageService.Migrations
{
    public class CreateMessageServiceTables(IDatabase db) : Migration(db)
    {

        public override void Up()
        {
            Append<TemplateEntity>(table => {
                table.Comment("消息模板");
                table.Id();
                table.String("title", 100).Comment("标题");
                table.String("name", 20).Comment("调用代码");
                table.Uint("type", 1).Default(MessageProtocol.TYPE_TEXT)
                .Comment("模板的类型");
                table.String("data").Comment("模板字段");
                table.Text("content").Comment("模板内容");
                table.String("target_no", 32).Default(string.Empty).Comment("外部编号");
                table.Timestamps();
            }).Append<LogEntity>(table => {
                table.Comment("短信发送记录");
                table.Id();
                table.Uint("template_id").Default(0).Comment("模板id");
                table.Uint("target_type", 1).Comment("接受者类型");
                table.String("target", 100).Comment("接受者，手机号/邮箱");
                table.String("template_name", 20).Default(string.Empty).Comment("调用代码");
                table.Uint("type", 1).Default(MessageProtocol.TYPE_TEXT)
                    .Comment("内容的类型");
                table.String("title").Comment("发送的标题");
                table.Text("content").Comment("发送的内容");
                table.String("code", 50).Default(string.Empty).Comment("发送的验证码");
                table.Uint("status", 1).Default(0).Comment("发送状态");
                table.String("message").Default(string.Empty).Comment("发送结果，成功为消息id,否则为错误信息");
                table.String("ip", 120).Default(string.Empty).Comment("发送者ip");
                table.Timestamps();
            });
            AutoUp();
        }

        public override void Seed()
        {
            var repository = new MessageRepository(db);
            repository.InsertIf(MessageProtocol.EVENT_LOGIN_CODE,
                "登录验证码", "登录验证码{code}");
            repository.InsertIf(MessageProtocol.EVENT_REGISTER_CODE,
                "注册验证码", "注册验证码{code}");
            repository.InsertIf(MessageProtocol.EVENT_REGISTER_EMAIL_CODE,
                "验证你的Email", "<a href=\"{url}\">确认你的Email</a><br/>或复制链接在浏览器中打开：<br/>{url}", MessageProtocol.TYPE_HTML);
            repository.InsertIf(MessageProtocol.EVENT_FIND_CODE,
                "重置密码验证码", "重置密码验证码{code}");
        }
    }
}
