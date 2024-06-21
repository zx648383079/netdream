using NetDream.Shared.Migrations;
using NetDream.Modules.Contact.Entities;
using NPoco;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetDream.Modules.Contact.Migrations
{
    public class CreateContactTables(IDatabase db) : Migration(db)
    {

        public override void Up()
        {
            Append<FeedbackEntity>(table => {
                table.Comment("留言");
                table.Id();
                table.String("name", 20);
                table.String("email", 50).Default(string.Empty);
                table.String("phone", 30).Default(string.Empty);
                table.String("content").Default(string.Empty);
                table.Bool("status").Default(0);
                table.Bool("open_status").Default(0).Comment("是否前台可见");
                table.Uint("user_id").Default(0);
                table.Timestamps();
            }).Append<FriendLinkEntity>(table => {
                table.Comment("友情链接");
                table.Id();
                table.String("name", 20);
                table.String("url", 50);
                table.String("logo", 200).Default(string.Empty);
                table.String("brief").Default(string.Empty);
                table.String("email", 100).Default(string.Empty);
                table.Bool("status").Default(0);
                table.Uint("user_id").Default(0);
                table.Timestamps();
            }).Append<SubscribeEntity>(table => {
                table.Comment("邮箱订阅");
                table.Id();
                table.String("email", 50).Unique();
                table.String("name", 30).Default(string.Empty).Comment("称呼");
                table.Bool("status").Default(0);
                table.Timestamps();
            }).Append<ReportEntity>(table => {
                table.Comment("举报和投诉");
                table.Id();
                table.String("email", 50).Default(string.Empty);
                table.Uint("item_type", 1).Default(0);
                table.Uint("item_id").Default(0);
                table.Uint("type", 1).Default(0);
                table.String("title").Default(string.Empty);
                table.String("content").Default(string.Empty);
                table.String("files").Default(string.Empty);
                table.Bool("status").Default(0);
                table.Uint("user_id").Default(0);
                table.String("ip", 120).Default(string.Empty);
                table.Timestamps();
            });
            AutoUp();
        }
    }
}
