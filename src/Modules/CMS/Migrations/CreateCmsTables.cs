using NetDream.Shared.Extensions;
using NetDream.Shared.Interfaces;
using NetDream.Shared.Migrations;
using NetDream.Modules.CMS.Entities;
using NetDream.Modules.CMS.Repositories;
using NPoco;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetDream.Modules.CMS.Migrations
{
    class CreateCmsTables(IDatabase db, IPrivilegeManager privilege) : Migration(db)
    {

        public override void Up()
        {
            Append<ModelFieldEntity>(table => {
                table.Id();
                table.String("name", 100);
                table.String("field", 100);
                table.Uint("model_id");
                table.String("type", 20).Default("text");
                table.Uint("length", 10).Default(0);
                table.Uint("position", 2).Default(99);
                table.Uint("form_type", 2).Default(0);
                table.Bool("is_main").Default(0);
                table.Bool("is_required").Default(1);
                table.Bool("is_search").Default(0).Comment("是否能搜索");
                table.Bool("is_disable").Default(0).Comment("禁用/启用");
                table.Bool("is_system").Default(0)
                    .Comment("系统自带禁止删除");
                table.String("match").Default(string.Empty);
                table.String("tip_message").Default(string.Empty);
                table.String("error_message").Default(string.Empty);
                table.String("tab_name", 4).Default(string.Empty).Comment("编辑组名");
                table.Text("setting").Nullable();
            }).Append<ModelEntity>(table => {
                table.Id();
                table.String("name", 100);
                table.String("table", 100);
                table.Uint("type", 2).Default(0);
                table.Uint("position", 2).Default(99);
                table.Uint("child_model").Default(0).Comment("分集模型");
                table.String("category_template", 20).Default(string.Empty);
                table.String("list_template", 20).Default(string.Empty);
                table.String("show_template", 20).Default(string.Empty);
                table.String("edit_template", 20).Default(string.Empty);
                table.Text("setting").Nullable();
            }).Append<GroupEntity>(table => {
                table.Id();
                table.String("name", 20);
                table.Uint("type", 2).Default(0);
                table.String("description").Default(string.Empty);
            }).Append<LinkageEntity>(table => {
                table.Id();
                table.String("name", 100);
                table.Uint("type", 2).Default(0);
                table.Char("code", 20);
                table.String("language", 10).Default(string.Empty);
            }).Append<LinkageDataEntity>(table => {
                table.Id();
                table.Uint("linkage_id");
                table.String("name", 100);
                table.Uint("parent_id").Default(0);
                table.Uint("position", 2).Default(99);
                table.String("description").Default(string.Empty);
                table.String("thumb").Default(string.Empty);
                table.String("full_name", 200).Default(string.Empty);
            }).Append<SiteEntity>(table => {
                table.Id();
                table.String("title");
                table.String("keywords").Default(string.Empty);
                table.String("description").Default(string.Empty);
                table.String("logo").Default(string.Empty);
                table.String("language", 10).Default(string.Empty);
                table.String("theme", 100);
                table.String("match_rule", 100).Default(string.Empty);
                table.Bool("is_default").Default(0);
                table.Uint("status", 1).Default(SiteRepository.PUBLISH_STATUS_POSTED)
                    .Comment("发布状态");
                table.Text("options").Nullable();
                table.Timestamps();
            }).Append<RecycleBinEntity>(table => {
                table.Id();
                table.Uint("site_id").Default(0);
                table.Uint("model_id").Default(0);
                table.Uint("item_type", 1).Default(0);
                table.Uint("item_id");
                table.Uint("user_id").Comment("删除者");
                table.String("title");
                table.String("remark").Default(string.Empty);
                table.Text("data");
                table.Timestamp(MigrationTable.COLUMN_CREATED_AT);
            });
            AutoUp();
        }

        public override void Seed()
        {
            privilege.AddPermission(CMSRepository.MANAGE_ROLE, "CMS管理");
            if (db.FindCount<SiteEntity>(string.Empty) > 0)
            {
                return;
            }
            //CMSRepository.GenerateSite(SiteModel.Create([
            //    "title" => "默认站点",
            //    "keywords" => string.Empty,
            //    "description" => string.Empty,
            //    "theme" => "default",
            //    "match_type" => 0,
            //    "match_rule" => string.Empty,
            //    "is_default" => 1,
            //]));
        }
    }
}
