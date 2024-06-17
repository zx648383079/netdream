using NetDream.Core.Extensions;
using NetDream.Core.Interfaces;
using NetDream.Core.Migrations;
using NetDream.Modules.Book.Entities;
using NetDream.Modules.Book.Repositories;
using NPoco;

namespace NetDream.Modules.Book.Migrations
{
    internal class CreateBookTables(IPrivilegeManager privilege, IDatabase db) : Migration(db)
    {

        public override void Up()
        {
            var repository = new BookRepository(db);
            repository.Tag().Migration(this);
            repository.Log().Migration(this);
            repository.ClickLog().Migration(this);
            Append<BookEntity>(table => {
                table.Comment("小说");
                table.Id();
                table.String("name", 100).Unique().Comment("书名");
                table.String("cover", 200).Default(string.Empty).Comment("封面");
                table.String("description", 200).Default(string.Empty).Comment("简介");
                table.Uint("author_id").Default(0).Comment("作者");
                table.Uint("user_id").Default(0);
                table.Uint("classify", 2).Default(0).Comment("小说分级");
                table.Uint("cat_id").Default(0).Comment("分类");
                table.Uint("size").Default(0).Comment("总字数");
                table.Uint("click_count").Default(0).Comment("点击数");
                table.Uint("recommend_count")
                    .Default(0).Comment("点击数");
                table.Timestamp("over_at").Comment("完本日期");
                table.Uint("status", 2).Default(0);
                table.Uint("source_type", 2).Default(0).Comment("来源类型，原创或转载");
                table.SoftDeletes();
                table.Timestamps();
            }).Append<BookMetaEntity>(table => {
                table.Comment("小说附加信息");
                table.Id();
                table.Uint("target_id");
                table.String("name", 50);
                table.String("content").Default(string.Empty);
            }).Append<BookSourceSiteEntity>(table => {
                table.Comment("小说来源站点");
                table.Id();
                table.String("name", 30).Comment("站点名");
                table.String("url", 100).Comment("网址");
                table.Timestamps();
            }).Append<BookSourceEntity>(table => {
                table.Comment("小说来源");
                table.Id();
                table.Uint("book_id");
                table.Uint("size_id");
                table.String("url", 200).Comment("来源网址");
                table.SoftDeletes();
                table.Timestamps();
            }).Append<ChapterEntity>(table => {
                table.Comment("小说章节");
                table.Id();
                table.Uint("book_id");
                table.Uint("type", 1).Default(BookRepository.CHAPTER_TYPE_FREE_CHAPTER).Comment("章节类型，是分卷还是章节");
                table.String("title", 200).Comment("标题");
                table.Uint("parent_id").Default(0);
                table.Uint("price").Default(0);
                table.Uint("status", 2).Default(0);
                table.Uint("position", 4).Default(99);
                table.Uint("size").Default(0).Comment("字数");
                table.SoftDeletes();
                table.Timestamps();
            }).Append<ChapterBodyEntity>(table => {
                table.Comment("小说章节内容");
                table.Id();
                table.LongText("content").Comment("内容");
            }).Append<BookCategoryEntity>(table => {
                table.Comment("小说分类");
                table.Id();
                table.String("name", 100).Unique().Comment("分类名");
                table.Timestamp(MigrationTable.COLUMN_CREATED_AT);
            }).Append<BookAuthorEntity>(table => {
                table.Comment("小说作者");
                table.Id();
                table.String("name", 100).Unique().Comment("作者名");
                table.String("avatar", 200).Default(string.Empty).Comment("作者头像");
                table.String("description", 200).Default(string.Empty).Comment("简介");
                table.Uint("user_id").Default(0);
                table.Uint("status", 2).Default(0);
                table.Timestamps();
            }).Append<BookHistoryEntity>(table => {
                table.Comment("小说阅读历史");
                table.Id();
                table.Uint("user_id");
                table.Uint("book_id");
                table.Uint("chapter_id").Default(0);
                table.Uint("progress", 1).Default(0);
                table.Uint("source_id").Default(0);
                table.Timestamps();
            }).Append<BookBuyLogEntity>(table => {
                table.Comment("小说购买记录");
                table.Id();
                table.Uint("book_id");
                table.Uint("chapter_id");
                table.Uint("user_id");
                table.Timestamp(MigrationTable.COLUMN_CREATED_AT);
            }).Append<BookRoleEntity>(table => {
                table.Comment("小说角色");
                table.Id();
                table.Uint("book_id");
                table.String("name", 50);
                table.String("avatar", 200).Default(string.Empty);
                table.String("description", 200).Default(string.Empty);
                table.String("character", 20).Default(string.Empty).Comment("身份：主角或");
                table.String("x", 20).Default(string.Empty);
                table.String("y", 20).Default(string.Empty);
            }).Append<RoleRelationEntity>(table => {
                table.Comment("小说角色关系");
                table.Id();
                table.Uint("role_id");
                table.String("title", 50);
                table.Uint("role_link");
            }).Append<BookListEntity>(table => {
                table.Comment("书单");
                table.Id();
                table.Uint("user_id");
                table.String("title", 50);
                table.String("description", 200).Default(string.Empty);
                table.Uint("book_count").Default(0);
                table.Uint("click_count").Default(0);
                table.Uint("collect_count").Default(0);
                table.Timestamps();
            }).Append<ListItemEntity>(table => {
                table.Comment("书单书籍");
                table.Id();
                table.Uint("list_id");
                table.Uint("book_id");
                table.String("remark", 200).Default(string.Empty);
                table.Uint("star", 1).Default(10);
                table.Uint("agree_count").Default(0);
                table.Uint("disagree_count").Default(0);
                table.Timestamps();
            });
            AutoUp();
        }

        public override void Seed()
        {
            privilege.AddPermission("book_manage", "书籍管理");
            if (db.FindCount<BookCategoryEntity>(string.Empty) > 0)
            {
                return;
            }
            db.InsertBatch(new BookCategoryEntity[] {
                new("玄幻"),
                new("奇幻"),
                new("仙侠"),
                new("武侠"),
                new("都市"),
                new("言情"),
                new("穿越"),
                new("历史"),
                new("科幻"),
                new("灵异"),
                new("同人"),
                new("网游"),
            });
            db.Insert(new BookAuthorEntity("未知", 1));
        }
    }
}
