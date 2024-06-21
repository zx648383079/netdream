using NetDream.Shared.Interfaces;
using NetDream.Shared.Migrations;
using NetDream.Shared.Repositories;
using NetDream.Modules.Blog.Entities;
using NetDream.Modules.Blog.Repositories;
using NPoco;

namespace NetDream.Modules.Blog.Migrations
{
    public class CreateBlogTables(
        IPrivilegeManager privilege,
        IGlobeOption option,
        LocalizeRepository localize, 
        IDatabase db) : Migration(db)
    {
    
        public override void Up()
        {
            Append<BlogEntity>(table => {
                table.Id();
                table.String("title", 200);
                table.String("description").Default(string.Empty);
                table.String("keywords").Default(string.Empty);
                table.Uint("parent_id").Default(0);
                table.String("programming_language", 20)
                    .Default(string.Empty).Comment("编程语言");
                localize.AddTableColumn(table);
                table.String("thumb").Default(string.Empty);
                table.Uint("edit_type", 1)
                    .Default(PublishRepository.EDIT_HTML).Comment("编辑器类型");
                table.Text("content");
                table.Uint("user_id");
                table.Uint("term_id");
                table.Bool("type").Default(PublishRepository.TYPE_ORIGINAL).Comment("原创或转载");
                table.Uint("recommend_count").Default(0);
                table.Uint("comment_count").Default(0);
                table.Uint("click_count").Default(0);
                table.Uint("open_type", 1)
                    .Default(PublishRepository.OPEN_PUBLIC).Comment("公开类型");
                table.String("open_rule", 20).Default(string.Empty).Comment("类型匹配的值");
                table.Uint("publish_status", 1)
                    .Default(PublishRepository.PUBLISH_STATUS_DRAFT).Comment("发布状态");
                table.Timestamps();
            });
            Append<BlogMetaEntity>(table => {
                table.Id();
                table.Uint("blog_id");
                table.String("name", 100);
                table.Text("content");
            });
            Append<TermEntity>(table => {
                table.Id();
                foreach (var lang in localize.LanguageAsColumnPrefix())
                {
                    table.String(lang + "name", 40).Nullable(!string.IsNullOrEmpty(lang));
                }
                table.Uint("parent_id").Default(0);
                table.String("keywords").Default(string.Empty);
                table.String("description").Default(string.Empty);
                table.String("thumb").Default(string.Empty);
                table.String("styles").Default(string.Empty).Comment("独立引入样式");
            });
            Append<CommentEntity>(table => {
                table.Id();
                table.String("content");
                table.String("extra_rule", 300).Default(string.Empty)
                    .Comment("内容的一些附加规则");
                table.String("name", 30).Default(string.Empty);
                table.String("email", 50).Default(string.Empty);
                table.String("url", 50).Default(string.Empty);
                table.Uint("parent_id").Default(0);
                table.Uint("position").Default(1);
                table.Uint("user_id").Default(0);
                table.Uint("blog_id");
                table.String("ip", 120).Default(string.Empty);
                table.String("agent").Default(string.Empty);
                table.Uint("agree_count").Default(0);
                table.Uint("disagree_count").Default(0);
                table.Uint("approved", 1).Default(2);
                table.Timestamp(MigrationTable.COLUMN_CREATED_AT);
            });
            Append<BlogLogEntity>(table => {
                table.Id();
                table.Uint("item_type", 1).Default(0);
                table.Uint("item_id");
                table.Uint("user_id");
                table.Uint("action");
                table.Timestamp(MigrationTable.COLUMN_CREATED_AT);
            });
            Append<TagEntity>(table => {
                table.Id();
                table.String("name", 40);
                table.String("description").Default(string.Empty);
                table.Uint("blog_count").Default(0);
            });
            Append<TagRelationshipEntity>(table => {
                table.Uint("tag_id");
                table.Uint("blog_id");
                table.Uint("position", 2).Default(99);
            });
            Append<BlogClickLogEntity>(table => {
                table.Id();
                table.Date("happen_day");
                table.Uint("blog_id");
                table.Uint("click_count").Default(0);
            });
            AutoUp();
        }

        public override void Seed()
        {
            privilege.AddPermission("blog_term_edit", "博客分类管理");
            option.AddGroup("博客设置", () => {
                return new OptionConfigureItem[] {
                    new("blog_list_view", "博客列表显示", "select", 
                        "无图\n左图\n右图", 2
                    ),
                    new("blog_comment", "博客评论", "select", 
                        "关闭\n开启\n仅登录", 2
                    ),
                    new("comment_approved", "评论审核", "switch", "关闭\n开启",
                        2
                    ),
                };
            });
        }
    }
}
