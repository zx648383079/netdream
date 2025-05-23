using System.Collections.Generic;

namespace NetDream.Modules.ResourceStore.Repositories
{
    public class MetaRepository(ResourceContext db): Shared.Repositories.MetaRepository(db)
    {

        protected override Dictionary<string, string> DefaultItems => new()
        {
            { "preview_file", string.Empty }, // 预览文件地址
            { "file_catalog", string.Empty },  // 文件目录缓存
        };
    }
}
