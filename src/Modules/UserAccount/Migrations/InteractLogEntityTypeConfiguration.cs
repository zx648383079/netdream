using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NetDream.Modules.UserAccount.Entities;

namespace NetDream.Modules.UserAccount.Migrations
{
    public class InteractLogEntityTypeConfiguration : IEntityTypeConfiguration<InteractLogEntity>
    {
        public void Configure(EntityTypeBuilder<InteractLogEntity> builder)
        {
            builder.ToTable("user_interact_log", table => table.HasComment("用户交互记录"));
            builder.HasKey(i => i.Id);
            builder.Property(table => table.Id).HasColumnName("id");
            builder.Property(table => table.ItemType).HasColumnName("item_type").HasMaxLength(1).HasDefaultValue(0);
            builder.Property(table => table.ItemId).HasColumnName("item_id");
            builder.Property(table => table.UserId).HasColumnName("user_id");
            builder.Property(table => table.Action).HasColumnName("action");
            builder.Property(table => table.Data).HasColumnName("data");
            builder.Property(table => table.Ip).HasColumnName("ip");
            builder.Property(table => table.CreatedAt).HasColumnName("created_at");
        }
    }
}
