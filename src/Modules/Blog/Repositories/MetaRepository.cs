using NetDream.Modules.Blog.Entities;
using NetDream.Shared.Repositories;
using NPoco;
using System.Collections.Generic;

namespace NetDream.Modules.Blog.Repositories
{
    public class MetaRepository(IDatabase db): MetaRepository<BlogMetaEntity>(db)
    {

        protected override string IdKey => "blog_id";

        protected override Dictionary<string, object> DefaultItems => new()
        {
            {"is_hide", 0 }, // 如果是转载文章是否只显示部分，并链接到原文
            { "source_url", string.Empty }, // 原文链接
            {"source_author", string.Empty }, // 原文作者
            { "cc_license", string.Empty }, // 版权协议
            { "weather", string.Empty }, // 天气
            { "audio_url", string.Empty }, // 音频
            { "video_url", string.Empty }, // 视频
            { "comment_status", 0 }, // 是否允许评论
            { "seo_link", string.Empty }, // 优雅链接
            { "seo_title", string.Empty }, // "seo 优化标题",
            { "seo_description", string.Empty }, // "seo 优化描述",
        };
    }
}
