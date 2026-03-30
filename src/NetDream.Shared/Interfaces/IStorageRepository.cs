using NetDream.Shared.Interfaces.Forms;
using NetDream.Shared.Models;

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

        public IPage<FileListItem> Search(IQueryForm form);

        public IPage<FileListItem> SearchImages(IQueryForm form);

        public IOperationResult<FileUploadResult> UploadAudio(int user, IUploadFile file);

        public IOperationResult<FileUploadResult> UploadVideo(int user, IUploadFile file);

        public IOperationResult<FileUploadResult[]> UploadImages(int user, IUploadFileCollection files);

        public IOperationResult<FileUploadResult> UploadImage(int user, IUploadFile file);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="user"></param>
        /// <param name="content">data:image/png;base64,</param>
        /// <returns></returns>
        public IOperationResult<FileUploadResult> UploadBase64(int user, string content);

        public IOperationResult<FileUploadResult[]> UploadFiles(int user, IUploadFileCollection files);

        public IOperationResult<FileUploadResult> UploadFile(int user, IUploadFile file);
    }
}
