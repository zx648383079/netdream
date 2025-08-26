using NetDream.Shared.Helpers;
using NetDream.Shared.Interfaces;
using NetDream.Shared.Models;
using NetDream.Shared.Providers;
using NetDream.Shared.Providers.Entities;
using NetDream.Shared.Providers.Forms;
using NetDream.Shared.Repositories.Forms;
using NetDream.Shared.Repositories.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace NetDream.Shared.Repositories
{
    public class ExplorerRepository(StorageProvider storage, IEnvironment environment)
    {
        /// <summary>
        /// 备份的文件夹
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public string BakPath(string fileName = "")
        {
            var root = "data/bak";
            if (string.IsNullOrWhiteSpace(fileName))
            {
                return Path.Combine(environment.Root, root);
            }
            return Path.Combine(environment.Root, root, fileName);
        }

        /// <summary>
        /// 获取备份的文件
        /// </summary>
        /// <param name="prefix"></param>
        /// <returns></returns>
        public IList<FileItem> BakFiles(string prefix)
        {
            var root = new DirectoryInfo(BakPath());
            if (!root.Exists)
            {
                return [];
            }
            var items = new List<FileItem>();
            foreach (var item in root.EnumerateFiles())
            {
                if (item.Name.StartsWith(prefix))
                {
                    items.Add(new(item.Name)
                    {
                        Size = item.Length,
                        CreatedAt = TimeHelper.TimestampFrom(item.CreationTime)
                    });
                }
            }
            return items;
        }

        /// <summary>
        /// 删除一些备份文件
        /// </summary>
        /// <param name="prefix"></param>
        public void RemoveBakFiles(string prefix)
        {
            var root = new DirectoryInfo(BakPath());
            if (!root.Exists)
            {
                return;
            }
            foreach (var item in root.EnumerateFiles())
            {
                if (item.Name.StartsWith(prefix))
                {
                    item.Delete();
                }
            }
        }

        protected StorageProvider? Storage(string tag)
        {
            return Storage(tag.ToLower()[0] - 96);
        }
        protected StorageProvider? Storage(int tag)
        {
            return tag switch {
                1 => storage.PublicStore(),
                2 => storage.PrivateStore(),
                _ => null,
            };
        }

        public IDictionary<string, string> DriveItems()
        {
            var items = new Dictionary<string, string>()
            {
                {"a", Path.Combine(environment.PublicRoot, environment.AssetRoot) },
                {"a", Path.Combine(environment.Root, "data/storage") },
            };
            if (!string.IsNullOrWhiteSpace(environment.OnlineDiskRoot))
            {
                items.Add("c", Path.Combine(environment.Root, environment.OnlineDiskRoot));
            }
            return items;
        }

        public IList<VirtualFileItem> DriveList()
        {
            return DriveItems().Select(item => {
                return new VirtualFileItem($"DATA({item.Key}:)", $"{item.Key}:", true);
            }).ToArray();
        }

        public IPage<VirtualFileItem> Search(ExplorerQueryForm form)
        {
            var (drive, p, folder) = SplitPath(form.Path);
            var path = p;
            if (string.IsNullOrWhiteSpace(drive))
            {
                return new Page<VirtualFileItem>(DriveList());
            }
            if (string.IsNullOrWhiteSpace(folder))
            {
                return drive switch {
                   "image" or "video" or "document" => SearchWithType(new ExplorerQueryForm(form)
                   {
                       Type = drive
                   }),
                    _ => new Page<VirtualFileItem>(),
                };
            }
            if (!Directory.Exists(folder))
            {
                return new Page<VirtualFileItem>();
            }
            var visualFolder = string.Format("{0}:{1}{2}", drive, !string.IsNullOrWhiteSpace(path) ? "/" : string.Empty, path);
            var items = new List<VirtualFileItem>();
            var info = new DirectoryInfo(folder);
            foreach (var item in info.EnumerateDirectories())
            {
                if (!IsMatch(form.Keywords, item.Name))
                {
                    continue;
                }
                if (!IsFilterFile(form.Filter, true, string.Empty))
                {
                    continue;
                }
                items.Add(new(item.Name, $"{visualFolder}/{item.Name}")
                {
                    IsFolder = true,
                    CreatedAt = TimeHelper.TimestampFrom(item.LastWriteTime)
                });
            }
            foreach (var item in info.EnumerateFiles())
            {
                if (!IsMatch(form.Keywords, item.Name))
                {
                    continue;
                }
                if (!IsFilterFile(form.Filter, false, item.Extension))
                {
                    continue;
                }
                items.Add(new(item.Name, $"{visualFolder}/{item.Name}")
                {
                    Size = item.Length,
                    CreatedAt = TimeHelper.TimestampFrom(item.LastWriteTime)
                });
            }
            return new Page<VirtualFileItem>(items);
        }

        public string Download(string path)
        {
            return ToFileName(path);
            //fileName = static.ToFileName(path);
            //if (empty(fileName) || !is_file(fileName))
            //{
            //    output.Header->setContentDisposition("error.txt");
            //    return output.Custom(sprintf("Path [%s] is error", path), "txt");
            //}
            //return output.File(new File(fileName));
        }

        public void Delete(string path)
        {
            var (drive, p, fileName) = SplitPath(path);
            path = p;
            if (string.IsNullOrWhiteSpace(fileName))
            {
                throw new Exception(string.Format("Path [{0}] is error", path));
            }
            if (Directory.Exists(fileName))
            {
                Directory.Delete(fileName);
                return;
            }
            if (!File.Exists(fileName))
            {
                return;
            }
            File.Delete(fileName);
            var storage = Storage(drive);
            if (storage is null)
            {
                return;
            }
            storage.RemoveFile(path);
        }

        /**
         * 获取真实地址
         * @param string path
         * @return string
         */
        public string ToFileName(string path)
        {
            var (_, _, fileName) = SplitPath(path);
            return fileName;
        }

        /**
         * 解析地址
         * @param string path
         * @return string[] // [drive: string, path: string, fileName: string]
         */
        public (string, string, string) SplitPath(string path)
        {
            if (string.IsNullOrWhiteSpace(path) || path == "/")
            {
                return (string.Empty, string.Empty, string.Empty);
            }
            var args = path.Split(':');
            path = args.Last();
            if (!string.IsNullOrWhiteSpace(path))
            {
                path = FileHelper.FilterPath(path);
            }
            var drive = "a";
            if (args.Length > 1)
            {
                drive = args[0].ToLower();
            }
            var maps = DriveItems();
            if (!maps.TryGetValue(drive, out var fileName))
            {
                return (drive, string.Empty, string.Empty);
            }
            return (drive, path, string.IsNullOrWhiteSpace(path) ? fileName : Path.Combine(fileName, path));
        }

        protected bool IsMatch(string keywords, string fileName)
        {
            if (string.IsNullOrWhiteSpace(keywords))
            {
                return true;
            }
            foreach (var item in keywords.Split(' '))
            {
                if (string.IsNullOrWhiteSpace(item))
                {
                    continue;
                }
                if (fileName.Contains(item))
                {
                    return true;
                }
            }
            return false;
        }

        protected bool IsFilterFile(string filter, bool isFolder, string ext)
        {
            if (string.IsNullOrWhiteSpace(filter))
            {
                return true;
            }
            if (filter == "folder")
            {
                return isFolder;
            }
            if (isFolder)
            {
                return false;
            }
            if (string.IsNullOrWhiteSpace(ext))
            {
                return false;
            }
            var i = filter.IndexOf('/');
            if (i >= 0)
            {
                filter = filter[0.. i];
            }
            return FileRepository.IsTypeExtension(ext, filter);
        }

        public IPage<VirtualFileItem> SearchWithType(ExplorerQueryForm form)
        {
            var provider = storage.PublicStore();
            var items = provider.Search(new StorageQueryForm(form)
            {
                Extension = FileRepository.TypeExtension(form.Type).Split('|')
            });
            //foreach (var item in items.Items)
            //{
            //    if (type == "image")
            //    {
            //        item.Thumb = provider.ToPublicUrl(item.Path);
            //    }
            //}
            return new Page<VirtualFileItem>(items.TotalItems, items.CurrentPage, items.ItemsPerPage)
            {
                Items = items.Items.Select(item => new VirtualFileItem(item.Name, item.Path)).ToArray(),
            };
        }


        public IPage<FileEntity> StorageSearch(StorageQueryForm form)
        {
            return storage.Search(form);
        }

        public void StorageRemove(int[] id)
        {
            var items = storage.Context.Files.Where(i => id.Contains(i.Id)).ToArray();
            if (items is null || items.Length == 0)
            {
                return;
            }
            foreach (var item in items)
            {
                var storage = Storage(item.Folder);
                if (storage is null)
                {
                    throw new Exception("unknown folder");
                }
                storage.Remove(item);
            }
        }

        public void StorageReload(int tag)
        {
            var storage = Storage(tag);
            if (storage is null)
            {
                throw new Exception("unknown folder");
            }
            storage.Reload();
        }

        public void StorageSync(int[] id)
        {
            var items = storage.Context.Files.Where(i => id.Contains(i.Id)).ToArray();
            if (items is null || items.Length == 0)
            {
                return;
            }
            foreach (var item in items)
            {
                var storage = Storage(item.Folder);
                if (storage is null)
                {
                    throw new Exception("unknown folder");
                }
                storage.SyncFile(item.Path);
            }
        }
    }
}
