using Microsoft.Extensions.DependencyInjection;
using NetDream.Modules.Note.Entities;
using NetDream.Modules.Note.Models;
using NetDream.Modules.Note.Repositories;
using System.Linq;

namespace NetDream.Modules.Note
{
    public static class Extension
    {
        public static void ProvideNoteRepositories(this IServiceCollection service)
        {
            service.AddScoped<NoteRepository>();
            service.AddScoped<StatisticsRepository>();
        }


        internal static IQueryable<NoteListItem> SelectAs(this IQueryable<NoteEntity> query)
        {
            return query.Select(i => new NoteListItem()
            {
                Id = i.Id,
                UserId = i.UserId,
                Html = i.Content,
                CreatedAt = i.CreatedAt,
            });
        }
    }
}
