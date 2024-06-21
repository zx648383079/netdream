using Modules.MicroBlog.Entities;
using NetDream.Shared.Interfaces;
using NetDream.Shared.Migrations;
using NetDream.Modules.MicroBlog.Repositories;
using NPoco;

namespace NetDream.Modules.MicroBlog.Migrations
{
    public class CreateMicroBlogTables(IDatabase db, IPrivilegeManager privilege, IGlobeOption option) : Migration(db)
    {
        public override void Up()
        {
            new MicroRepository(db).Comment().Migration(this);
            Append<MicroBlogEntity>(table => {
                table.Id();
                table.Uint("user_id");
                table.String("content", 140);
                table.String("extra_rule", 500).Default(string.Empty)
                    .Comment("内容的一些附加规则");
                table.Uint("open_type", 1).Default(0);
                table.Uint("recommend_count").Default(0).Comment("推荐数");
                table.Uint("collect_count").Default(0).Comment("收藏数");
                table.Uint("forward_count").Default(0).Comment("转发数");
                table.Uint("comment_count").Default(0).Comment("评论数");
                table.Uint("forward_id").Default(0).Comment("转发的源id");
                table.String("source", 30).Default(string.Empty).Comment("来源");
                table.Timestamps();
            }).Append<AttachmentEntity>(table => {
                table.Id();
                table.Uint("micro_id");
                table.String("thumb");
                table.String("file");
            }).Append<TopicEntity>(table => {
                table.Id();
                table.String("name", 200);
                table.Uint("user_id");
                table.Timestamps();
            }).Append<BlogTopicEntity>(table => {
                table.Id();
                table.Uint("micro_id");
                table.Uint("topic_id");
            });
            AutoUp();
        }

        public override void Seed()
        {
            privilege.AddPermission("micro_manage", "微博管理");
            option.AddGroup("微博客设置", () => {
                return new OptionConfigureItem[] {
                    new("micro_time_limit", "发布间隔/秒", "text", "300", 2),
                };
            });
        }
    }
}
