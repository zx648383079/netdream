using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NetDream.Modules.AdSense.Entities;

namespace NetDream.Modules.AdSense.Migrations
{
    public class PositionEntityTypeConfiguration : IEntityTypeConfiguration<PositionEntity>
    {
        public void Configure(EntityTypeBuilder<PositionEntity> builder)
        {
            builder.ToTable("ad_position");
            builder.HasKey(i => i.Id);
            builder.HasIndex(table => table.Code).IsUnique();
            builder.Property(table => table.Id).HasColumnName("id");
            builder.Property(table => table.Name).HasColumnName("name").HasMaxLength(30);
            builder.Property(table => table.Code).HasColumnName("code").HasMaxLength(20).HasComment("���ù��λ�Ĵ���");
            builder.Property(table => table.AutoSize).HasColumnName("auto_size").HasDefaultValue(1).HasComment("����Ӧ");
            builder.Property(table => table.SourceType).HasColumnName("source_type").HasDefaultValue(0).HasComment("�����Դ");
            builder.Property(table => table.Width).HasColumnName("width").HasMaxLength(10).HasDefaultValue(string.Empty);
            builder.Property(table => table.Height).HasColumnName("height").HasMaxLength(10).HasDefaultValue(string.Empty);
            builder.Property(table => table.Template).HasColumnName("template").HasMaxLength(500).HasDefaultValue(string.Empty);
            builder.Property(table => table.Status).HasColumnName("status").HasDefaultValue(1).HasComment("�Ƿ����ù��λ");
            builder.Property(table => table.UpdatedAt).HasColumnName("updated_at");
            builder.Property(table => table.CreatedAt).HasColumnName("created_at");

            builder.HasMany(p => p.Items)
                .WithOne(b => b.Position)
                .HasPrincipalKey(p => p.Id)
                .HasForeignKey(b => b.PositionId);

            
            var data = new PositionEntity[] {
                new("banner", "��ҳBanner"),
                new("mobile_banner", "�ֻ�����ҳBanner"),
                new("home_notice", "��ҳ֪ͨ��"),
                new ("home_floor", "��ҳ¥��"),
                new ("app_list", "APP�б�ҳ"),
                new("app_detail", "APP����ҳ"),
                new("blog_list", "Blog�б�ҳ"),
                new("blog_detail", "Blog����ҳͷ��"),
                new("blog_inner", "Blog����ҳ������"),
                new("res_list", "��Դ�б�ҳ"),
                new("res_detail", "��Դ����ҳ"),
                new("cms_article", "CMS��������ҳ"),
                new("bbs_list", "��̳�б�ҳ"),
                new("bbs_thread", "��̳����ҳ"),
                new("book_chapter", "С˵�½�ҳ"),
                new("book_detail", "С˵����ҳ"),
            };
            foreach (var item in data)
            {
                item.Template = "{each}{.content}{/each}";
            }
            builder.HasData(data);
        }
    }
}
