using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NetDream.Modules.Note.Entities;
using NetDream.Modules.Note.Repositories;

namespace NetDream.Modules.Note.Migrations
{
    public class NoteEntityTypeConfiguration : IEntityTypeConfiguration<NoteEntity>
    {
        public void Configure(EntityTypeBuilder<NoteEntity> builder)
        {
            builder.ToTable("note", table => table.HasComment("��ǩ"));
            builder.HasKey(i => i.Id);
            builder.Property(table => table.Id).HasColumnName("id");
            builder.Property(table => table.Content).HasColumnName("content").HasComment("����");
            builder.Property(table => table.UserId).HasColumnName("user_id");
            builder.Property(table => table.IsNotice).HasColumnName("is_notice").HasDefaultValue(0).HasComment("�Ƿ�ʱվ�㹫��");
            builder.Property(table => table.Status).HasColumnName("status").HasMaxLength(1).HasDefaultValue(NoteRepository.STATUS_VISIBLE).HasComment("����״̬,");
            builder.Property(table => table.CreatedAt).HasColumnName("created_at");
        }
    }
}
