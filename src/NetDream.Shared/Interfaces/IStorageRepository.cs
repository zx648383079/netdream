using NetDream.Shared.Models;

namespace NetDream.Shared.Interfaces
{
    public interface IStorageRepository
    {

        public bool IsTypeExtension(string extension, string type);

        public string TypeExtension(string type);

        public IPage<FileListItem> Search(QueryForm form);

        public IPage<FileListItem> SearchImages(QueryForm form);

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
