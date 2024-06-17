using Modules.AdSense.Entities;
using NetDream.Core.Extensions;
using NetDream.Core.Interfaces;
using NetDream.Core.Migrations;
using NetDream.Modules.AdSense.Repositories;
using NPoco;

namespace NetDream.Modules.AdSense.Migrations
{
    public class CreateAdTables(IPrivilegeManager privilege, IDatabase db) : Migration(db)
    {

        public override void Up()
        {
            Append<AdEntity>(table => {
                table.Id();
                table.String("name", 30);
                table.Uint("position_id");
                table.Uint("type", 1).Default(AdRepository.TYPE_TEXT);
                table.String("url");
                table.String("content");
                table.Timestamp("start_at");
                table.Timestamp("end_at");
                table.Bool("status").Default(1).Comment("是否启用广告");
                table.Timestamps();
            });
            Append<PositionEntity>(table => {
                table.Id();
                table.String("name", 30);
                table.String("code", 20).Unique().Comment("调用广告位的代码");
                table.Bool("auto_size").Default(1).Comment("自适应");
                table.Bool("source_type").Default(0).Comment("广告来源");
                table.String("width", 10).Default(string.Empty);
                table.String("height", 10).Default(string.Empty);
                table.String("template", 500).Default(string.Empty);
                table.Bool("status").Default(1).Comment("是否启用广告位");
                table.Timestamps();
            });
            AutoUp();
        }

        public override void Seed()
        {
            privilege.AddPermission("ad_manage", "广告管理");
            if (db.FindCount<int, PositionEntity>(string.Empty) > 0)
            {
                return;
            }
            var data = new PositionEntity[] {
                new("banner", "首页Banner"),
                new("mobile_banner", "手机端首页Banner"),
                new("home_notice", "首页通知栏"),
                new ("home_floor", "首页楼层"),
                new ("app_list", "APP列表页"),
                new("app_detail", "APP详情页"),
                new("blog_list", "Blog列表页"),
                new("blog_detail", "Blog详情页头部"),
                new("blog_inner", "Blog详情页内容中"),
                new("res_list", "资源列表页"),
                new("res_detail", "资源详情页"),
                new("cms_article", "CMS文章详情页"),
                new("bbs_list", "论坛列表页"),
                new("bbs_thread", "论坛帖子页"),
                new("book_chapter", "小说章节页"),
                new("book_detail", "小说详情页"),
            };
            foreach (var item in data)
            {
                item.Template = "{each}{.content}{/each}";
            }
            db.InsertBatch(data);
        }
    }
}
