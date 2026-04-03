using NetDream.Shared.Interfaces.Forms;

namespace NetDream.Shared.Interfaces
{
    public interface IStorageRepository
    {
        /// <summary>
        /// 外部可以直接网址访问的目录
        /// </summary>
        public IStorageRepository Open { get; }
        /// <summary>
        /// 内部目录，无法直接访问
        /// </summary>
        public IStorageRepository Secret { get; }
        /// <summary>
        /// 临时目录
        /// </summary>
        public IStorageFolder Temporary { get; }
        /// <summary>
        /// 是否是公开目录
        /// </summary>
        public bool IsOpen { get; }

        public bool IsTypeExtension(string extension, string type);

        public string TypeExtension(string type);

        public IPage<IFileListItem> Search(IQueryForm form);

        public IPage<IFileListItem> SearchImages(IQueryForm form);

        public IOperationResult<IFileListItem> UploadAudio(int user, IUploadFile file);

        public IOperationResult<IFileListItem> UploadVideo(int user, IUploadFile file);

        public IOperationResult<IFileListItem[]> UploadImages(int user, IUploadFileCollection files);

        public IOperationResult<IFileListItem> UploadImage(int user, IUploadFile file);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="user"></param>
        /// <param name="content">data:image/png;base64,</param>
        /// <returns></returns>
        public IOperationResult<IFileListItem> UploadBase64(int user, string content);

        public IOperationResult<IFileListItem[]> UploadFiles(int user, IUploadFileCollection files);

        public IOperationResult<IFileListItem> UploadFile(int user, IUploadFile file);
        /// <summary>
        /// 根据文件名移除
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public IOperationResult Remove(string fileName);
        /// <summary>
        /// 根据 id 删除文件 
        /// </summary>
        /// <param name="idItems"></param>
        /// <returns></returns>
        public IOperationResult Remove(int[] idItems);

        /// <summary>
        /// 更新文件的属性
        /// </summary>
        public void Reload();
        /// <summary>
        /// 根据部分 id 更新文件的属性
        /// </summary>
        /// <param name="idItems"></param>
        public void Reload(int[] idItems);
    }


    public interface IFileListItem
    {
        public string Name { get; }

        public long Size { get; }

        public string Type { get; }

        public string Url { get; }

        public int UpdatedAt { get; }
        public int CreatedAt { get; }
    }
}
