using NetDream.Shared.Interfaces;
using NetDream.Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace NetDream.Modules.Storage.Repositories
{
    public class FileRepository(
        StorageContext db,
        ISystemStorage storage) : IStorageRepository
    {

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

        public IPage<FileListItem> Search(QueryForm form)
        {
            throw new NotImplementedException();
        }

        public IPage<FileListItem> SearchImages(QueryForm form)
        {
            throw new NotImplementedException();
        }

        public IOperationResult<FileUploadResult> UploadAudio(int user, IUploadFile file)
        {
            throw new NotImplementedException();
        }

        public IOperationResult<FileUploadResult> UploadVideo(int user, IUploadFile file)
        {
            throw new NotImplementedException();
        }

        public IOperationResult<FileUploadResult[]> UploadImages(int user, IUploadFileCollection files)
        {
            throw new NotImplementedException();
        }

        public IOperationResult<FileUploadResult> UploadImage(int user, IUploadFile file)
        {
            throw new NotImplementedException();
        }

        public IOperationResult<FileUploadResult> UploadBase64(int user, string content)
        {
            throw new NotImplementedException();
        }

        public IOperationResult<FileUploadResult[]> UploadFiles(int user, IUploadFileCollection files)
        {
            throw new NotImplementedException();
        }

        public IOperationResult<FileUploadResult> UploadFile(int user, IUploadFile file)
        {
            throw new NotImplementedException();
        }
    }
}
