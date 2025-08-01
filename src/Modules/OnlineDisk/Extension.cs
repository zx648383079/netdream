using Microsoft.Extensions.DependencyInjection;
using NetDream.Modules.OnlineDisk.Entities;
using NetDream.Modules.OnlineDisk.Models;
using NetDream.Modules.OnlineDisk.Repositories;
using System.Linq;

namespace NetDream.Modules.OnlineDisk
{
    public static class Extension
    {
        public static void ProvideContactRepositories(this IServiceCollection service)
        {
            service.AddScoped<DiskRepository>();
        }

        internal static IQueryable<DiskListItem> SelectAs(this IQueryable<DiskEntity> query)
        {
            return query.Select(i => new DiskListItem()
            {
                Id = i.Id,
                UserId = i.UserId,
                Name = i.Name,
                Extension = i.Extension,
                FileId = i.FileId,
                ParentId  = i.ParentId,
                CreatedAt = i.CreatedAt,
                UpdatedAt = i.UpdatedAt,
            });
        }

        internal static IQueryable<FileListItem> SelectAs(this IQueryable<FileEntity> query)
        {
            return query.Select(i => new FileListItem()
            {
                Id = i.Id,
                Name = i.Name,
                Extension = i.Extension,
                Md5 = i.Md5,
                DeletedAt = i.DeletedAt,
                Size = i.Size,
                Thumb = i.Thumb,
                CreatedAt = i.CreatedAt,
                UpdatedAt = i.UpdatedAt,
            });
        }

        internal static IQueryable<ShareListItem> SelectAs(this IQueryable<ShareEntity> query)
        {
            return query.Select(i => new ShareListItem()
            {
                Id = i.Id,
                UserId = i.UserId,
                Name = i.Name,
                Mode = i.Mode,
                DeathAt = i.CreatedAt,
                DownCount = i.DownCount,
                ViewCount = i.ViewCount,
                SaveCount = i.SaveCount,
                CreatedAt = i.CreatedAt,
                UpdatedAt = i.UpdatedAt,
            });
        }

        internal static IQueryable<ServerListItem> SelectAs(this IQueryable<ServerEntity> query)
        {
            return query.Select(i => new ServerListItem()
            {
                Id = i.Id,
                Ip = i.Ip,
                FileCount = i.FileCount,
                Status = i.Status,
                CreatedAt = i.CreatedAt,
                UpdatedAt = i.UpdatedAt,
            });
        }

        internal static IQueryable<FileListItem> SelectAs(this IQueryable<ClientFileEntity> query)
        {
            return query.Select(i => new FileListItem()
            {
                Id = i.Id,
                Name = i.Name,
                Extension = i.Extension,
                Md5 = i.Md5,
                Size = i.Size,
                CreatedAt = i.CreatedAt,
                UpdatedAt = i.UpdatedAt,
            });
        }
    }
}
