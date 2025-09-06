using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NetDream.Modules.Counter.Entities;

namespace NetDream.Modules.Counter.Migrations
{
    public class AnalysisFlagEntityTypeConfiguration : IEntityTypeConfiguration<AnalysisFlagEntity>
    {
        public void Configure(EntityTypeBuilder<AnalysisFlagEntity> builder)
        {
            builder.ToTable("ctr_analysis_flag", table => table.HasComment("分析数据用的标记"));
            builder.HasKey(i => i.Id);
            builder.Property(table => table.Id).HasColumnName("id");
            builder.Property(table => table.ItemType).HasColumnName("item_type").HasDefaultValue(0);
            builder.Property(table => table.ItemValue).HasColumnName("item_value").HasMaxLength(255);
            builder.Property(table => table.Flags).HasColumnName("flags");
            builder.Property(table => table.UserId).HasColumnName("user_id");
            builder.Property(table => table.CreatedAt).HasColumnName("created_at");
        }
    }
}
