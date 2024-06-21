using Modules.Note.Entities;
using NetDream.Shared.Interfaces;
using NetDream.Shared.Migrations;
using NetDream.Modules.Note.Repositories;
using NPoco;

namespace NetDream.Modules.Note.Migrations
{
    public class CreateNoteTables(IDatabase db, IPrivilegeManager privilege) : Migration(db)
    {

        public override void Up()
        {
            Append<NoteEntity>(table => {
                table.Comment("便签");
                table.Id();
                table.String("content").Comment("内容");
                table.Uint("user_id");
                table.Bool("is_notice").Default(0).Comment("是否时站点公告");
                table.Uint("status", 1).Default(NoteRepository.STATUS_VISIBLE).Comment("发布状态,");
                table.Timestamp(MigrationTable.COLUMN_CREATED_AT);
            });
            AutoUp();
        }

        public override void Seed()
        {
            privilege.AddPermission("note_manage", "便签管理");
        }
    }
}
