using Modules.Counter.Entities;
using NetDream.Core.Migrations;
using NPoco;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetDream.Modules.Counter.Migrations
{
    public class CreateCounterTables(IDatabase db) : Migration(db)
    {

        public override void Up()
        {
            Append<PageLogEntity>(table => {
                table.Comment("页面访问记录");
                table.Id();
                table.String("url");
                table.Uint("visit_count").Default(0);
            }).Append<VisitorLogEntity>(table => {
                table.Comment("访客日志");
                table.Id();
                table.Uint("user_id").Default(0);
                table.String("ip", 120);
                table.Timestamp("first_at");
                table.Timestamp("last_at");
            }).Append<JumpLogEntity>(table => {
                table.Comment("页面跳出记录");
                table.Id();
                table.String("referrer").Default(string.Empty);
                table.String("url");
                table.String("ip", 120);
                table.String("session_id", 32).Default(string.Empty);
                table.String("user_agent").Default(string.Empty).Comment("代理");
                table.Timestamp(MigrationTable.COLUMN_CREATED_AT);
            }).Append<ClickLogEntity>(table => {
                table.Comment("页面点击记录");
                table.Id();
                table.String("url");
                table.String("ip", 120);
                table.String("session_id", 32).Default(string.Empty);
                table.String("user_agent").Default(string.Empty).Comment("代理");
                table.String("x", 100).Default(0);
                table.String("y", 100).Default(0);
                table.String("tag", 120);
                table.String("tag_url", 120).Default(string.Empty);
                table.Timestamp(MigrationTable.COLUMN_CREATED_AT);
            }).Append<LoadTimeLogEntity>(table => {
                table.Comment("页面加载记录");
                table.Id();
                table.String("url");
                table.String("ip", 120);
                table.String("session_id", 32).Default(string.Empty);
                table.String("user_agent").Default(string.Empty).Comment("代理");
                table.Uint("load_time", 5);
                table.Timestamp(MigrationTable.COLUMN_CREATED_AT);
            }).Append<LogEntity>(table => {
                table.Comment("访问记录");
                table.Id();
                table.String("ip", 120);
                table.String("browser", 40).Default(string.Empty).Comment("浏览器");
                table.String("browser_version", 20).Default(string.Empty).Comment("浏览器版本");
                table.String("os", 20).Default(string.Empty).Comment("操作系统");
                table.String("os_version", 20).Default(string.Empty).Comment("操作系统版本");
                table.String("url").Default(string.Empty).Comment("请求网址");
                table.String("referrer").Default(string.Empty).Comment("来路");
                table.String("user_agent").Default(string.Empty).Comment("代理");
                table.String("country", 45).Default(string.Empty);
                table.String("region", 45).Default(string.Empty);
                table.String("city", 45).Default(string.Empty);
                table.Uint("user_id").Default(0);
                table.String("session_id", 32).Default(string.Empty);
                table.String("language", 20).Default(string.Empty);
                table.String("latitude", 30).Default(string.Empty).Comment("纬度");
                table.String("longitude", 30).Default(string.Empty).Comment("经度");
                table.Timestamp(MigrationTable.COLUMN_CREATED_AT);
            }).Append<StayTimeLogEntity>(table => {
                table.Comment("页面停留时间");
                table.Id();
                table.String("url");
                table.String("ip", 120);
                table.String("user_agent").Default(string.Empty).Comment("代理");
                table.String("session_id", 32).Default(string.Empty);
                table.Bool("status").Default(0)
                    .Comment("是否停留");
                table.Timestamp("enter_at");
                table.Timestamp("leave_at");
            });
            AutoUp();
        }
    }
}
