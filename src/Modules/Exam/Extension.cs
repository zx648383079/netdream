using Microsoft.Extensions.DependencyInjection;
using NetDream.Modules.Exam.Repositories;

namespace NetDream.Modules.Exam
{
    public static class Extension
    {
        public static void ProvideExamRepositories(this IServiceCollection service)
        {
            service.AddScoped<QuestionRepository>();
        }
    }
}
