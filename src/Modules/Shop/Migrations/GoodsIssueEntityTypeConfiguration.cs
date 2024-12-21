using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NetDream.Modules.Shop.Entities;

namespace NetDream.Modules.Shop.Migrations
{
    public class GoodsIssueEntityTypeConfiguration : IEntityTypeConfiguration<GoodsIssueEntity>
    {
        public void Configure(EntityTypeBuilder<GoodsIssueEntity> builder)
        {
            builder.ToTable("GoodsIssue", table => table.HasComment(""));
            builder.HasKey("id");
            builder.Property(table => table.Id).HasColumnName("id");
            builder.Property(table => table.GoodsId).HasColumnName("goods_id");
            builder.Property(table => table.Question).HasColumnName("question");
            builder.Property(table => table.Answer).HasColumnName("answer").HasDefaultValue(string.Empty);
            builder.Property(table => table.AskId).HasColumnName("ask_id");
            builder.Property(table => table.AnswerId).HasColumnName("answer_id").HasDefaultValue(0);
            builder.Property(table => table.Status).HasColumnName("status").HasMaxLength(1).HasDefaultValue(0).HasComment("����״̬����������ѹرգ���ɾ�������ö�");
            builder.Property(table => table.UpdatedAt).HasColumnName("updated_at");
            builder.Property(table => table.CreatedAt).HasColumnName("created_at");
        }
    }
}
