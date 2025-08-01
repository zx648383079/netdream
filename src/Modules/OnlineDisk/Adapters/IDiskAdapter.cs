using NetDream.Modules.OnlineDisk.Forms;
using NetDream.Modules.OnlineDisk.Models;
using NetDream.Shared.Interfaces;

namespace NetDream.Modules.OnlineDisk.Adapters
{
    public interface IDiskAdapter
    {
        public IPage<DiskListItem> Catalog(CatalogQueryForm form);

        public IPage<DiskListItem> Search(DiskQueryForm form);

        public IOperationResult Remove(string id);

        public IOperationResult<DiskListItem> Create(DiskFolderForm data);

        /// <summary>
        /// 上传单个文件
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public IOperationResult<DiskListItem> UploadFile(DiskFileForm data);

        /// <summary>
        /// 上传分块
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public IOperationResult<DiskChunkResult> UploadChunk(DiskChunkForm data);

        /// <summary>
        /// 合并分块
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public IOperationResult<DiskListItem> UploadFinish(DiskChunkFinishForm data);

        /// <summary>
        /// 验证MD5
        /// </summary>
        /// <param name="data"></param>
        /// <param name="name"></param>
        /// <param name=""></param>
        /// <param name="parentId"></param>
        /// <returns></returns>
        public IOperationResult<DiskListItem> UploadCheck(DiskCheckForm data);

        public IOperationResult<DiskListItem> Rename(DiskRenameForm data);

        public IOperationResult<DiskListItem> Copy(DiskMoveForm data);

        public IOperationResult<DiskListItem> Move(DiskMoveForm data);

        public IOperationResult<DiskListItem> File(string id);
    }
}
