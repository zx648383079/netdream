using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NetDream.Modules.Chat.Entities;

namespace NetDream.Modules.Chat.Migrations
{
    public class ApplyEntityTypeConfiguration : IEntityTypeConfiguration<ApplyEntity>
    {
        public void Configure(EntityTypeBuilder<ApplyEntity> builder)
        {
            builder.ToTable("chat_apply", table => table.HasComment(""));
            builder.HasKey("id");
            builder.Property(table => table.Id).HasColumnName("id");
            builder.Property(table => table.ItemType).HasColumnName("item_type").HasMaxLength(2).HasDefaultValue(0).HasComment("ÉêÇëÀà±ð");
            builder.Property(table => table.ItemId).HasColumnName("item_id").HasComment("ÉêÇëÄÚÈÝ");
            builder.Property(table => table.Remark).HasColumnName("remark").HasDefaultValue(string.Empty);
            builder.Property(table => table.UserId).HasColumnName("user_id").HasComment("ÉêÇëÈË");
            builder.Property(table => table.Status).HasColumnName("status").HasMaxLength(2).HasDefaultValue(0);
            builder.Property(table => table.UpdatedAt).HasColumnName("updated_at");
            builder.Property(table => table.CreatedAt).HasColumnName("created_at");
        }
    }
}
