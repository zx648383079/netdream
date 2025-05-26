using NetDream.Modules.Exam.Entities;
using NetDream.Shared.Providers;
using System;
using System.Linq;
using System.Text.Json.Nodes;

namespace NetDream.Modules.Exam
{
    public class QuestionChecker(ExamContext db)
    {
        public float Check(QuestionEntity question, JsonNode answer, 
            JsonObject? dynamic = null)
        {
            if (question.Type < 1)
            {
                return CheckType0(question, answer.ToString());
            }
            if (question.Type == 1)
            {
                return CheckType1(question, answer.AsArray().Select(i => i.ToString()).ToArray());
            }
            if (question.Type == 2)
            {
                return CheckType2(question, answer.ToString());
            }
            if (question.Type == 3)
            {
                return CheckType3(question, answer.ToString(), dynamic);
            }
            if (question.Type == 4)
            {
                return CheckType4(question, answer.AsArray().Select(i => i.ToString()).ToArray(), dynamic);
            }
            return 0;
        }

        private float CheckType0(QuestionEntity question, string answer)
        {
            var id = int.Parse(answer);
            return db.QuestionOptions.Where(i => i.QuestionId == question.Id
                && i.Id == id && i.IsRight == 1).Any() ? 1 : 0;
        }

        private float CheckType1(QuestionEntity question, string[] answer)
        {
            var items = db.QuestionOptions.Where(i => i.QuestionId == question.Id && i.IsRight == 1).Pluck(i => i.Id);
            return answer.Length == items.Length && !items.Where(i => !answer.Contains(i.ToString())).Any() ? 1 : 0;
        }

        private float CheckType2(QuestionEntity question, string answer)
        {
            return int.Parse(question.Answer) == int.Parse(answer) ? 1 : 0;
        }

        private static float CheckType3(QuestionEntity question, string answer, JsonObject dynamic)
        {
            // 完全对比有问题
            return QuestionCompiler.StrReplace(question.Answer, dynamic) == answer ? 1 : 0;
        }

        private static float CheckType4(QuestionEntity question, string[] answer, JsonObject dynamic)
        {
            var items = question.Answer.Split('\n');
            var total = items.Length;
            var right = 0;
            for (var i = 0; i < items.Length; i ++)
            {
                var line = items[i].Trim();
                if (line.StartsWith('='))
                {
                    line = QuestionCompiler.CompilerValue(line[1..], dynamic).ToString();
                }
                if (answer.Length <= i || answer[i] != line)
                {
                    continue;
                }
                right++;
            }
            return right / total;
        }
    }
}
