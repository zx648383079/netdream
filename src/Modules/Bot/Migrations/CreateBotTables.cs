using Modules.Bot.Entities;
using NetDream.Shared.Interfaces;
using NetDream.Shared.Migrations;
using NetDream.Modules.Bot.Repositories;
using NPoco;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetDream.Modules.Bot.Migrations
{
    public class CreateBotTables(IDatabase db, IPrivilegeManager privilege) : Migration(db)
    {
        /**
         * Run the migrations.
         *
         * @return void
         */
        public override void Up()
        {
            InitEditor();
            InitBotTable();
            InitUserTable();
            InitMessageHistoryTable();
            InitMediaTable();

            Append<ReplyEntity>(table => {
                table.Comment("微信回复");
                table.Id();
                table.Uint("bot_id").Comment("所属微信公众号ID");
                table.String("event", 20).Comment("事件");
                table.String("keywords", 60).Default(string.Empty).Comment("关键词");
                table.Bool("match").Default(0).Comment("关键词匹配模式");
                table.Text("content").Comment("微信返回数据");
                table.Uint("type", 1).Default(0).Comment("素材类型");
                table.Bool("status").Default(1).Comment("激活");
                table.Timestamps();
            }).Append<MenuEntity>(table => {
                table.Comment("微信菜单");
                table.Id();
                table.Uint("bot_id").Comment("所属微信公众号ID");
                table.String("name", 100).Comment("菜单名称");
                table.Uint("type", 1).Default(0).Comment("菜单类型");
                table.String("content", 500).Default(string.Empty).Comment("菜单数据");
                table.Uint("parent_id").Default(0);
                table.Timestamps();
            }).Append<TemplateEntity>(table => {
                table.Comment("微信模板消息模板");
                table.Id();
                table.Uint("bot_id").Comment("所属微信公众号ID");
                table.String("template_id", 64).Comment("模板id");
                table.String("title", 100).Comment("标题");
                table.String("content").Comment("内容");
                table.String("example").Default(string.Empty).Comment("示例");
            }).Append<QrcodeEntity>(table => {
                table.Comment("微信二维码");
                table.Id();
                table.Uint("bot_id").Comment("所属微信公众号ID");
                table.String("name").Comment("使用用途");
                table.Bool("type").Default(0).Comment("永久或临时");
                table.Bool("scene_type").Default(0).Comment("数字或字符串");
                table.String("scene_str").Default(string.Empty).Comment("场景值");
                table.Uint("scene_id").Default(0).Comment("场景值ID，临时二维码时为32位非0整型，永久二维码时最大值为100000");
                table.Uint("expire_time").Default(0).Comment("过期事件/s");
                table.String("qr_url").Default(string.Empty).Comment("二维码地址");
                table.String("url").Default(string.Empty).Comment("解析后的地址");
                table.Timestamps();
            });
            AutoUp();
        }

        public override void Seed()
        {
            privilege.AddPermission("bot_manage", "Bot管理");
        }

        public void InitEditor()
        {
            Append<EditorTemplateEntity>(table => {
                table.Comment("微信图文模板");
                table.Id();
                table.Uint("type", 2).Default(0).Comment("类型：素材、节日、行业");
                table.Uint("cat_id").Default(0).Comment("详细分类");
                table.String("name", 100).Comment("模板标题");
                table.Text("content").Comment("模板内容");
                table.Timestamps();
            }).Append<EditorTemplateCategoryEntity>(table => {
                table.Comment("微信图文模板分类");
                table.Id();
                table.String("name", 20).Comment("模板标题");
                table.Uint("parent_id").Default(0);
            });
        }

        /**
         * 公众号表
         */
        public void InitBotTable()
        {
            Append<BotEntity>(table => {
                table.Id();
                table.Uint("user_id");
                table.Uint("platform_type", 1)
                    .Default(BotRepository.PLATFORM_TYPE_WX).Comment("公众号平台类型");
                table.String("name", 40).Comment("公众号名称");
                table.String("token", 32).Comment("微信服务访问验证token");
                table.String("access_token").Default(string.Empty).Comment("访问微信服务验证token");
                table.String("account", 30).Default(string.Empty).Comment("微信号");
                table.String("original", 40).Default(string.Empty).Comment("原始ID");
                table.Uint("type", 1).Default(0).Comment("公众号类型");
                table.String("appid", 50).Default(string.Empty).Comment("公众号的AppID");
                table.String("secret", 50).Default(string.Empty).Comment("公众号的AppSecret");
                table.String("aes_key", 43).Default(string.Empty).Comment("消息加密秘钥EncodingAesKey");
                table.String("avatar").Default(string.Empty).Comment("头像地址");
                table.String("qrcode").Default(string.Empty).Comment("二维码地址");
                table.String("address").Default(string.Empty).Comment("所在地址");
                table.String("description").Default(string.Empty).Comment("公众号简介");
                table.String("username", 40).Default(string.Empty).Comment("微信官网登录名");
                table.String("password", 32).Default(string.Empty).Comment("微信官网登录密码");
                table.Bool("status").Default(BotRepository.STATUS_INACTIVE).Comment("状态");
                table.Timestamps();
            });
        }

        /**
         * 粉丝用户表
         */
        public void InitUserTable()
        {
            // 公众号粉丝详情表
            Append<UserEntity>(table => {
                table.Id().Comment("粉丝ID");
                table.String("openid", 50).Comment("微信ID");
                table.String("nickname", 20).Default(string.Empty).Comment("昵称");
                table.Bool("sex").Default(0).Comment("性别");
                table.String("city", 40).Default(string.Empty).Comment("所在城市");
                table.String("country", 40).Default(string.Empty).Comment("所在国家");
                table.String("province", 40).Default(string.Empty).Comment("所在省");
                table.String("language", 40).Default(string.Empty).Comment("用户语言");
                table.String("avatar").Comment("用户头像");
                table.Timestamp("subscribe_at");
                table.String("union_id", 30).Default(string.Empty).Comment("微信ID");
                table.String("remark").Default(string.Empty).Comment("备注");
                table.Uint("group_id").Default(0);
                table.Uint("bot_id").Comment("所属微信公众号ID");
                table.String("note_name", 20).Default(string.Empty).Comment("备注名");
                table.Bool("status").Default(BotRepository.STATUS_SUBSCRIBED)
                    .Comment("关注状态");
                table.Bool("is_black").Default(0).Comment("是否是黑名单");
                table.Timestamps();
            }).Append<UserGroupEntity>(table => {
                table.Id().Comment("分组");
                table.Uint("bot_id").Comment("所属微信公众号ID");
                table.String("name", 20).Comment("名称");
                table.String("tag_id", 20).Default(string.Empty).Comment("公众平台标签id");
            });
        }
        /**
         * 消息记录表
         */
        public void InitMessageHistoryTable()
        {
            Append<MessageHistoryEntity>(table => {
                table.Id();
                table.Uint("bot_id").Comment("所属微信公众号ID");
                table.Uint("type", 1).Default(0).Comment("消息类型");
                table.Uint("item_type", 1).Default(0).Comment("发送类型");
                table.Uint("item_id").Default(0).Comment("相应规则ID");
                table.String("from", 50).Comment("请求用户ID");
                table.String("to", 50).Comment("相应用户ID");
                table.Text("content").Nullable().Comment("消息体内容");
                table.Bool("is_mark").Default(0).Comment("是否标记");
                table.Timestamp(MigrationTable.COLUMN_CREATED_AT);
            });
        }
        /**
         * 素材表
         */
        public void InitMediaTable()
        {
            Append<MediaEntity>(table => {
                table.Id();
                table.Uint("bot_id").Comment("所属微信公众号ID");
                table.String("type", 10).Comment("素材类型");
                table.Bool("material_type").Default(MediaRepository.MATERIAL_PERMANENT)
                    .Comment("素材类别:永久/临时");
                table.String("title", 200).Comment("素材标题");
                table.String("thumb", 200).Default(string.Empty).Comment("图文的封面");
                table.Bool("show_cover").Default(0).Comment("显示图文的封面");
                table.Bool("open_comment").Default(0).Comment("图文是否可以评论");
                table.Bool("only_comment").Default(0).Comment("图文可以评论的人");
                table.LongText("content").Comment("素材内容");
                table.Uint("parent_id").Default(0).Comment("图文父id");
                table.String("media_id", 100).Default(string.Empty).Comment("素材ID");
                table.String("url").Default(string.Empty).Comment("图片的url");
                table.Uint("publish_status", 1).Default(0).Comment("当前发布的状态");
                table.String("publish_id", 50).Default(string.Empty).Comment("当前发布的id, publish_status=6,为草稿=7,为发布中publish_id；为草稿=8,为发布成功，article_id");
                table.Timestamp("expired_at").Comment("临时素材的过期时间");
                table.Timestamps();
            });
        }
    }
}
