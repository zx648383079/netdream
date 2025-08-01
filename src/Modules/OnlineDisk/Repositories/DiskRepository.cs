using NetDream.Modules.OnlineDisk.Adapters;
using NetDream.Modules.OnlineDisk.Models;
using NetDream.Shared.Interfaces;
using NetDream.Shared.Models;
using System;
using System.Linq;

namespace NetDream.Modules.OnlineDisk.Repositories
{
    public class DiskRepository(OnlineDiskContext db, IClientContext client)
    {
        public const string TYPE_IMAGE = "image";
        public const string TYPE_DOCUMENT = "doc";
        public const string TYPE_VIDEO = "video";
        public const string TYPE_BT = "bt";
        public const string TYPE_MUSIC = "music";
        public const string TYPE_ZIP = "archive";
        public const string TYPE_APP = "app";
        public const string TYPE_UNKNOWN = "unknown";
        /// <summary>
        /// 是否启用分布式存储
        /// </summary>
        public bool UseDistributed => false;

        public IDiskAdapter Driver { get; private set; } = new DatabaseAdapter(db, client);

        /// <summary>
        /// 对网址进行许可
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public IOperationResult<string> AllowUrl(string url)
        {
            return OperationResult.Ok(url);
        }

        public IOperationResult<DiskListItem> File(string id)
        {
            var res = Driver.File(id);
            if (!res.Succeeded)
            {
                return res;
            }
            return res;
        }


        public string[] TypeToExtension(string type)
        {
            return [];
        }


        internal static void IncludeFile(OnlineDiskContext db, IWithFileModel[] items)
        {
            var idItems = items.Select(item => item.FileId).Where(i => i > 0)
                .Distinct().ToArray();
            if (idItems.Length == 0)
            {
                return;
            }
            var data = db.Files.Where(i => idItems.Contains(i.Id))
                .Select(i => new FileLabelItem()
                {
                    Id = i.Id,
                    Size = i.Size,
                    Thumb = i.Thumb,
                })
                .ToDictionary(i => i.Id);
            if (data.Count == 0)
            {
                return;
            }
            foreach (var item in items)
            {
                if (item.FileId > 0 && data.TryGetValue(item.FileId, out var res))
                {
                    item.File = res;
                }
            }
        }
    }
}
