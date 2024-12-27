using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NetDream.Modules.Forum.Entities;

namespace NetDream.Modules.Forum.Migrations
{
    public class ThreadLogEntityTypeConfiguration : IEntityTypeConfiguration<ThreadLogEntity>
    {
        public void Configure(EntityTypeBuilder<ThreadLogEntity> builder)
        {
            builder.ToTable("bbs_thread_log", table => table.HasComment(""));
            builder.HasKey(i => i.Id);
            builder.Property(table => table.Id).HasColumnName("id");
            builder.Property(table => table.ItemType).HasColumnName("item_type").HasMaxLength(2).HasDefaultValue(0);
            builder.Property(table => table.ItemId).HasColumnName("item_id");
            builder.Property(table => table.UserId).HasColumnName("user_id");
            builder.Property(table => table.Action).HasColumnName("action");
            builder.Property(table => table.NodeIndex).HasColumnName("node_index").HasMaxLength(1).HasDefaultValue(0).HasComment("每一个回帖内部的节点");
            builder.Property(table => table.Data).HasColumnName("data").HasDefaultValue(string.Empty).HasComment("执行的参数");
            builder.Property(table => table.CreatedAt).HasColumnName("created_at");
        }
    }
}
