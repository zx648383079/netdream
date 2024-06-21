using NetDream.Shared.Extensions;
using NetDream.Shared.Interfaces;
using NetDream.Shared.Migrations;
using NetDream.Shared.Providers;
using NetDream.Shared.Repositories;
using NetDream.Modules.SEO.Entities;
using NetDream.Modules.SEO.Models;
using NPoco;

namespace NetDream.Modules.SEO.Migrations
{
    public class CreateSEOTables(IDatabase db, IGlobeOption option, LocalizeRepository localize) : Migration(db)
    {

        public override void Up()
        {
            StorageProvider.PrivateStore(db).Migration(this);
            Append<OptionEntity>(table => {
                table.Comment("全局设置");
                table.Id();
                table.String("name", 20);
                table.String("code", 50).Default(string.Empty);
                table.Uint("parent_id").Default(0);
                table.String("type", 20).Default("text");
                table.Uint("visibility", 1).Default(1).Comment("是否对外显示, 0 页面不可见，1 编辑可见 2 前台可见");
                table.String("default_value").Default(string.Empty).Comment("默认值或候选值");
                table.Text("value").Nullable();
                table.Uint("position", 2).Default(99);
            }).Append<BlackWordEntity>(table => {
                table.Comment("违禁词");
                table.Id();
                table.String("words");
                table.String("replace_words").Default(string.Empty);
            }).Append<EmojiEntity>(table => {
                table.Comment("表情");
                table.Id();
                table.Uint("cat_id");
                table.String("name", 30);
                table.Uint("type", 1).Default(0).Comment("图片或文字");
                table.String("content").Default(string.Empty);
            }).Append<EmojiCategoryEntity>(table => {
                table.Comment("表情分类");
                table.Id();
                table.String("name", 20);
                table.String("icon").Default(string.Empty);
            }).Append<AgreementEntity>(table => {
                table.Comment("服务协议");
                table.Id();
                table.String("name", 50);
                table.String("title", 100);
                localize.AddTableColumn(table);
                table.String("description", 500).Default(string.Empty);
                table.MediumText("content");
                table.Uint("status", 1).Default(0);
                table.Timestamps();
            });
            AutoUp();
        }

        public override void Seed()
        {
            if (db.FindCount<OptionModel>(string.Empty) > 0)
            {
                return;
            }
            option.AddGroup("基本", () => {
                return new OptionConfigureItem[] {
                    new("site_title", "站点名", 2),
                    new("site_keywords", "站点关键字", 2),
                    new("site_description", "站点介绍", "textarea", 2),
                    new("site_logo", "站点LOGO", "image", 2),
                    new("site_icp_beian", "ICP备案号", 2),
                    new("site_pns_beian", "公网安备案号", 2),
                };
            });
            option.AddGroup("上传", () => {
                return new OptionConfigureItem[] {
                    new("upload_add_water", "添加水印", "switch", "0", 1),
                    new("upload_water_text", "水印文字", "text", 1),
                    new("upload_water_position", "水印位置", "select", "左上\n右上\n左下\n右下",
                        1)
                };
            });
            option.AddGroup("高级", () => {
                return new OptionConfigureItem[] {
                    new("site_close", "关站", "switch", "0", 2),
                    new("site_close_tip", "关站说明", "basic_editor", 2),
                    new("site_close_retry", "预计开站时间", "text", 2),
                    new("site_gray", "开启灰度", "switch", "0", 2),
                };
            });
        }

    }
}
