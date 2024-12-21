using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NetDream.Modules.Bot.Entities;

namespace NetDream.Modules.Bot.Migrations
{
    public class MediaEntityTypeConfiguration : IEntityTypeConfiguration<MediaEntity>
    {
        public void Configure(EntityTypeBuilder<MediaEntity> builder)
        {
            builder.ToTable("bot_media", table => table.HasComment(""));
            builder.HasKey("id");
            builder.Property(table => table.Id).HasColumnName("id");
            builder.Property(table => table.BotId).HasColumnName("bot_id").HasComment("所属微信公众号ID");
            builder.Property(table => table.Type).HasColumnName("type").HasMaxLength(10).HasComment("素材类型");
            builder.Property(table => table.MaterialType).HasColumnName("material_type").HasDefaultValue(MediaModel.MATERIAL_PERMANENT)
                .HasComment("素材类别:永久/临时");
            builder.Property(table => table.Title).HasColumnName("title").HasMaxLength(200).HasComment("素材标题");
            builder.Property(table => table.Thumb).HasColumnName("thumb").HasMaxLength(200).HasDefaultValue(string.Empty).HasComment("图文的封面");
            builder.Property(table => table.ShowCover).HasColumnName("show_cover").HasDefaultValue(0).HasComment("显示图文的封面");
            builder.Property(table => table.OpenComment).HasColumnName("open_comment").HasDefaultValue(0).HasComment("图文是否可以评论");
            builder.Property(table => table.OnlyComment).HasColumnName("only_comment").HasDefaultValue(0).HasComment("图文可以评论的人");
            builder.Property(table => table.Content).HasColumnName("content").HasComment("素材内容");
            builder.Property(table => table.ParentId).HasColumnName("parent_id").HasDefaultValue(0).HasComment("图文父id");
            builder.Property(table => table.MediaId).HasColumnName("media_id").HasMaxLength(100).HasDefaultValue(string.Empty).HasComment("素材ID");
            builder.Property(table => table.Url).HasColumnName("url").HasDefaultValue(string.Empty).HasComment("图片的url");
            builder.Property(table => table.PublishStatus).HasColumnName("publish_status").HasMaxLength(1).HasDefaultValue(0).HasComment("当前发布的状态");
            builder.Property(table => table.PublishId).HasColumnName("publish_id").HasMaxLength(50).HasDefaultValue(string.Empty).HasComment("当前发布的id, publish_status=6,为草稿=7,为发布中publish_id；为草稿=8,为发布成功，article_id");
            builder.Property(table => table.ExpiredAt).HasColumnName("expired_at").HasComment("临时素材的过期时间");
            builder.Property(table => table.UpdatedAt).HasColumnName("updated_at");
            builder.Property(table => table.CreatedAt).HasColumnName("created_at");
        }
    }
}
