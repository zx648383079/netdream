using Modules.Forum.Entities;
using NetDream.Core.Interfaces;
using NetDream.Core.Migrations;
using NPoco;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetDream.Modules.Forum.Migrations
{
    public class CreateForumTables(IDatabase db, IPrivilegeManager privilege) : Migration(db)
    {

        public override void Up()
        {
            Append<ForumEntity>(table => {
                table.Id();
                table.String("name", 100);
                table.String("thumb", 100).Default(string.Empty);
                table.String("description").Default(string.Empty);
                table.Uint("parent_id").Default(0);
                table.Uint("thread_count").Default(0).Comment("主题数");
                table.Uint("post_count").Default(0).Comment("回帖数");
                table.Uint("type", 2).Default(0);
                table.Uint("position", 2).Default(99);
                table.Timestamps();
            }).Append<ForumClassifyEntity>(table => {
                table.Id();
                table.String("name", 20);
                table.String("icon", 100).Default(string.Empty);
                table.Uint("forum_id").Default(0);
                table.Uint("position", 2).Default(99);
            }).Append<ForumModeratorEntity>(table => {
                table.Id();
                table.Uint("user_id");
                table.Uint("forum_id").Default(0);
                table.Uint("role_id").Default(0);
            }).Append<ThreadEntity>(table => {
                table.Id();
                table.Uint("forum_id");
                table.Uint("classify_id").Default(0);
                table.String("title", 200).Comment("主题");
                table.Uint("user_id").Comment("发送用户");
                table.Uint("view_count").Default(0).Comment("查看数");
                table.Uint("post_count").Default(0).Comment("回帖数");
                table.Uint("collect_count").Default(0).Comment("关注数");
                table.Bool("is_highlight").Default(0)
                    .Comment("是否高亮");
                table.Bool("is_digest").Default(0)
                    .Comment("是否精华");
                table.Bool("is_closed").Default(0)
                    .Comment("是否关闭");
                table.Uint("top_type", 1).Default(0)
                    .Comment("置顶类型，1 本版置顶 2 分类置顶 3 全局置顶");
                table.Bool("is_private_post").Default(0).Comment("是否仅楼主可见");
                table.Timestamps();
            }).Append<ThreadPostEntity>(table => {
                table.Id();
                table.MediumText("content");
                table.Uint("thread_id");
                table.Uint("user_id").Comment("用户");
                table.String("ip", 120);
                table.Uint("grade", 5)
                    .Default(0).Comment("回复的层级");
                table.Bool("is_invisible").Default(0)
                    .Comment("是否通过审核");
                table.Uint("agree_count").Default(0)
                    .Comment("赞成数");
                table.Uint("disagree_count").Default(0)
                    .Comment("不赞成数");
                table.Uint("status", 1).Default(0)
                    .Comment("帖子的状态");
                table.Timestamps();
            }).Append<ThreadLogEntity>(table => {
                table.Id();
                table.Uint("item_type", 2).Default(0);
                table.Uint("item_id");
                table.Uint("user_id");
                table.Uint("action");
                table.Uint("node_index", 1).Default(0).Comment("每一个回帖内部的节点");
                table.String("data").Default(string.Empty).Comment("执行的参数");
                table.Timestamp(MigrationTable.COLUMN_CREATED_AT);
            }).Append<ForumLogEntity>(table => {
                table.Id();
                table.Uint("item_type", 2).Default(0);
                table.Uint("item_id");
                table.Uint("user_id");
                table.Uint("action");
                table.String("data").Default(string.Empty).Comment("执行的参数");
                table.Timestamp(MigrationTable.COLUMN_CREATED_AT);
            });
            AutoUp();
        }

        public override void Seed()
        {
            privilege.AddPermission("forum_manage", "论坛管理");
        }
    }
}
