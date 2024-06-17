using NetDream.Core.Migrations;
using NetDream.Modules.Chat.Entities;
using NPoco;

namespace NetDream.Modules.Chat.Migrations
{
    public class CreateChatTables(IDatabase db) : Migration(db)
    {

        public override void Up()
        {
            Append<FriendEntity>(table => {
                table.Id();
                table.String("name", 50)
                    .Default(string.Empty).Comment("备注");
                table.Uint("classify_id").Default(1).Comment("分组/1为默认分组，0为黑名单");
                table.Uint("user_id").Comment("用户");
                table.Uint("belong_id").Comment("归属");
                table.Bool("status").Default(0).Comment("是否互相关注");
                table.Timestamps();
            }).Append<ApplyEntity>(table => {
                table.Id();
                table.Uint("item_type", 2).Default(0).Comment("申请类别");
                table.Uint("item_id").Comment("申请内容");
                table.String("remark").Default(string.Empty);
                table.Uint("user_id").Comment("申请人");
                table.Uint("status", 2).Default(0);
                table.Timestamps();
            }).Append<FriendClassifyEntity>(table => {
                table.Id().Ai(10);
                table.String("name", 100).Comment("分组名");
                table.Uint("user_id").Comment("用户");
                table.Timestamp(MigrationTable.COLUMN_CREATED_AT);
            }).Append<MessageEntity>(table => {
                table.Id();
                table.Uint("type", 2).Default(0);
                table.String("content", 400).Comment("内容");
                table.String("extra_rule", 400)
                    .Default(string.Empty).Comment("附加替换规则");
                table.Uint("item_id")
                    .Default(0).Comment("附加id");
                table.Uint("receive_id")
                    .Default(0).Comment("接收用户");
                table.Uint("group_id")
                    .Default(0).Comment("所属群");
                table.Uint("user_id").Comment("发送用户");
                table.Uint("status", 2).Default(0);
                table.SoftDeletes();
                table.Timestamps();
            }).Append<GroupEntity>(table => {
                table.Id();
                table.String("name", 50).Comment("群名");
                table.String("logo", 100).Comment("群LOGO");
                table.String("description").Default(string.Empty)
                    .Comment("群说明");
                table.Uint("user_id").Comment("用户");
                table.Timestamps();
            }).Append<GroupUserEntity>(table => {
                table.Id();
                table.Uint("group_id").Comment("群");
                table.Uint("user_id").Comment("用户");
                table.String("name", 50)
                    .Default(string.Empty).Comment("群备注");
                table.Uint("role_id").Default(0).Comment("管理员等级");
                table.Uint("status", 1)
                    .Default(5).Comment("用户状态/禁言或");
                table.Timestamps();
            }).Append<HistoryEntity>(table => {
                table.Id();
                table.Uint("item_type", 2);
                table.Uint("item_id").Comment("聊天历史");
                table.Uint("user_id").Comment("关联用户");
                table.Uint("unread_count").Comment("未读消息数量");
                table.Uint("last_message").Comment("最后一条消息");
                table.Timestamps();
            });
            AutoUp();
        }
    }
}
