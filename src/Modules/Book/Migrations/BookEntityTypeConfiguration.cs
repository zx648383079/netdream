using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NetDream.Modules.Book.Entities;

namespace NetDream.Modules.Book.Migrations
{
    public class BookEntityTypeConfiguration : IEntityTypeConfiguration<BookEntity>
    {
        public void Configure(EntityTypeBuilder<BookEntity> builder)
        {
            builder.ToTable("book", table => table.HasComment("С˵"));
            builder.HasKey(table => table.Id);
            builder.Property(table => table.Id).HasColumnName("id");
            builder.HasIndex(table => table.Name).IsUnique();
            builder.Property(table => table.Name).HasColumnName("name").HasMaxLength(100).HasComment("����");
            builder.Property(table => table.Cover).HasColumnName("cover").HasMaxLength(200).HasDefaultValue(string.Empty).HasComment("����");
            builder.Property(table => table.Description).HasColumnName("description").HasMaxLength(200).HasDefaultValue(string.Empty).HasComment("���");
            builder.Property(table => table.AuthorId).HasColumnName("author_id").HasDefaultValue(0).HasComment("����");
            builder.Property(table => table.UserId).HasColumnName("user_id").HasDefaultValue(0);
            builder.Property(table => table.Classify).HasColumnName("classify").HasMaxLength(2).HasDefaultValue(0).HasComment("С˵�ּ�");
            builder.Property(table => table.CatId).HasColumnName("cat_id").HasDefaultValue(0).HasComment("����");
            builder.Property(table => table.Size).HasColumnName("size").HasDefaultValue(0).HasComment("������");
            builder.Property(table => table.ClickCount).HasColumnName("click_count").HasDefaultValue(0).HasComment("�����");
            builder.Property(table => table.RecommendCount).HasColumnName("recommend_count")
                .HasDefaultValue(0).HasComment("�Ƽ���");
            builder.Property(table => table.OverAt).HasColumnName("over_at").HasComment("�걾����");
            builder.Property(table => table.Status).HasColumnName("status").HasMaxLength(2).HasDefaultValue(0);
            builder.Property(table => table.SourceType).HasColumnName("source_type").HasMaxLength(2).HasDefaultValue(0).HasComment("��Դ���ͣ�ԭ����ת��");
            builder.Property(table => table.DeletedAt).HasColumnName("deleted_at");
            builder.Property(table => table.UpdatedAt).HasColumnName("updated_at");
            builder.Property(table => table.CreatedAt).HasColumnName("created_at");

            builder.HasMany(p => p.Chapters)
                  .WithOne(b => b.Book)
                  .HasPrincipalKey(p => p.Id)
                  .HasForeignKey(b => b.BookId);

            builder.HasMany(p => p.Sources)
                  .WithOne(b => b.Book)
                  .HasPrincipalKey(p => p.Id)
                  .HasForeignKey(b => b.BookId);
        }
    }
}
