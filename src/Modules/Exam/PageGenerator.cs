using Microsoft.EntityFrameworkCore;
using NetDream.Modules.Exam.Entities;
using NetDream.Modules.Exam.Models;
using NetDream.Modules.Exam.Repositories;
using NetDream.Shared.Helpers;
using NetDream.Shared.Providers;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace NetDream.Modules.Exam
{
    public class PageGenerator(ExamContext db)
    {
        public PageBuilder Create(int course, int type = 0)
        {
            if (type < 2)
            {
                return CreateId(db.Questions.Where(i => i.CourseId == course
                    && i.Type < 5)
                    .OrderBy(i => i.Id)
                    .Select(i => new RuleQuestion()
                    {
                        Id = i.Id,
                    }).ToArray(), type > 0)
                    .SetTitle(type < 1 ? "顺序练习" : "随机练习");
            }
            if (type == 3)
            {
                return CreateId(db.Questions.Where(i => i.CourseId == course 
                && i.Type < 5 && i.Easiness > 5)
                    .OrderBy(i => i.Id).Select(i => new RuleQuestion()
                    {
                        Id = i.Id,
                    }).ToArray(), true)
                    .SetTitle("难题练习");
            }
            var args = new int[] { 10, 5, 5, 3, 2 };
            return CreateId(QuestionByRule(args.Select((i,j) => new RuleRandomQuestion()
            {
                Course = course,
                Type = (byte)j,
                Amount = i
            }).ToArray()), false)
            .SetTitle("全真模拟");
        }

        public RuleQuestion[] QuestionByRule(RuleRandomQuestion[] data)
        {
            var items = new List<RuleQuestion>();
            var exist = new List<int>();
            foreach (var item in data)
            {
                if (item.Amount < 1)
                {
                    continue;
                }
                var args = db.Questions
                    .Where(i => i.Type < 5 && i.CourseId == item.Course && i.Type == item.Type && !exist.Contains(i.Id))
                    .OrderBy(i => EF.Functions.Random())
                    .Take(item.Amount)
                    .Pluck(i => i.Id);
                foreach (var id in args)
                {
                    exist.Add(id);
                    items.Add(new()
                    {
                        Id = id,
                        Score = item.Score
                    });
                }
            }
            return [..items];
        }

        public PageBuilder CreateId(RuleQuestion[] items, bool shuffle = false)
        {
            return new PageBuilder(db).Add(shuffle ? items.OrderBy(_ => Random.Shared.Next()) : items);
        }

        public FormattedQuestion FormatQuestion(QuestionEntity model, 
            string answer, 
            string dynamic = null, 
            bool shuffle = true)
        {
            var dynamicItmes = JsonSerializer.Deserialize<JsonObject>(StrHelper.Base64Decode(dynamic));
            var data = new QuestionRepository(db, null, null).Format(model, 0,
                dynamicItmes,
                true, shuffle);
            data.YourAnswer = answer;
            data.Right = new QuestionChecker(db).Check(model, answer, dynamicItmes) == 1 ? 1 : -1;
            return data;
        }
    }
}
