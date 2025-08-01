using NetDream.Modules.OnlineDisk.Forms;
using NetDream.Modules.OnlineDisk.Models;
using NetDream.Shared.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetDream.Modules.OnlineDisk.Adapters
{
    public class DatabaseAdapter(OnlineDiskContext db, IClientContext client) : IDiskAdapter
    {
        public IPage<DiskListItem> Catalog(CatalogQueryForm form)
        {
            throw new NotImplementedException();
        }

        public IOperationResult<DiskListItem> Copy(DiskMoveForm data)
        {
            throw new NotImplementedException();
        }

        public IOperationResult<DiskListItem> Create(DiskFolderForm data)
        {
            throw new NotImplementedException();
        }

        public IOperationResult<DiskListItem> File(string id)
        {
            throw new NotImplementedException();
        }

        public IOperationResult<DiskListItem> Move(DiskMoveForm data)
        {
            throw new NotImplementedException();
        }

        public IOperationResult Remove(int id)
        {
            throw new NotImplementedException();
        }

        public IOperationResult<DiskListItem> Rename(DiskRenameForm data)
        {
            throw new NotImplementedException();
        }

        public IPage<DiskQueryForm> Search(DiskQueryForm form)
        {
            throw new NotImplementedException();
        }

        public IOperationResult<DiskListItem> uploadCheck(DiskCheckForm data)
        {
            throw new NotImplementedException();
        }

        public object uploadChunk(DiskChunkForm data)
        {
            throw new NotImplementedException();
        }

        public IOperationResult<DiskListItem> uploadFile(DiskFileForm data)
        {
            throw new NotImplementedException();
        }

        public IOperationResult<DiskListItem> uploadFinish(DiskChunkFinishForm data)
        {
            throw new NotImplementedException();
        }
    }
}
