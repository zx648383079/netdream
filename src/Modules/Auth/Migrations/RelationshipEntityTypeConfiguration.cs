using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NetDream.Modules.Auth.Entities;

namespace NetDream.Modules.Auth.Migrations
{
    public class RelationshipEntityTypeConfiguration : IEntityTypeConfiguration<RelationshipEntity>
    {
        public void Configure(EntityTypeBuilder<RelationshipEntity> builder)
        {
            builder.ToTable("user_relationship", table => table.HasComment("�û���ϵ��"));
            builder.Property(table => table.UserId).HasColumnName("user_id");
            builder.Property(table => table.LinkId).HasColumnName("link_id").HasComment("����ϵ����");
            builder.Property(table => table.Type).HasColumnName("type").HasMaxLength(1).HasDefaultValue(0).HasComment("�����ϵ");
            builder.Property(table => table.CreatedAt).HasColumnName("created_at");
        }
    }
}
