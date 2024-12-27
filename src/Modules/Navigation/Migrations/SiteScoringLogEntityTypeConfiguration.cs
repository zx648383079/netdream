using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NetDream.Modules.Navigation.Entities;

namespace NetDream.Modules.Navigation.Migrations
{
    public class SiteScoringLogEntityTypeConfiguration : IEntityTypeConfiguration<SiteScoringLogEntity>
    {
        public void Configure(EntityTypeBuilder<SiteScoringLogEntity> builder)
        {
            builder.ToTable("search_site_scoring_log", table => table.HasComment("վ�����ּ�¼��"));
            builder.HasKey(i => i.Id);
            builder.Property(table => table.Id).HasColumnName("id");
            builder.Property(table => table.SiteId).HasColumnName("site_id");
            builder.Property(table => table.UserId).HasColumnName("user_id");
            builder.Property(table => table.Score).HasColumnName("score").HasMaxLength(1).HasComment("վ������/�ٷ���");
            builder.Property(table => table.LastScore).HasColumnName("last_score").HasMaxLength(1).HasComment("�ϴε�����");
            builder.Property(table => table.ChangeReason).HasColumnName("change_reason").HasDefaultValue(string.Empty).HasComment("���ֱ䶯ԭ��");
            builder.Property(table => table.CreatedAt).HasColumnName("created_at");
        }
    }
}
