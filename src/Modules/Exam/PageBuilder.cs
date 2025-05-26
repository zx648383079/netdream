using NetDream.Modules.Exam.Entities;
using NetDream.Modules.Exam.Models;
using NetDream.Modules.Exam.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace NetDream.Modules.Exam
{
    public class PageBuilder(ExamContext db)
    {
        private int _index = 0;
        private List<PageQuestionItem> _items = [];
        public int Id { get; set; }
        public int PageId { get; set; }
        public string Title { get; set; }
        public int StartTime { get; set; }
        public int LimitTime { get; set; } = 120;
        
        public bool Finished { get; set; }

        public int Count => _items.Count;

        public void SelectedId(int id)
        {
            if (id == 0)
            {
                return;
            }
            for (int i = 0; i < _items.Count; i++)
            {
                if (_items[i].Id == id)
                {
                    _index = i;
                    break;
                }
            }
        }

        public PageBuilder Append(int id)
        {
            _items.Add(new()
            {
                Id = id,
            });
            return this;
        }

        public bool Answer(int id, string answer, string dynamic = null)
        {
            SelectedId(id);
            return Judge(answer, dynamic);
        }

        public bool Judge(string answer, string dynamic = null)
        {
            var model = GetQuestion();
            var right = new QuestionChecker(db).Check(model, answer, JsonSerializer.Deserialize<JsonObject>(dynamic)) == 1 ? 1 : -1;
            var item = _items[_index];
            item.Answer = answer;
            item.Dynamic = dynamic;
            item.Right = right;
            return right > 0;
        }

        public PageBuilder Finish()
        {
            Finished = true;
            foreach (var item in _items)
            {
                if (item.Right == 0)
                {
                    item.Right = -1;
                    item.Answer = string.Empty;
                }
            }
            return this;
        }

        public JsonObject GetPage(int page = 1, int per_page = 1)
        {
            if (page < 1)
            {
                page = 1;
            }
            var start = (page - 1) * per_page;
            var items = new JsonArray();
            var len = _items.Count;
            var end = Math.Min(per_page, len - start);
            for (var i = 0; i < end; i++)
            {
                start += i;
                items.Add(Format(start));
            }
            var previous = 0;
            var next = 0;
            if (page > 1)
            {
                previous = page - 1;
            }
            if (start < len - 1)
            {
                next = page + 1;
            }
            return new JsonObject()
            {
                ["items"] = items,
                ["previous"] = previous,
                ["next"] = next,
            };
        }

        public JsonObject GetReport()
        {
            var wrong = 0;
            var right = 0;
            var score = 0;
            foreach (var item in _items)
            {
                if (item.Right == 0)
                {
                    continue;
                }
                if (item.Right > 0)
                {
                    right++;
                    score += item.Score;
                    continue;
                }
                if (item.Right < 0)
                {
                    wrong++;
                    continue;
                }
            }
            var scale = 100.0;
            if (wrong > 0 || right > 0)
            {
                scale = Math.Round((double)right * 100 / (wrong + right), 2);
            }
            return new JsonObject() 
            {
                ["wrong"] = wrong,
                ["right"] = right,
                ["scale"] = scale,
                ["score"]  = score
            };
        }

        public PageCardItem[] GetCard()
        {
            return _items.Select((i, j) => new PageCardItem()
            {
                Order = j + 1,
                Id = i.Id,
                Right = i.Right
            }).ToArray();
        }

        private JsonObject Format(int i = -1)
        {
            if (i < 0)
            {
                i = _index;
            }
            if (i < 0 || i >= _items.Count)
            {
                throw new Exception("题目错误");
            }
            var item = _items[i];
            var model = GetQuestion(i);
            var data = new QuestionRepository(db, null, null)
                .Format(model, i + 1,
                item.Dynamic,
                Finished);
            data.Right = item.Right;
            data.YourAnswer = item.Answer;
            data.MaxScore = item.Score;
            return JsonSerializer.Deserialize<JsonObject>(JsonSerializer.Serialize(data));
        }

        /**
         * @param int i
         * @return bool|QuestionModel
         * @throws \Exception
         */
        private QuestionEntity? GetQuestion(int i = -1)
        {
            if (i < 0)
            {
                i = _index;
            }
            if (i < 0 || i >= _items.Count)
            {
                throw new Exception("题目错误");
            }
            var item = _items[i];
            if (item.Model is not null)
            {
                return item.Model;
            }
            return item.Model = db.Questions.Where(i => i.Id == item.Id).Single();
        }

        /**
         * Get the instance as an array.
         *
         * @return array
         * @throws Exception
         */
        public JsonObject ToArray()
        {
            var items = new JsonArray();
            for (var i = 0; i < _items.Count; i++)
            {
                items.Add(Format(i));
            }
            var res = new JsonObject
            {
                ["id"] = Id,
                ["page_id"] = PageId,
                ["title"] = Title,
                ["time"] = LimitTime,
                ["start_time"] = StartTime,
                ["finished"] = Finished,
                ["data"] = items
            };
            if (Finished)
            {
                res["report"] = GetReport();
            }
            return res;
        }

        public PageBuilder Add(IEnumerable<RuleQuestion> items)
        {
            foreach (var item in items)
            {
                _items.Add(new PageQuestionItem()
                {
                    Id = item.Id,
                    Score = item.Score,
                });
            }
            return this;
        }

        public PageBuilder SetTitle(string val)
        {
            Title = val;
            return this;
        }

        private class PageQuestionItem
        {
            public int Id { get; set; }

            public string Answer { get; set; }
            public string Dynamic { get; set; }
            public int Right { get; set; }
            public QuestionEntity? Model { get; set; }
            public int Score { get; internal set; }
        }

        public class PageCardItem
        {
            public int Order { get; set; }
            public int Id { get; set; }
            public int Right { get; set; }
        }
    }

}
