using Microsoft.EntityFrameworkCore;
using NetDream.Modules.OnlineDisk.Entities;
using NetDream.Modules.OnlineDisk.Forms;
using NetDream.Modules.OnlineDisk.Models;
using NetDream.Shared.Interfaces;
using NetDream.Shared.Models;
using NetDream.Shared.Providers;
using System;
using System.Linq;
using FileListItem = NetDream.Modules.OnlineDisk.Models.FileListItem;

namespace NetDream.Modules.OnlineDisk.Repositories
{
    public class ServerRepository(OnlineDiskContext db, IClientContext client)
    {
        public IPage<ServerListItem> ServerList(QueryForm form)
        {
            return db.Servers.Search(form.Keywords, "ip")
                .OrderByDescending(i => i.Status)
                .ThenByDescending(i => i.Id)
                .ToPage(form, i => i.SelectAs());
        }

        public IOperationResult<ServerEntity> Save(ServerForm data)
        {
            var model = data.Id > 0 ? db.Servers.Where(i => i.Id == data.Id)
                .SingleOrDefault() :
                new ServerEntity();
            if (model is null)
            {
                return OperationResult.Fail<ServerEntity>("id error");
            }
            model.Ip = data.Ip;
            model.Port = data.Port;
            model.CanUpload = data.CanUpload;
            model.UploadUrl = data.UploadUrl;
            model.PingUrl = data.PingUrl;
            model.DownloadUrl = data.DownloadUrl;
            db.Servers.Save(model, client.Now);
            db.SaveChanges();
            return OperationResult.Ok(model);
        }

        public IPage<FileListItem> FileList(ServerFileQueryForm form)
        {
            var query = db.Files.Search(form.Keywords, "name");
            if (form.Server > 0)
            {
                var fileId = db.ServerFiles.Where(i => i.ServerId == form.Server).Pluck(i => i.FileId);
                if (fileId.Length == 0)
                {
                    return new Page<FileListItem>();
                }
            }
            return query.OrderByDescending(i => i.Id)
                .ToPage(form, i => i.SelectAs());
        }

        public IOperationResult<FileEntity> FileSave(ServerFileForm data)
        {
            var model = data.Id > 0 ? db.Files.Where(i => i.Id == data.Id)
                .SingleOrDefault() :
                new FileEntity();
            if (model is null)
            {
                return OperationResult.Fail<FileEntity>("id error");
            }
            model.Name = data.Name;
            model.Extension = data.Extension;
            model.Md5 = data.Md5;
            model.Size = data.Size;
            db.Files.Save(model, client.Now);
            db.SaveChanges();
            return OperationResult.Ok(model);
        }

        public void RemoveFile(params int[] idItems)
        {
            db.Files.Where(i => idItems.Contains(i.Id))
                .ExecuteUpdate(setters => setters.SetProperty(i => i.DeletedAt, client.Now));
            db.SaveChanges();
        }

        public IOperationResult LinkClient(ServerLinkForm data)
        {
            var model = db.Servers.Where(i => i.Id == data.Server).SingleOrDefault();
            if (model is null)
            {
                return OperationResult.Fail("数据错误");
            }
            model.UploadUrl = data.UploadUrl;
            model.DownloadUrl = data.DownloadUrl;
            model.PingUrl = data.PingUrl;
            model.Ip = data.Ip;
            model.Port = data.Port;
            model.Status = 1;
            db.Servers.Save(model, client.Now);
            db.SaveChanges();
            if (data.Files is null || data.Files.Length == 0)
            {
                return OperationResult.Ok();
            }
            db.ServerFiles.Where(i => i.ServerId == model.Id).ExecuteDelete();
            db.SaveChanges();
            var i = 0;
            var chunkSize = 20;
            while (i < data.Files.Length)
            {
                var chunk = data.Files.Skip(i).Take(chunkSize).ToArray();
                var fileId = db.Files.Where(i => chunk.Contains(i.Md5)).Pluck(i => i.Id);
                foreach (var item in fileId)
                {
                    db.ServerFiles.Add(new ServerFileEntity()
                    {
                        ServerId = model.Id,
                        FileId = item
                    });
                }
            }
            db.SaveChanges();
            return OperationResult.Ok();
        }

        public bool CheckFile(string md5)
        {
            var model = db.Files.Where(i => i.Md5 == md5).SingleOrDefault();
            if (model is null)
            {
                return false;
            }
            var items = db.ServerFiles.Where(i => i.FileId == model.Id).Pluck(i => i.ServerId);
            if (items.Length == 0)
            {
                return false;
            }
            return db.Servers.Where(i => items.Contains(i.Id) && i.Status == 1).Any();
        }

        public void AsyncFile(ServerFileForm data, int serverId)
        {
            var model = db.Files.Where(i => i.Md5 == data.Md5).SingleOrDefault();
            ServerFileEntity? link = null;
            if (model is null)
            {
                model = new FileEntity()
                {
                    Name = data.Name,
                    Extension = data.Extension,
                    Md5 = data.Md5,
                    Size = data.Size,
                    CreatedAt = client.Now,
                    UpdatedAt = client.Now,
                };
                db.Files.Add(model);
                db.SaveChanges();
            } else
            {
                link = db.ServerFiles.Where(i => i.FileId == model.Id && i.ServerId == serverId)
                .SingleOrDefault();
            }
            if (link is null)
            {
                db.ServerFiles.Add(new ServerFileEntity()
                {
                    ServerId = serverId,
                    FileId = model.Id,
                });
                db.SaveChanges();
            }
        }

        public IOperationResult<ServerEntity> FindServer(string token)
        {
            var model = db.Servers.Where(i => i.Token == token).SingleOrDefault();
            if (model is null)
            {
                return OperationResult<ServerEntity>.Fail("token error");
            }
            return OperationResult.Ok(model);
        }
    }
}
