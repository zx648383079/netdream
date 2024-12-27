using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NetDream.Modules.Forum.Entities;

namespace NetDream.Modules.Forum.Migrations
{
    public class ThreadPostEntityTypeConfiguration : IEntityTypeConfiguration<ThreadPostEntity>
    {
        public void Configure(EntityTypeBuilder<ThreadPostEntity> builder)
        {
            builder.ToTable("bbs_thread_post", table => table.HasComment(""));
            builder.HasKey(i => i.Id);
            builder.Property(table => table.Id).HasColumnName("id");
            builder.Property(table => table.Content).HasColumnName("content");
            builder.Property(table => table.ThreadId).HasColumnName("thread_id");
            builder.Property(table => table.UserId).HasColumnName("user_id").HasComment("�û�");
            builder.Property(table => table.Ip).HasColumnName("ip").HasMaxLength(120);
            builder.Property(table => table.Grade).HasColumnName("grade").HasMaxLength(5)
                .HasDefaultValue(0).HasComment("�ظ��Ĳ㼶");
            builder.Property(table => table.IsInvisible).HasColumnName("is_invisible").HasDefaultValue(0)
                .HasComment("�Ƿ�ͨ�����");
            builder.Property(table => table.AgreeCount).HasColumnName("agree_count").HasDefaultValue(0)
                .HasComment("�޳���");
            builder.Property(table => table.DisagreeCount).HasColumnName("disagree_count").HasDefaultValue(0)
                .HasComment("���޳���");
            builder.Property(table => table.Status).HasColumnName("status").HasMaxLength(1).HasDefaultValue(0)
                .HasComment("���ӵ�״̬");
            builder.Property(table => table.UpdatedAt).HasColumnName("updated_at");
            builder.Property(table => table.CreatedAt).HasColumnName("created_at");
        }
    }
}
