using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NetDream.Modules.Navigation.Entities;

namespace NetDream.Modules.Navigation.Migrations
{
    public class KeywordEntityTypeConfiguration : IEntityTypeConfiguration<KeywordEntity>
    {
        public void Configure(EntityTypeBuilder<KeywordEntity> builder)
        {
            builder.ToTable("search_keyword", table => table.HasComment("�ؼ��ֱ�"));
            builder.HasKey(i => i.Id);
            builder.Property(table => table.Id).HasColumnName("id");
            builder.Property(table => table.Word).HasColumnName("word").HasMaxLength(30);
            builder.Property(table => table.Type).HasColumnName("type").HasMaxLength(1).HasDefaultValue(0).HasComment("�ؼ������ͣ�Ĭ�϶�β�ʣ���β��");
        }
    }
}
