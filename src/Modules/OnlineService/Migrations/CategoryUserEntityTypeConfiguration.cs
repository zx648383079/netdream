using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NetDream.Modules.OnlineService.Entities;

namespace NetDream.Modules.OnlineService.Migrations
{
    public class CategoryUserEntityTypeConfiguration : IEntityTypeConfiguration<CategoryUserEntity>
    {
        public void Configure(EntityTypeBuilder<CategoryUserEntity> builder)
        {
            builder.ToTable("service_category_user", table => table.HasComment(""));
            builder.HasKey(i => i.Id);
            builder.Property(table => table.Id).HasColumnName("id");
            builder.Property(table => table.CatId).HasColumnName("cat_id");
            builder.Property(table => table.UserId).HasColumnName("user_id");
            builder.Property(table => table.UpdatedAt).HasColumnName("updated_at");
            builder.Property(table => table.CreatedAt).HasColumnName("created_at");
        }
    }
}
