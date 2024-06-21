using NetDream.Shared.Interfaces;
using NetDream.Shared.Migrations;
using NetDream.Modules.Auth.Entities;
using NetDream.Modules.Auth.Repositories;
using NPoco;

namespace NetDream.Modules.Auth.Migrations
{
    public class CreateAuthTables(
        IPrivilegeManager privilege,
        IGlobeOption option,
        IDatabase db) : Migration(db)
    {

        public override void Up()
        {
            Append<UserEntity>(table => {
                table.Id();
                table.String("name", 100);
                table.String("email", 200).Default(string.Empty);
                table.String("mobile", 20).Default(string.Empty);
                table.String("password", 100);
                table.Uint("sex", 1).Default(0);
                table.String("avatar").Default(string.Empty);
                table.Date("birthday").Default(DateTime.Now.ToString("yyyy-MM-dd"));
                table.Uint("money").Default(0);
                table.Uint("credits").Default(0).Comment("积分");
                table.Uint("parent_id").Default(0);
                table.String("token", 60).Default(0);
                table.Uint("status", 2).Default(UserRepository.STATUS_ACTIVE);
                table.Timestamps();
            });
            Append<OauthEntity>(table => {
                table.Id();
                table.Uint("user_id");
                table.Uint("platform_id").Default(0).Comment("平台id");
                table.String("nickname", 30).Default(string.Empty).Comment("昵称");
                table.String("vendor", 20).Default(string.Empty);
                table.String("unionid", 100).Default(string.Empty).Comment("联合id");
                table.String("identity", 100);
                table.Text("data").Nullable();
                table.Timestamp(MigrationTable.COLUMN_CREATED_AT);
            });
            Append<UserMetaEntity>(table => {
                table.Id();
                table.Uint("user_id");
                table.String("name", 100);
                table.Text("content");
            });
            Append<UserRelationshipEntity>(table => {
                table.Comment("用户关系表");
                table.Uint("user_id");
                table.Uint("link_id").Comment("被联系的人");
                table.Uint("type", 1).Default(0).Comment("具体关系");
                table.Timestamp(MigrationTable.COLUMN_CREATED_AT);
            });
            Append<BanAccountEntity>(table => {
                table.Id();
                table.Uint("user_id").Default(0);
                table.String("item_key", 100);
                table.Uint("item_type", 2).Default(0);
                table.Uint("platform_id").Default(0).Comment("平台id");
                table.Timestamps();
            });
            Append<EquityCardEntity>(table => {
                table.Comment("有期限的权益卡");
                table.Id();
                table.String("name", 32);
                table.String("icon");
                table.String("configure", 200).Default(string.Empty);
                table.Uint("status", 2).Default(0);
                table.Timestamps();
            });
            Append<UserEquityCardEntity>(table => {
                table.Comment("用户的权益卡");
                table.Id();
                table.Uint("user_id");
                table.Uint("card_id");
                table.Uint("exp").Default(0);
                table.Uint("status", 2).Default(0);
                table.Timestamp("expired_at");
                table.Timestamps();
            });
            Append<InviteCodeEntity>(table => {
                table.Comment("邀请码生成");
                table.Id();
                table.Uint("type", 1).Default(InviteRepository.TYPE_CODE);
                table.Uint("user_id").Default(0);
                table.Uint("amount").Default(1);
                table.Uint("invite_count").Default(0);
                table.String("token", 32);
                table.Timestamp("expired_at");
                table.Timestamps();
            });
            Append<InviteLogEntity>(table => {
                table.Comment("邀请记录");
                table.Id();
                table.Uint("user_id");
                table.Uint("parent_id").Default(0);
                table.Uint("code_id").Default(0);
                table.Uint("status", 1).Default(0);
                table.Timestamps();
            });
            CreateLog();
            CreateRole();
            CreateBulletin();
            AutoUp();
        }

        public override void Seed()
        {
            privilege.AddRole("administrator", "超级管理员");
            privilege.AddPermission("user_manage", "会员管理");
            privilege.AddPermission("system_manage", "系统配置");
            option.AddGroup("高级", () => {
                return new OptionConfigureItem[] {
                    new(
                        AuthRepository.OPTION_REGISTER_CODE,
                        "注册方式", "radio", "默认注册\n邀请码注册\n关闭注册",
                        2
                    ),
                    new(
                        AuthRepository.OPTION_OAUTH_CODE,
                        "开启第三登录",
                        "switch",
                        "0",
                        2
                    ),
                };
            });
        }

        private void CreateRole()
        {
            Append<RoleEntity>(table => {
                table.Id();
                table.String("name", 40).Unique();
                table.String("display_name", 100).Default(string.Empty);
                table.String("description").Default(string.Empty);
                table.Timestamps();
            });
            Append<UserRoleEntity>(table => {
                table.Uint("user_id");
                table.Uint("role_id");
            });
            Append<PermissionEntity>(table => {
                table.Id();
                table.String("name", 40).Unique();
                table.String("display_name", 100).Default(string.Empty);
                table.String("description").Default(string.Empty);
                table.Timestamps();
            });
            Append<RolePermissionEntity>(table => {
                table.Uint("role_id");
                table.Uint("permission_id");
            });
        }

        private void CreateBulletin()
        {
            Append<BulletinEntity>(table => {
                table.Id();
                table.String("title", 100);
                table.String("content");
                table.String("extra_rule").Default(string.Empty);
                table.Uint("type", 2).Default(0);
                table.Uint("user_id");
                table.Timestamps();
            });
            Append<BulletinUserEntity>(table => {
                table.Id();
                table.Uint("bulletin_id");
                table.Uint("status", 2).Default(0);
                table.Uint("user_id");
                table.Timestamps();
            });
        }

        private void CreateLog()
        {
            Append<AccountLogEntity>(table => {
                table.Comment("账户资金变动表");
                table.Id();
                table.Uint("user_id").Default(0);
                table.Uint("type", 1).Default(99);
                table.Uint("item_id").Default(0);
                table.Int("money").Comment("本次发生金额");
                table.Int("total_money").Comment("当前账户余额");
                table.Uint("status", 2).Default(0);
                table.String("remark").Default(string.Empty);
                table.Timestamps();
            });
            Append<CreditLogEntity>(table => {
                table.Comment("账户积分变动表");
                table.Id();
                table.Uint("user_id").Default(0);
                table.Uint("type", 1).Default(99);
                table.Uint("item_id").Default(0);
                table.Int("credits").Comment("本次发生积分");
                table.Int("total_credits").Comment("当前账户积分");
                table.Uint("status", 2).Default(0);
                table.String("remark").Default(string.Empty);
                table.Timestamps();
            });
            Append<LoginLogEntity>(table => {
                table.Comment("账户登录日志表");
                table.Id();
                table.String("ip", 120);
                table.Uint("user_id").Default(0);
                table.String("user", 100).Comment("登陆账户");
                table.Bool("status").Default(0);
                table.String("mode", 20).Default(AuthRepository.LOGIN_MODE_WEB);
                table.Uint("platform_id").Default(0).Comment("平台id");
                table.Timestamp(MigrationTable.COLUMN_CREATED_AT);
            });
            Append<ActionLogEntity>(table => {
                table.Comment("操作记录");
                table.Id();
                table.String("ip", 120);
                table.Uint("user_id");
                table.String("action", 30);
                table.String("remark").Default(string.Empty);
                table.Timestamp(MigrationTable.COLUMN_CREATED_AT);
            });
            Append<AdminLogEntity>(table => {
                table.Comment("管理员操作记录");
                table.Id();
                table.String("ip", 120);
                table.Uint("user_id");
                table.Uint("item_type", 2).Default(0);
                table.Uint("item_id").Default(0);
                table.String("action", 30);
                table.String("remark").Default(string.Empty);
                table.Timestamp(MigrationTable.COLUMN_CREATED_AT);
            });
            Append<ApplyLogEntity>(table => {
                table.Comment("用户申请记录");
                table.Id();
                table.Uint("user_id");
                table.Uint("type", 1).Default(0);
                table.Int("money").Default(0);
                table.String("remark").Default(string.Empty);
                table.Uint("status", 2).Default(0);
                table.Timestamps();
            });
        }
    }
}
