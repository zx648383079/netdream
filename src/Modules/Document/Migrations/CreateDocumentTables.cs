using NetDream.Shared.Interfaces;
using NetDream.Shared.Migrations;
using NetDream.Modules.Document.Entities;
using NetDream.Modules.Document.Repositories;
using NPoco;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetDream.Modules.Document.Migrations
{
    public class CreateDocumentTables(IDatabase db, IPrivilegeManager privilege) : Migration(db)
    {
        public override void Up()
        {
            new ProjectRepository(db).Comment().Migration(this);
            Append<CategoryEntity>(table => {
                table.Comment("分类");
                table.Id();
                table.String("name", 30);
                table.String("icon").Default(string.Empty);
                table.Uint("parent_id").Default(0);
            }).Append<ProjectEntity>(table => {
                table.Comment("项目表");
                table.Id();
                table.Uint("user_id");
                table.Uint("cat_id").Default(0);
                table.String("name", 35).Comment("项目名");
                table.String("cover", 200).Default(string.Empty)
                    .Comment("项目封面");
                table.Uint("type", 1).Default(0).Comment("文档类型");
                table.String("description").Default(string.Empty).Comment("描述");
                table.Text("environment").Nullable().Comment("环境域名,json字符串");
                table.Uint("status", 2).Default(0).Comment("是否可见");
                table.SoftDeletes();
                table.Timestamps();
            }).Append<ProjectVersionEntity>(table => {
                table.Comment("项目版本表");
                table.Id();
                table.Uint("project_id");
                table.String("name", 20).Comment("版本号");
                table.Timestamps();
            }).Append<ApiEntity>(table => {
                table.Comment("项目接口表");
                table.Id();
                table.String("name", 35).Comment("接口名");
                table.Bool("type").Default(0).Comment("是否有内容,0为有内容");
                table.String("method", 10).Default("POST").Comment("请求方式");
                table.String("uri").Default(string.Empty).Comment("接口地址");
                table.Uint("project_id").Comment("项目");
                table.Uint("version_id").Default(0).Comment("版本");
                table.String("description").Default(string.Empty).Comment("接口简介");
                table.Uint("parent_id").Default(0);
                table.Timestamps();
            }).Append<PageEntity>(table => {
                table.Comment("项目普通文档页");
                table.Id();
                table.String("name", 35).Comment("文档");
                table.Uint("project_id").Comment("项目");
                table.Uint("version_id").Default(0).Comment("版本");
                table.Uint("parent_id").Default(0);
                table.Bool("type").Default(0).Comment("是否有内容,0为有内容");
                table.Text("content").Nullable().Comment("内容");
                table.Timestamps();
            }).Append<FieldEntity>(table => {
                table.Comment("项目字段表");
                table.Id();
                table.String("name", 50).Comment("接口名称");
                table.String("title", 50).Default(string.Empty).Comment("接口标题");
                table.Bool("is_required").Default(1).Comment("是否必传");
                table.String("default_value").Default(string.Empty).Comment("默认值");
                table.String("mock").Default(string.Empty).Comment("mock规则");
                table.Uint("parent_id").Default(0);
                table.Uint("api_id").Comment("接口id");
                table.Uint("kind", 2).Default(1).Comment("参数类型，1:请求字段 2:响应字段 3:header字段");
                table.String("type", 10).Default(string.Empty).Comment("字段类型");
                table.Text("remark").Nullable().Comment("备注");
                table.Timestamps();
            });
            AutoUp();
        }

        public override void Seed()
        {
            privilege.AddRole("doc_admin", "文档管理员");
        }
    }
}
