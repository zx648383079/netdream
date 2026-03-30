using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NetDream.Modules.Article.Entities;
using NetDream.Shared.Models;

namespace NetDream.Modules.Article.Migrations
{
    public class CategoryEntityTypeConfiguration : IEntityTypeConfiguration<CategoryEntity>
    {
        public void Configure(EntityTypeBuilder<CategoryEntity> builder)
        {
            builder.ToTable("category", table => table.HasComment("分类"));
            builder.HasKey(i => i.Id);
            builder.Property(table => table.Id).HasColumnName("id");
            builder.Property(table => table.Name).HasColumnName("name").HasMaxLength(40);
            builder.Property(table => table.EnName).HasColumnName("en_name").HasMaxLength(40);
            
            builder.Property(table => table.ParentId).HasColumnName("parent_id").HasDefaultValue(0);
            builder.Property(table => table.Thumb).HasColumnName("thumb").HasDefaultValue(string.Empty);
            builder.Property(table => table.Description).HasColumnName("description").HasDefaultValue(string.Empty);
            builder.Property(table => table.Type).HasColumnName("type").HasDefaultValue(ModuleTargetType.Article).HasComment("所属模块");
            builder.Property(table => table.ExtraData).HasColumnName("extra_data").HasDefaultValue(string.Empty).HasComment("独立引入样式");
            builder.Property(table => table.Status).HasColumnName("status").HasMaxLength(1)
              .HasDefaultValue(ReviewStatus.None).HasComment("审核状态");
            builder.Property(table => table.UpdatedAt).HasColumnName("updated_at");
            builder.Property(table => table.CreatedAt).HasColumnName("created_at");
        }
    }
}
