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
    public class LocationAdapter : IDiskAdapter
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

        public IOperationResult Remove(string id)
        {
            throw new NotImplementedException();
        }

        public IOperationResult<DiskListItem> Rename(DiskRenameForm data)
        {
            throw new NotImplementedException();
        }


        public IOperationResult<DiskListItem> UploadCheck(DiskCheckForm data)
        {
            throw new NotImplementedException();
        }


        public IOperationResult<DiskChunkResult> UploadChunk(DiskChunkForm data)
        {
            throw new NotImplementedException();
        }


        public IOperationResult<DiskListItem> UploadFile(DiskFileForm data)
        {
            throw new NotImplementedException();
        }


        public IOperationResult<DiskListItem> UploadFinish(DiskChunkFinishForm data)
        {
            throw new NotImplementedException();
        }

        public IPage<DiskListItem> Search(DiskQueryForm form)
        {
            throw new NotImplementedException();
        }
    }
}
