using Microsoft.Extensions.DependencyInjection;
using NetDream.Modules.Note.Repositories;

namespace NetDream.Modules.Note
{
    public static class Extension
    {
        public static void ProvideNoteRepositories(this IServiceCollection service)
        {
            service.AddScoped<NoteRepository>();
            service.AddScoped<StatisticsRepository>();
        }
    }
}
