using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NetDream.Modules.Bot.Entities;

namespace NetDream.Modules.Bot.Migrations
{
    public class EditorTemplateEntityTypeConfiguration : IEntityTypeConfiguration<EditorTemplateEntity>
    {
        public void Configure(EntityTypeBuilder<EditorTemplateEntity> builder)
        {
            builder.ToTable("bot_editor_template", table => table.HasComment("΢��ͼ��ģ��"));
            builder.HasKey("id");
            builder.Property(table => table.Id).HasColumnName("id");
            builder.Property(table => table.Type).HasColumnName("type").HasMaxLength(2).HasDefaultValue(0).HasComment("���ͣ��زġ����ա���ҵ");
            builder.Property(table => table.CatId).HasColumnName("cat_id").HasDefaultValue(0).HasComment("��ϸ����");
            builder.Property(table => table.Name).HasColumnName("name").HasMaxLength(100).HasComment("ģ�����");
            builder.Property(table => table.Content).HasColumnName("content").HasComment("ģ������");
            builder.Property(table => table.UpdatedAt).HasColumnName("updated_at");
            builder.Property(table => table.CreatedAt).HasColumnName("created_at");
        }
    }
}
