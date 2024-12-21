using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NetDream.Modules.Forum.Entities;

namespace NetDream.Modules.Forum.Migrations
{
    public class ThreadEntityTypeConfiguration : IEntityTypeConfiguration<ThreadEntity>
    {
        public void Configure(EntityTypeBuilder<ThreadEntity> builder)
        {
            builder.ToTable("bbs_thread", table => table.HasComment(""));
            builder.HasKey("id");
            builder.Property(table => table.Id).HasColumnName("id");
            builder.Property(table => table.ForumId).HasColumnName("forum_id");
            builder.Property(table => table.ClassifyId).HasColumnName("classify_id").HasDefaultValue(0);
            builder.Property(table => table.Title).HasColumnName("title").HasMaxLength(200).HasComment("����");
            builder.Property(table => table.UserId).HasColumnName("user_id").HasComment("�����û�");
            builder.Property(table => table.ViewCount).HasColumnName("view_count").HasDefaultValue(0).HasComment("�鿴��");
            builder.Property(table => table.PostCount).HasColumnName("post_count").HasDefaultValue(0).HasComment("������");
            builder.Property(table => table.CollectCount).HasColumnName("collect_count").HasDefaultValue(0).HasComment("��ע��");
            builder.Property(table => table.IsHighlight).HasColumnName("is_highlight").HasDefaultValue(0)
                .HasComment("�Ƿ����");
            builder.Property(table => table.IsDigest).HasColumnName("is_digest").HasDefaultValue(0)
                .HasComment("�Ƿ񾫻�");
            builder.Property(table => table.IsClosed).HasColumnName("is_closed").HasDefaultValue(0)
                .HasComment("�Ƿ�ر�");
            builder.Property(table => table.TopType).HasColumnName("top_type").HasMaxLength(1).HasDefaultValue(0)
                .HasComment("�ö����ͣ�1 �����ö� 2 �����ö� 3 ȫ���ö�");
            builder.Property(table => table.IsPrivatePost).HasColumnName("is_private_post").HasDefaultValue(0).HasComment("�Ƿ��¥���ɼ�");
            builder.Property(table => table.UpdatedAt).HasColumnName("updated_at");
            builder.Property(table => table.CreatedAt).HasColumnName("created_at");
        }
    }
}
