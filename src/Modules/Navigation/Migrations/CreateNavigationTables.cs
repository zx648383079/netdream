using NetDream.Shared.Interfaces;
using NetDream.Shared.Migrations;
using NetDream.Modules.Navigation.Repositories;
using NPoco;
using Modules.Navigation.Entities;

namespace NetDream.Modules.Navigation.Migrations
{
    public class CreateNavigationTables(IDatabase db, IPrivilegeManager privilege) : Migration(db)
    {
        public override void Up()
        {
            new SiteRepository(db).Tag().Migration(this);
            Append<CategoryEntity>(table => {
                table.Comment("站点分类表");
                table.Id();
                table.String("name", 30);
                table.String("icon").Default(string.Empty);
                table.Uint("parent_id").Default(0);
            }).Append<SiteEntity>(table => {
                table.Comment("站点表");
                table.Id();
                table.String("schema", 10).Default("https");
                table.String("domain", 100);
                table.String("name", 30);
                table.String("logo").Default(string.Empty);
                table.String("description").Default(string.Empty);
                table.Uint("cat_id").Default(0);
                table.Uint("user_id").Default(0);
                table.Uint("top_type", 1).Default(0).Comment("推荐类型");
                table.Uint("score", 1).Default(60).Comment("站点评分/百分制");
                table.Timestamps();
            }).Append<SiteScoringLogEntity>(table => {
                table.Comment("站点评分记录表");
                table.Id();
                table.Uint("site_id");
                table.Uint("user_id");
                table.Uint("score", 1).Comment("站点评分/百分制");
                table.Uint("last_score", 1).Comment("上次的评分");
                table.String("change_reason").Default(string.Empty).Comment("评分变动原因");
                table.Timestamp(MigrationTable.COLUMN_CREATED_AT);
            }).Append<PageEntity>(table => {
                table.Comment("网页表");
                table.Id();
                table.String("title", 30);
                table.String("description").Default(string.Empty);
                table.String("thumb").Default(string.Empty);
                table.String("link");
                table.String("site_id").Default(0);
                table.Uint("score", 1).Default(60).Comment("页面评分");
                table.Uint("user_id").Default(0);
                table.Timestamps();
            }).Append<KeywordEntity>(table => {
                table.Comment("关键字表");
                table.Id();
                table.String("word", 30);
                table.Uint("type", 1).Default(0).Comment("关键词类型：默认短尾词，长尾词");
            }).Append<PageKeywordEntity>(table => {
                table.Comment("网页包含关键字表");
                table.Uint("page_id");
                table.Uint("word_id");
                table.Bool("is_official").Default(0).Comment("是否为关键词官网");
            }).Append<CollectGroupEntity>(table => {
                table.Comment("收藏分组表");
                table.Id();
                table.String("name", 20);
                table.Uint("user_id").Default(0);
                table.Uint("position", 1).Default(5);
            }).Append<CollectEntity>(table => {
                table.Comment("收藏网页表");
                table.Id();
                table.String("name", 20);
                table.String("link");
                table.Uint("group_id").Default(0);
                table.Uint("user_id").Default(0);
                table.Uint("position", 1).Default(5);
                table.Timestamps();
            });
            AutoUp();
        }

        public override void Seed()
        {
            privilege.AddPermission("navigation_manage", "导航管理");
        }
    }
}
