using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NetDream.Modules.Navigation.Entities;

namespace NetDream.Modules.Navigation.Migrations
{
    public class PageKeywordEntityTypeConfiguration : IEntityTypeConfiguration<PageKeywordEntity>
    {
        public void Configure(EntityTypeBuilder<PageKeywordEntity> builder)
        {
            builder.ToTable("search_page_keyword", table => table.HasComment("��ҳ�����ؼ��ֱ�"));
            builder.Property(table => table.PageId).HasColumnName("page_id");
            builder.Property(table => table.WordId).HasColumnName("word_id");
            builder.Property(table => table.IsOfficial).HasColumnName("is_official").HasDefaultValue(0).HasComment("�Ƿ�Ϊ�ؼ��ʹ���");
        }
    }
}
