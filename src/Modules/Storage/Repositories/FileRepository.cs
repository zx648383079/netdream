using MimeMapping;
using NetDream.Modules.Storage.Models;
using NetDream.Shared.Interfaces;
using NetDream.Shared.Interfaces.Forms;
using NetDream.Shared.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace NetDream.Modules.Storage.Repositories
{
    public class FileRepository(
        StorageContext db,
        ISystemStorage storage) : IStorageRepository
    {
        /// <summary>
        /// 外部可以直接网址访问的目录
        /// </summary>
        public IStorageRepository Open => IsOpen ? this : new FileRepository(db, storage);
        /// <summary>
        /// 内部目录，无法直接访问
        /// </summary>
        public IStorageRepository Secret => !IsOpen ? this : new FileRepository(db, storage) { IsOpen = false };
        /// <summary>
        /// 临时目录
        /// </summary>
        public IStorageFolder Temporary => storage.Temporary;

        private IStorageFolder Folder => IsOpen ? storage.Open : storage.Secret;
        /// <summary>
        /// 是否是公开目录
        /// </summary>
        public bool IsOpen { get; private set; } = true;
        /// <summary>
        /// 根据拓展名判断是否是类型
        /// </summary>
        /// <param name="extension"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public bool IsTypeExtension(string extension, string type)
        {
            if (extension.StartsWith('.'))
            {
                extension = extension[1..];
            }
            var items = TypeExtension(type);
            if (string.IsNullOrWhiteSpace(items))
            {
                return false;
            }
            return items.Split('|').Contains(extension.ToLower());
        }

        /// <summary>
        /// 根据文件类型获取拓展名
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public string TypeExtension(string type)
        {
            var maps = new Dictionary<string, string>()
            {

            };
            if (maps.TryGetValue(type, out var res))
            {
                return res;
            }
            return string.Empty;
        }

        public IPage<IFileListItem> Search(IQueryForm form)
        {
            throw new NotImplementedException();
        }

        public IPage<IFileListItem> SearchImages(IQueryForm form)
        {
            throw new NotImplementedException();
        }

        public IOperationResult<IFileListItem> UploadAudio(int user, IUploadFile file)
        {
            if (!file.FileType.StartsWith("audio/"))
            {
                return OperationResult<IFileListItem>.Fail("只允许音频");
            }
            return UploadFile(user, file, GetRandomName());
        }

        public IOperationResult<IFileListItem> UploadVideo(int user, IUploadFile file)
        {
            if (!file.FileType.StartsWith("video/"))
            {
                return OperationResult<IFileListItem>.Fail("只允许视频");
            }
            return UploadFile(user, file, GetRandomName());
        }

        public IOperationResult<IFileListItem[]> UploadImages(int user, IUploadFileCollection files)
        {
            if (files.Where(i => !i.FileType.StartsWith("image/")).Any())
            {
                return OperationResult<IFileListItem[]>.Fail("只允许图片");
            }
            return UploadFiles(user, files);
        }

        public IOperationResult<IFileListItem> UploadImage(int user, IUploadFile file)
        {
            if (!file.FileType.StartsWith("image/"))
            {
                return OperationResult<IFileListItem>.Fail("只允许图片");
            }
            return UploadFile(user, file, GetRandomName());
        }

        public IOperationResult<IFileListItem> UploadBase64(int user, string content)
        {
            // data:image/png;base64,
            if (!content.StartsWith("data:image/"))
            {
                return OperationResult<IFileListItem>.Fail("只允许图片");
            }
            var i = content.IndexOf(";base64,");
            if (i <= 0)
            {
                return OperationResult<IFileListItem>.Fail("不是base64");
            }
            var extension = MimeUtility.GetExtensions(content[5..(i - 1)])?.FirstOrDefault();
            if (string.IsNullOrWhiteSpace(extension))
            {
                return OperationResult<IFileListItem>.Fail("只允许图片");
            }
            try
            {
                var data = Convert.FromBase64String(content[(i + 8)..]);
                var fileName = $"{GetRandomName()}.{extension}";
                using var fs = Folder.Create(fileName);
                fs?.Write(data, 0, data.Length);
                return OperationResult.Ok<IFileListItem>(new FileUploadResult(Path.GetFileName(fileName))
                {
                    Size = data.Length,
                });
            }
            catch (Exception)
            {
                return OperationResult<IFileListItem>.Fail("不是base64");
            }
        }

        public IOperationResult<IFileListItem[]> UploadFiles(int user, IUploadFileCollection files)
        {
            var items = new List<IFileListItem>(files.Count);
            foreach (var file in files)
            {
                var extension = MimeUtility.GetExtensions(file.FileType)?.FirstOrDefault();
                if (string.IsNullOrWhiteSpace(extension))
                {
                    continue;
                }
                try
                {
                    using var fs = file.OpenRead();
                    var fileName = $"{GetRandomName()}_{items.Count}.{extension}";
                    Folder.Create(fileName, fs);
                    items.Add(new FileUploadResult(Path.GetFileName(fileName))
                    {
                        Original = file.Name,
                        Size = file.Size,
                    });
                }
                catch (Exception)
                {
                    
                }
            }
            return OperationResult.Ok(items.ToArray());
        }

        public IOperationResult<IFileListItem> UploadFile(int user, IUploadFile file)
        {
            return UploadFile(user, file, GetRandomName());
        }

        private IOperationResult<IFileListItem> UploadFile(int user, IUploadFile file, string fileName)
        {
            var extension = MimeUtility.GetExtensions(file.FileType)?.FirstOrDefault();
            if (string.IsNullOrWhiteSpace(extension))
            {
                return OperationResult<IFileListItem>.Fail("不支持此格式");
            }
            try
            {
                using var fs = file.OpenRead();
                fileName = $"{fileName}.{extension}";
                Folder.Create(fileName, fs);
                return OperationResult.Ok<IFileListItem>(new FileUploadResult(Path.GetFileName(fileName))
                {
                    Original = file.Name,
                    Size = file.Size,
                });
            }
            catch (Exception ex)
            {
                return OperationResult<IFileListItem>.Fail(ex.Message);
            }
        }

        private static string GetRandomName()
        {
            var now = DateTime.Now;
            return $"{now:yyyyMM}/{now:ddHHmmss}{Random.Shared.Next()}";
        }

        public IOperationResult Remove(string fileName)
        {
            return OperationResult.Ok();
        }

        public IOperationResult Remove(int[] idItems)
        {
            return OperationResult.Ok();
        }

        public void Reload()
        {
        }

        public void Reload(int[] idItems)
        {
        }
    }
}
