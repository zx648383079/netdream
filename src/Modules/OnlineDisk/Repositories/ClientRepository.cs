using NetDream.Modules.OnlineDisk.Entities;
using NetDream.Modules.OnlineDisk.Forms;
using NetDream.Shared.Interfaces;
using NetDream.Shared.Models;
using NetDream.Shared.Providers;
using System.IO;
using System.Linq;
using FileListItem = NetDream.Modules.OnlineDisk.Models.FileListItem;

namespace NetDream.Modules.OnlineDisk.Repositories
{
    public class ClientRepository(OnlineDiskContext db, IClientContext client)
    {
        public const string SERVER_KEY = "disk_server_url";

        public IOperationResult<ServerSourceForm> Link(ServerSourceForm data)
        {
            var files = db.ClientFiles.Pluck(i => i.Md5);
            SendServer(new ServerLinkForm()
            {
                Files = files,
                UploadUrl = data.UploadUrl,
                DownloadUrl = data.DownloadUrl,
                PingUrl = data.PingUrl,
            }, data.ServerUrl);
            // cache().Set(SERVER_KEY, data["server_url"]);
            return OperationResult.Ok(data);
        }

        private IOperationResult SendServer(ServerLinkForm data, string serverUrl = "")
        {
            if (string.IsNullOrWhiteSpace(serverUrl))
            {
                // serverUrl = cache(SERVER_KEY);
            }
            if (string.IsNullOrWhiteSpace(serverUrl))
            {
                return OperationResult.Fail("server error");
            }
            data.Token = "";//config("disk.server_token");
            return OperationResult.Ok();
        }

        public IPage<FileListItem> FileList(QueryForm form)
        {
            return db.ClientFiles.Search(form.Keywords, "name")
                .OrderByDescending(i => i.Id)
                .ToPage(form, i => i.SelectAs());
        }


        public IOperationResult<FileListItem> UploadFile(DiskFileForm data)
        {
            return OperationResult<FileListItem>.Fail("未实现");
        }

        public IOperationResult<FileListItem> UploadChunk(DiskChunkForm data)
        {
            return OperationResult<FileListItem>.Fail("未实现");
        }

        public IOperationResult<FileListItem> UploadChunkMerge(DiskChunkFinishForm data)
        {
            return OperationResult<FileListItem>.Fail("未实现");
        }

        private IOperationResult<FileListItem> SaveUpload(string name, string md5, string location, int size)
        {
            var model = new ClientFileEntity()
            {
                Name = name,
                Extension = Path.GetExtension(name),
                Md5 = md5,
                Location = location,
                Size = size,
                CreatedAt = client.Now,
                UpdatedAt = client.Now,
            };
            db.ClientFiles.Add(model);
            db.SaveChanges();
            if (model.Id == 0)
            {
                return OperationResult.Fail<FileListItem>("添加失败");
            }
            // TODO 上报主服务器
            SendServer(new ServerLinkForm()
            {
                Files = [model.Md5]
            });
            return OperationResult.Ok(new FileListItem(model));
        }

        public IOperationResult<IDownloadFile> Download(string md5)
        {
            var model = db.ClientFiles.Where(i => i.Md5 == md5).FirstOrDefault();
            if (model is null)
            {
                return OperationResult<IDownloadFile>.Fail("file error");
            }
            return OperationResult.Ok<IDownloadFile>(new FileResult(model.Name, model.Location));
        }
    }
}
