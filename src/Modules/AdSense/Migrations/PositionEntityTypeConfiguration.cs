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
            builder.Property(table => table.Code).HasColumnName("code").HasMaxLength(20).HasComment("调用广告位的代码");
            builder.Property(table => table.AutoSize).HasColumnName("auto_size").HasDefaultValue(1).HasComment("自适应");
            builder.Property(table => table.SourceType).HasColumnName("source_type").HasDefaultValue(0).HasComment("广告来源");
            builder.Property(table => table.Width).HasColumnName("width").HasMaxLength(10).HasDefaultValue(string.Empty);
            builder.Property(table => table.Height).HasColumnName("height").HasMaxLength(10).HasDefaultValue(string.Empty);
            builder.Property(table => table.Template).HasColumnName("template").HasMaxLength(500).HasDefaultValue(string.Empty);
            builder.Property(table => table.Status).HasColumnName("status").HasDefaultValue(1).HasComment("是否启用广告位");
            builder.Property(table => table.UpdatedAt).HasColumnName("updated_at");
            builder.Property(table => table.CreatedAt).HasColumnName("created_at");

            builder.HasMany(p => p.Items)
                .WithOne(b => b.Position)
                .HasPrincipalKey(p => p.Id)
                .HasForeignKey(b => b.PositionId);

            
            var data = new PositionEntity[] {
                new("banner", "首页Banner"),
                new("mobile_banner", "手机端首页Banner"),
                new("home_notice", "首页通知栏"),
                new ("home_floor", "首页楼层"),
                new ("app_list", "APP列表页"),
                new("app_detail", "APP详情页"),
                new("blog_list", "Blog列表页"),
                new("blog_detail", "Blog详情页头部"),
                new("blog_inner", "Blog详情页内容中"),
                new("res_list", "资源列表页"),
                new("res_detail", "资源详情页"),
                new("cms_article", "CMS文章详情页"),
                new("bbs_list", "论坛列表页"),
                new("bbs_thread", "论坛帖子页"),
                new("book_chapter", "小说章节页"),
                new("book_detail", "小说详情页"),
            };
            foreach (var item in data)
            {
                item.Template = "{each}{.content}{/each}";
            }
            builder.HasData(data);
        }
    }
}
