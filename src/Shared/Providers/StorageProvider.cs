using NetDream.Shared.Extensions;
using NetDream.Shared.Helpers;
using NetDream.Shared.Interfaces;
using NetDream.Shared.Interfaces.Database;
using NetDream.Shared.Migrations;
using NetDream.Shared.Providers.Models;
using NPoco;

namespace NetDream.Shared.Providers
{
    public class StorageProvider(IDatabase db, 
        IClientEnvironment client,
        IEnvironment server) : IMigrationProvider
    {
        public const string FILE_TABLE = "base_file";
        public const string FILE_LOG_TABLE = "base_file_log";
        public const string FILE_QUOTE_TABLE = "base_file_quote";

        private string _root = string.Empty;
        private int _tag;

        public StorageProvider Store(string root, int tag = 3)
        {
            _root = root;
            _tag = tag;
            return this;
        }

        public StorageProvider PublicStore()
        {
            return Store(server.AssetRoot, 1);
        }

        public StorageProvider PrivateStore()
        {
            return Store(Path.Combine(server.Root, "data/storage"), 2);
        }
        public void Migration(IMigration migration)
        {
            migration.Append(FILE_TABLE, table => {
                table.Id();
                table.String("name", 100);
                table.String("extension", 10);
                table.Char("md5", 32);//.Unique();
                table.String("path", 200);
                table.Uint("folder", 2).Default(1);
                table.String("size").Default("0");
                table.Timestamps();
            }).Append(FILE_LOG_TABLE, table => {
                table.Id();
                table.Uint("file_id");
                table.Uint("item_type", 1).Default(0);
                table.Uint("item_id");
                table.String("data").Default(string.Empty);
                table.Timestamp(MigrationTable.COLUMN_CREATED_AT);
            }).Append(FILE_QUOTE_TABLE, table => {
                table.Id();
                table.Uint("file_id");
                table.Uint("item_type", 1).Default(0);
                table.Uint("item_id");
                table.Uint("user_id").Default(0);
                table.Timestamp(MigrationTable.COLUMN_CREATED_AT);
            });
        }

        public FileOperationResult AddMd5(string md5)
        {
            return new FileOperationResult(GetByMd5(md5));
        }

        protected FileItem GetByMd5(string md5)
        {
            if (string.IsNullOrWhiteSpace(md5))
            {
                throw new Exception("not found");
            }
            var model = db.FindFirst<FileItem>(FILE_TABLE, "md5=@0", md5);
            if (model is null)
            {
                throw new Exception("not found");
            }
            return model;
        }

        /// <summary>
        /// 插入数据
        /// </summary>
        /// <param name="upload"></param>
        /// <returns></returns>
        public FileOperationResult AddFile(object upload)
        {
            var model = InsertFile(upload);
            return new FileOperationResult(model);
        }

        /**
         * 插入数据
         * @param BaseUpload|array upload
         * @param bool backFile 是否需要返回文件路径
         * @return array{id: int,name: string,extension: string,path:string,size: int,md5: string,folder: string, file: File} 返回 log 数据
         * @throws \Exception
         */
        public FileItem InsertFile(object upload, bool backFile = false)
        {
            //if (is_array(upload))
            //{
            //    upload = new UploadFile(upload);
            //}
            //file = upload.GetFile();
            //if (empty(file))
            //{
            //    file = Root->file(upload.GetRandomName());
            //    upload.SetFile(file);
            //}
            //path = file.GetRelative(Root);
            //if (empty(path) || !upload.Save())
            //{
            //    // 保存文件位置可能不在目录下
            //    throw new \Exception("add file error");
            //}
            //if (IsPublic() && FileRepository.IsDangerFile(file))
            //{
            //    file.Delete();
            //    throw new \Exception("error file content");
            //}
            //return InsertFileLog(file, [
            //    "name" => upload.GetName(),
            //    "type" => upload.GetType(),
            //], backFile);
            return null;
        }

        /**
         * 添加log 记录
         * @param File file
         * @param array rawData
         * @param bool backFile
         * @return array{id: int,name: string,extension: string,path:string,size: int,md5: string,folder: string, file: File} 返回的log数据
         * @throws \Exception
         */
        protected FileItem InsertFileLog(string file, object rawData, bool backFile = false)
        {
            //path = file.GetRelative(Root);
            //if (empty(path))
            //{
            //    // 保存文件位置可能不在目录下
            //    throw new \Exception("found file error");
            //}
            //md5 = file.Md5();
            //try
            //{
            //    model = GetByMd5(md5);
            //    distFile = ToFile(model["path"]);
            //    if (!distFile.Exist())
            //    {
            //        file.Move(distFile);
            //    }
            //    else
            //    {
            //        file.Delete();
            //    }
            //    if (backFile)
            //    {
            //        model["file"] = distFile;
            //    }
            //    return model;
            //}
            //catch (Exception ex) {
            //}
            //model = [
            //    "name" => rawData["name"] ?? file.GetName(),
            //    "extension" => rawData["type"] ?? file.GetExtension(),
            //    "path" => ltrim(path, "/"),
            //    "size" => file.Size(),
            //    "md5" => md5,
            //    "folder" => Tag,
            //    MigrationTable.COLUMN_CREATED_AT => time(),
            //    MigrationTable.COLUMN_UPDATED_AT => time(),
            //];
            //model["id"] = Query().Insert(model);
            //if (empty(model["id"]))
            //{
            //    file.Delete();
            //    throw new \Exception("add file log error");
            //}
            //if (backFile)
            //{
            //    model["file"] = file;
            //}
            //return model;

            return null;
        }

        /**
            * 复制文件到这里
            * @param File sourceFile
            * @param array rawData
            * @param bool backFile
            * @return array{id: int,name: string,extension: string,path:string,size: int,md5: string,folder: string, file: File} 返回 log 数据
            * @throws \Exception
            */
        public FileItem CopyFile(string sourceFile, object rawData, bool backFile = false)
        {
            //if (sourceFile.StartsWith(root))
            //{
            //    file = sourceFile;
            //}
            //else
            //{
            //    file = Root->file(sprintf("%s_%s", time(), sourceFile.GetName()));
            //}
            //return InsertFileLog(file, rawData, backFile);
            return null;
        }

        public Page<FileItem> Search(string keywords, string[] extension, int page = 1)
        {
            var sql = new Sql();
            sql.Select("*").From(FILE_TABLE)
                .Where("Folder=@0", _tag);
            SearchHelper.Where(sql, "name", keywords);
            if (extension.Length > 0)
            {
                sql.WhereIn("extension", extension);
            }
            sql.OrderBy("id DESC");
            return db.Page<FileItem>(page, 20, sql);
        }

        public FileItem Get(string url)
        {
            var model = db.FindFirst<FileItem>(FILE_TABLE, "folder=@0 AND path=@1",
                _tag, url);
            if (model is null)
            {
                throw new Exception("not found file");
            }
            return model;
        }

        public void AddQuote(string url, byte itemType, int itemId)
        {
            if (string.IsNullOrWhiteSpace(url))
            {
                return;
            }
            var id = db.FindScalar<int>(FILE_TABLE, "id", "folder=@0 AND path=@1",
                _tag, url);
            if (id < 1)
            {
                throw new Exception("not found file");
            }
            var count = db.FindCount(FILE_QUOTE_TABLE, "file_id=@0 AND item_id=@1 AND item_type=@2",
                id, itemId, itemType);
            if (count > 0)
            {
                return;
            }
            db.Insert(FILE_QUOTE_TABLE, new FileQuoteLog()
            {
                FileId = id,
                ItemId = itemId,
                ItemType = itemType,
                UserId = client.UserId,
                CreatedAt = client.Now,
            });
        }

        public void RemoveQuote(byte itemType, int itemId)
        {
            db.DeleteWhere(FILE_QUOTE_TABLE, "item_type=@0 AND item_id=@1", itemType, itemId);
        }

        public void RemoveFile(string url)
        {
            if (string.IsNullOrWhiteSpace(url))
            {
                return;
            }
            Remove(Get(url));
        }

        public void Remove(FileItem model)
        {
            db.DeleteWhere(FILE_QUOTE_TABLE, "file_id=@0", model.Id);
            db.DeleteWhere(FILE_LOG_TABLE, "file_id=@0", model.Id);
            db.DeleteWhere(FILE_TABLE, "id=@0", model.Id);
            File.Delete(ToFile(model));
        }

        /// <summary>
        /// 返回本地文件
        /// </summary>
        /// <param name="url"></param>
        /// <param name="useOriginalName">使用原文件名</param>
        /// <returns></returns>
        public string GetFile(string url, bool useOriginalName = true)
        {
            var model = Get(url);
            var file = ToFile(model.Path);
            if (!useOriginalName)
            {
                return file;
            }
            return file;// .SetName(model["name"]);
        }

        /// <summary>
        /// 确认一下文件
        /// </summary>
        public void Reload()
        {
            //folder = IsPublic() ? Root->directory("upload") : Root;
            //folder.MapDeep((FileObject file) {
            //    if (file instanceof Directory) {
            //        return;
            //    }
            //    md5 = file.Md5();
            //    if (static.Query().Where("md5", md5).Count() > 0) {
            //        return;
            //    }
            //    path = file.GetRelative(Root);
            //    model = static.Query().Where("path", path).Where("folder", Tag).First();
            //    if (empty(model))
            //    {
            //        Query().Insert([
            //            "name" => file.GetName(),
            //    "extension" => file.GetExtension(),
            //    "path" => path,
            //    "size" => file.Size(),
            //    "md5" => md5,
            //    "folder" => Tag,
            //    MigrationTable.COLUMN_CREATED_AT => time(),
            //    MigrationTable.COLUMN_UPDATED_AT => time(),
            //]);
            //        return;
            //    }
            //    if (model["md5"] !== md5)
            //    {
            //        static.Query().Where("id", model["id"])
            //            .Update([
            //                "size" => file.Size(),
            //        "md5" => md5
            //            ]);
            //    }
            //});
        }

        public void SyncFile(string path)
        {
            var file = ToFile(path);
            if (!File.Exists(file))
            {
                throw new Exception(string.Format("File is error: {0}", path));
            }
            db.UpdateWhere(FILE_TABLE, "path=@0 AND folder=@1", new Dictionary<string, object>()
            {
                {"size", 0 }, // file.Size()
                {"md5", "" } // file.Md5()
            }, path, _tag);
        }

        protected string ToFile(FileItem path)
        {
            return ToFile(path.Path);
        }
        protected string ToFile(string path) 
        {
            if (string.IsNullOrWhiteSpace(path))
            {
                throw new Exception("path is empty");
            }
            return Path.Combine(_root, path);
        }

        /// <summary>
        /// 输出文件
        /// </summary>
        /// <param name="output"></param>
        /// <param name="url"></param>
        /// <param name="notSplit">是否不启用分块下载，对于流媒体播放不要启用</param>
        /// <returns></returns>
        //public Output Output(Output output, string url, bool notSplit = false)
        //{
        //    file = GetFile(url);
        //    if (notSplit)
        //    {
        //        output.File(file, 0);
        //    }
        //    else
        //    {
        //        output.File(file);
        //    }
        //    return output;
        //}

        /// 判断当前根目录是否是对外开放的
        public bool IsPublic()
        {
            return false;//str_starts_with((string)Root, (string)public_path());
        }

        /// <summary>
        /// 从路径转成访问网址
        /// </summary>
        /// <param name="path"></param>
        /// <returns>不在公共目录返回 string.Empty</returns>
        public string ToPublicUrl(string path)
        {
            //root = (string)public_path();
            //file = (string)Root->file(path);
            //if (str_starts_with(file, root))
            //{
            //    return url().Asset(substr(file, strlen(root)));
            //}
            return string.Empty;
        }

        /// <summary>
        /// 从网址转成路径
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public string FromPublicUrl(string path)
        {
            var assetName = string.Empty; //config("view.asset_directory", string.Empty);
            if (!string.IsNullOrWhiteSpace(assetName))
            {
                if (!assetName.EndsWith('/'))
                {
                    assetName += "/";
                }
                var i = path.IndexOf(assetName);
                if (i >= 0)
                {
                    return path[(i + assetName.Length)..];
                }
            }
            if (path.Contains("://"))
            {
                path = new Uri(path).AbsolutePath;
            }
            var file = Path.GetFullPath(path);
            if (file.StartsWith(_root))
            {
                return file[_root.Length..].TrimStart('/');
            }
            return path;
        }
    }
}
