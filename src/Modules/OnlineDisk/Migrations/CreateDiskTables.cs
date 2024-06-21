using NetDream.Modules.OnlineDisk.Entities;
using NetDream.Shared.Migrations;
using NetDream.Modules.OnlineDisk.Repositories;
using NPoco;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetDream.Modules.OnlineDisk.Migrations
{
    public class CreateDiskTables(IDatabase db, DiskRepository repository) : Migration(db)
    {
        /**
         * Run the migrations.
         *
         * @return void
         */
        public override void Up()
        {
            if (repository.UseDistributed)
            {
                Append<ServerEntity>(table => {
                    table.Id();
                    table.String("token");
                    table.String("ip", 120);
                    table.String("port", 6);
                    table.Bool("can_upload");
                    table.String("upload_url");
                    table.String("download_url");
                    table.String("ping_url");
                    table.Uint("file_count");
                    table.Uint("status", 1).Default(0);
                    table.Timestamps();
                }).Append<ServerFileEntity>(table => {
                    table.Uint("server_id");
                    table.Uint("file_id");
                }).Append<ClientFileEntity>(table => {
                    table.Id();
                    table.String("name", 100);
                    table.String("extension", 20);
                    table.Char("md5", 32);
                    table.String("location", 200);
                    table.String("size", 50).Default(0);
                    table.Timestamps();
                });
            }
            Append<DiskEntity>(table => {
                table.Id();
                table.String("name", 100);
                table.String("extension", 20);
                table.Uint("file_id").Default(0);
                table.Uint("user_id");
                table.Uint("left_id").Default(0);
                table.Uint("right_id").Default(0);
                table.Uint("parent_id").Default(0);
                table.SoftDeletes();
                table.Timestamps();
            }).Append<FileEntity>(table => {
                table.Id();
                table.String("name", 100);
                table.String("extension", 20);
                table.Char("md5", 32);
                table.String("location", 200);
                table.String("thumb", 200).Default(string.Empty)
                    .Comment("预览图");
                table.String("size", 50).Default("0");
                table.SoftDeletes();
                table.Timestamps();
            }).Append<ShareEntity>(table => {
                table.Id();
                table.String("name", 100);
                table.Uint("mode", 2).Default(DiskRepository.SHARE_PUBLIC);
                table.String("password", 20).Default(string.Empty);
                table.Uint("user_id");
                table.Timestamp("death_at");
                table.Uint("view_count").Default(0);
                table.Uint("down_count").Default(0);
                table.Uint("save_count").Default(0);
                table.Timestamps();
            }).Append<ShareFileEntity>(table => {
                table.Id();
                table.Uint("disk_id");
                table.Uint("share_id");
            }).Append<ShareUserEntity>(table => {
                table.Id();
                table.Uint("user_id");
                table.Uint("share_id");
            });
            AutoUp();
        }
    }
}
