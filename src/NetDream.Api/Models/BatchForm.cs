using NetDream.Modules.Auth.Models;
using NetDream.Modules.Blog.Models;
using System.Collections.Generic;

namespace NetDream.Api.Models
{
    public class BatchForm
    {
        public object? AuthProfile { get; set; }
        public object? SeoConfigs { get; set; }
        public object? BlogCategories { get; set; }
    }

    public class BatchResult
    {
        public UserProfile? AuthProfile { get; set; }
        public IDictionary<string, object>? SeoConfigs { get; set; }
        public CategoryModel[]? BlogCategories { get; set; }
    }
}
