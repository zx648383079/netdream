using NetDream.Shared.Interfaces;
using NetDream.Shared.Migrations;
using NPoco;
using Modules.OnlineService.Entities;

namespace NetDream.Modules.OnlineService.Migrations
{
    public class CreateOnlineServiceTables(IDatabase db, IPrivilegeManager privilege) : Migration(db)
    {
        public override void Up()
        {
            Append<CategoryEntity>(table => {
                table.Id();
                table.String("name");
            }).Append<CategoryWordEntity>(table => {
                table.Id();
                table.String("content");
                table.Uint("cat_id");
            }).Append<CategoryUserEntity>(table => {
                table.Id();
                table.Uint("cat_id");
                table.Uint("user_id");
                table.Timestamps();
            }).Append<SessionEntity>(table => {
                table.Id();
                table.String("name", 20).Default(string.Empty);
                table.String("remark").Default(string.Empty);
                table.Uint("user_id").Default(0);
                table.Uint("service_id").Default(0);
                table.String("ip", 120);
                table.String("user_agent", 255);
                table.Uint("status", 1).Default(0);
                table.Uint("service_word").Default(0).Comment("客服设置的自动回复");
                table.Timestamps();
            }).Append<SessionLogEntity>(table => {
                table.Id();
                table.Uint("user_id");
                table.Uint("session_id");
                table.String("remark").Default(string.Empty);
                table.Uint("status", 1).Default(0);
                table.Timestamp(MigrationTable.COLUMN_CREATED_AT);
            }).Append<MessageEntity>(table => {
                table.Id();
                table.Uint("user_id").Default(0).Comment("发送者");
                table.Uint("session_id");
                table.Bool("send_type").Default(0).Comment("发送者的身份，0咨询者1客服");
                table.Uint("type", 2).Default(0).Comment("内容类型");
                table.String("content").Default(string.Empty);
                table.String("extra_rule", 400)
                    .Default(string.Empty).Comment("附加替换规则");
                table.Uint("status", 2).Default(0);
                table.Timestamps();
            });
            AutoUp();
        }

        public override void Seed()
        {
            privilege.AddPermission("service_manage", "客服管理");
        }
    }
}
