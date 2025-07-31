using NetDream.Shared.Interfaces;
using NetDream.Shared.Models;
using NetDream.Shared.Repositories;
using NetDream.Shared.Repositories.Models;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace NetDream.Modules.SEO.Repositories
{
    public class SEORepository(SEOContext db, 
        ExplorerRepository explorer)
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
            explorer.RemoveBakFiles("sql_");
        }

        public IOperationResult BackUpSql(bool isZip = true)
        {
            var root = explorer.BakPath();
            if (Directory.Exists(root)) 
            {
                Directory.CreateDirectory(root);
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

        public IList<FileItem> SqlFiles()
        {
            return explorer.BakFiles("sql_");
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
