using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NetDream.Modules.TradeTracker.Entities;

namespace NetDream.Modules.TradeTracker.Migrations
{
    public class ProductEntityTypeConfiguration : IEntityTypeConfiguration<ProductEntity>
    {
        public void Configure(EntityTypeBuilder<ProductEntity> builder)
        {
            builder.ToTable("tt_products", table => table.HasComment("商品货品表"));
            builder.HasKey(table => table.Id);
            builder.Property(table => table.Id).HasColumnName("id");
            builder.Property(table => table.ParentId).HasColumnName("parent_id").HasDefaultValue(0);
            builder.Property(table => table.Name).HasColumnName("name").HasMaxLength(40);
            builder.Property(table => table.EnName).HasColumnName("en_name").HasMaxLength(40);
            builder.Property(table => table.CatId).HasColumnName("cat_id").HasDefaultValue(0);
            builder.Property(table => table.ProjectId).HasColumnName("project_id").HasDefaultValue(0);
            builder.Property(table => table.UniqueCode).HasColumnName("unique_code").HasMaxLength(100).HasDefaultValue(string.Empty);
            builder.Property(table => table.IsSku).HasColumnName("is_sku").HasDefaultValue(1);
            builder.Property(table => table.UpdatedAt).HasColumnName("updated_at");
            builder.Property(table => table.CreatedAt).HasColumnName("created_at");
        }
    }
}
