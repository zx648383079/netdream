using NetDream.Modules.Exam.Models;
using System;

namespace NetDream.Modules.Exam.Repositories
{
    public class StatisticsRepository(ExamContext db)
    {
        public StatisticsResult Subtotal()
        {
            var res = new StatisticsResult();
            return res;
        }
    }
}
