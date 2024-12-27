using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NetDream.Modules.CMS.Entities;

namespace NetDream.Modules.CMS.Migrations
{
    public class ModelEntityTypeConfiguration : IEntityTypeConfiguration<ModelEntity>
    {
        public void Configure(EntityTypeBuilder<ModelEntity> builder)
        {
            builder.ToTable("Model", table => table.HasComment(""));
            builder.HasKey(i => i.Id);
            builder.Property(table => table.Id).HasColumnName("id");
            builder.Property(table => table.Name).HasColumnName("name").HasMaxLength(100);
            builder.Property(table => table.Table).HasColumnName("table").HasMaxLength(100);
            builder.Property(table => table.Type).HasColumnName("type").HasMaxLength(2).HasDefaultValue(0);
            builder.Property(table => table.Position).HasColumnName("position").HasMaxLength(2).HasDefaultValue(99);
            builder.Property(table => table.ChildModel).HasColumnName("child_model").HasDefaultValue(0).HasComment("分集模型");
            builder.Property(table => table.CategoryTemplate).HasColumnName("category_template").HasMaxLength(20).HasDefaultValue(string.Empty);
            builder.Property(table => table.ListTemplate).HasColumnName("list_template").HasMaxLength(20).HasDefaultValue(string.Empty);
            builder.Property(table => table.ShowTemplate).HasColumnName("show_template").HasMaxLength(20).HasDefaultValue(string.Empty);
            builder.Property(table => table.EditTemplate).HasColumnName("edit_template").HasMaxLength(20).HasDefaultValue(string.Empty);
            builder.Property(table => table.Setting).HasColumnName("setting");
        }
    }
}
