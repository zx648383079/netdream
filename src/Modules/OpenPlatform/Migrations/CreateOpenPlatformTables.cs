using NetDream.Shared.Interfaces;
using NetDream.Shared.Migrations;
using NetDream.Modules.OpenPlatform.Entities;
using NPoco;

namespace NetDream.Modules.OpenPlatform.Migrations
{
    internal class CreateOpenPlatformTables(IDatabase db, IPrivilegeManager privilege) : Migration(db)
    {

        public override void Up()
        {
            Append<PlatformEntity>(table => {
                table.Comment("第三方授权信息");
                table.Id();
                table.Uint("user_id");
                table.String("name", 20);
                table.String("description").Default(string.Empty).Comment("说明");
                table.Uint("type", 1).Default(0);
                table.String("domain", 50);
                table.String("appid", 12).Unique();
                table.Char("secret", 32);
                table.Uint("sign_type", 1).Default(0).Comment("签名方式");
                table.String("sign_key", 32).Default(string.Empty).Comment("签名密钥");
                table.Uint("encrypt_type", 1).Default(0).Comment("加密方式");
                table.Text("public_key").Nullable().Comment("密钥");
                table.String("rules").Default(string.Empty).Comment("允许访问的网址");
                table.Uint("status", 1).Default(0);
                table.Bool("allow_self").Default(0).Comment("是否允许后台用户自己添加");
                table.Timestamps();
            }).Append<UserTokenEntity>(table => {
                table.Comment("用户授权平台令牌");
                table.Id();
                table.Uint("user_id");
                table.Uint("platform_id");
                table.Text("token");
                table.Bool("is_self").Default(0).Comment("是否时用户后台添加的");
                table.Timestamp("expired_at").Comment("过期时间");
                table.Timestamps();
            }).Append<PlatformOptionEntity>(table => {
                table.Comment("平台一些第三方接口配置");
                table.Id();
                table.Uint("platform_id");
                table.String("store", 20).Comment("平台别名");
                table.String("name", 30).Comment("字段");
                table.Text("value").Nullable().Comment("配置值");
                table.Timestamps();
            });
            AutoUp();
        }

        public override void Seed()
        {
            privilege.AddPermission("open_manage", "开放应用管理");
        }
    }

}
