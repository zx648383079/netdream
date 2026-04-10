using NetDream.Modules.Storage.Models;
using NetDream.Shared.Helpers;
using NetDream.Shared.Interfaces;
using NetDream.Shared.Models;
using NetDream.Shared.Repositories;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace NetDream.Modules.SEO.Repositories
{
    public class SEORepository(SEOContext db,
        ISystemStorage storage)
    {

        public IList<OptionItem<string>> StoreItems()
        {
            return [new("默认", "default"), 
                new("用户", "auth"), 
                new("页面", "pages"), 
                new("部件", "nodes"),];
        }

        public void ClearSql()
        {
            var folder = storage.Backup;
            foreach (var item in folder.Files())
            {
                if (Path.GetFileName(item).StartsWith("sql_"))
                {
                    folder.Delete(item);
                }
            }
        }

        public IOperationResult BackUpSql(bool isZip = true)
        {
            var folder = storage.Backup;
            if (!folder.Exist())
            {
                folder.Create();
            }
            // TODO
            return OperationResult.Fail("未实现");
            //var fileName = sprintf("sql_%s.sql", date("Y-m-d"));
            //targetFileName = isZip ? sprintf("sql_%s.zip", date("Y-m-d")) : fileName;
            //targetFile = root.File(targetFileName);
            //set_time_limit(0);
            //if (targetFile.Exist() && targetFile.ModifyTime() > (time() - 60))
            //{
            //    return;
            //}
            //file = isZip ? root.File(fileName) : targetFile;
            //if (!GenerateModel.Schema()
            //    .Export(file, [], false))
            //{
            //    throw new Exception("导出失败！");
            //}
            //if (!isZip)
            //{
            //    return;
            //}
            //ZipStream.Create(targetFile).AddFile(file)
            //    .Close();
            //file.Delete();
            return OperationResult.Ok();
        }

        public IFileListItem[] SqlFiles()
        {
            var folder = storage.Backup;
            if (!folder.Exist())
            {
                return [];
            }
            var items = new List<IFileListItem>();
            foreach (var item in folder.Files())
            {
                var info = folder.File(item);
                if (info is not null && info.Name.StartsWith("sql_"))
                {
                    items.Add(new FileListItem(info.Name)
                    {
                        Size = info.Length,
                        CreatedAt = TimeHelper.TimestampFrom(info.CreationTime)
                    });
                }
            }
            return [];
        }

        public void ClearCache(string[] store)
        {
            FlushCache(store.Length == 0 ? StoreItems().Select(i => i.Value).ToArray() : 
                store);
        }

        public void ClearExcludeCache(IList<string> exclude)
        {
            FlushCache(StoreItems().Select(i => i.Value).Where(i => !exclude.Contains(i)).ToArray());
        }

        protected void FlushCache(string[] storeItems)
        {
            if (storeItems.Length == 0)
            {
                return;
            }
            foreach (var item in storeItems)
            {
                if (string.IsNullOrWhiteSpace(item))
                {
                    continue;
                }
                if (item == "default")
                {
                    //cache().Flush();
                    continue;
                }
                // cache().Store(item).Flush();
            }
        }

        
    }
}
