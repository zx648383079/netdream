using Microsoft.EntityFrameworkCore;
using NetDream.Modules.Exam.Entities;
using NetDream.Modules.Exam.Forms;
using NetDream.Modules.Exam.Models;
using NetDream.Shared.Helpers;
using NetDream.Shared.Interfaces;
using NetDream.Shared.Models;
using NetDream.Shared.Providers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace NetDream.Modules.Exam.Repositories
{
    public class QuestionRepository(ExamContext db, 
        IClientContext client,
        IUserRepository userStore)
    {
        public IPage<QuestionListItem> GetList(QueryForm form, int course = 0, 
            int user = 0, int grade = 0, int material = 0, bool filter = false)
        {
            var res = db.Questions
                .When(course > 0, i => i.CourseId == course)
                .Search(form.Keywords, "title")
                .When(user > 0, i => i.UserId == user)
                .When(material > 0, i => i.MaterialId == material)
                .When(grade > 0, i => i.CourseGrade == grade)
                .When(filter, i => i.Type < 5)
                .OrderByDescending(i => i.Id)
                .ToPage<QuestionEntity, QuestionListItem>(form);
            userStore.Include(res.Items);
            CourseRepository.Include(db, res.Items);
            var provider = new CourseRepository(db, client);
            foreach (var item in res.Items)
            {
                item.CourseGradeFormat = provider.FormatGrade(item.CourseId, item.CourseGrade);
            }
            return res;
        }

        public IPage<IQuestionModel> SearchList(QueryForm form, int course = 0, int user = 0, int grade = 0, bool full = false)
        {
            var query = db.Questions.When(course > 0, i => i.CourseId == course)
                .Search(form.Keywords, "title")
                .When(user > 0, i => i.UserId == user)
                .When(grade > 0, i => i.CourseGrade == grade)
                .OrderByDescending(i => i.Id);
            if (full)
            {
                var res = query.ToPage<QuestionEntity, QuestionModel>(form);
                CourseRepository.Include(db, res.Items);
                userStore.Include(res.Items);
                IncludeOption(db, res.Items);
                IncludeAnalysis(db, res.Items);
                foreach (var item in res.Items)
                {
                    item.Editable = item.UserId == client.UserId;
                }
                return res.Cast<IQuestionModel>();
            } 
            else
            {
                var res = query.Select(i => new QuestionListItem()
                {
                    Id = i.Id,
                    Title = i.Title,
                    CourseId = i.CourseId,
                    UserId = i.UserId,
                    Type = i.Type,
                    Easiness = i.Easiness,
                    CreatedAt = i.CreatedAt,
                }).ToPage(form);
                CourseRepository.Include(db, res.Items);
                userStore.Include(res.Items);
                return res.Cast<IQuestionModel>();
            }
        }

        

        public IPage<QuestionListItem> SelfList(QueryForm form, int course = 0)
        {
            return GetList(form, course, client.UserId);
        }

        public IOperationResult<QuestionEntity> Get(int id)
        {
            var model = db.Questions.Where(i => i.Id == id).SingleOrDefault();
            if (model == null)
            {
                return OperationResult<QuestionEntity>.Fail("数据有误");
            }
            return OperationResult.Ok(model);
        }

        public IOperationResult<QuestionModel> GetFull(int id, int user = 0)
        {
            var model = db.Questions.Where(i => i.Id == id).SingleOrDefault();
            if (model == null)
            {
                return OperationResult<QuestionModel>.Fail("数据有误");
            }
            if (user > 0 && model.UserId != user)
            {
                return OperationResult<QuestionModel>.Fail("数据有误");
            }
            var res = FormatItem(model);
            return OperationResult.Ok(res);
        }

        private QuestionModel FormatItem(QuestionEntity model)
        {
            var res = model.CopyTo<QuestionModel>();
            res.OptionItems = db.QuestionOptions.Where(i => i.QuestionId == model.Id).ToArray();
            res.AnalysisItems = db.QuestionAnalysis.Where(i => i.QuestionId == model.Id).ToArray();
            if (model.MaterialId > 0)
            {
                res.Material = db.QuestionMaterials.Where(i => i.Id == model.MaterialId).SingleOrDefault();
            }
            if (model.Type == 5)
            {
                res.Children = db.Questions.Where(i => i.ParentId == model.Id)
                    .ToList().Select(FormatItem).ToArray();
            }
            return res;
        }

        public IOperationResult<QuestionModel> SelfFull(int id)
        {
            return GetFull(id, client.UserId);
        }

        public IOperationResult<QuestionEntity> Save(QuestionForm data, int user = 0, bool check = false, bool addKid = true)
        {
            var isLarge = data.Type == 5;
            if (check && data.Id < 1 && CheckRepeat(data)) {
                return OperationResult<QuestionEntity>.Fail("请不要重复添加");
            }
            if (isLarge && (data.Children is null || data.Children.Length == 0))
            {
                return OperationResult<QuestionEntity>.Fail("大题下面必须包含小题");
            }
            var model = data.Id > 0 ? db.Questions.Where(i => i.Id == data.Id).SingleOrDefault() 
                : new QuestionEntity();
            if (model is null || (data.Id > 0 && user > 0 && model.UserId != user))
            {
                return OperationResult<QuestionEntity>.Fail("无法保存");
            }
            if (data.MaterialId <= 0 && data.Material is not null)
            {
                var m = new MaterialRepository(db, client).Save(data.Material);
                if (m.Succeeded)
                {
                    data.MaterialId = m.Result.Id;
                }
            }
            model.Title = data.Title;
            model.CourseGrade = data.CourseGrade;
            model.CourseId = data.CourseId;
            model.Answer = data.Answer;
            model.MaterialId = data.MaterialId;
            model.Content = data.Content;
            model.Dynamic = data.Dynamic;
            model.Image = data.Image;
            model.ParentId = data.ParentId;
            model.Type = data.Type;
            model.Easiness = data.Easiness;
            model.UserId = client.UserId;
            db.Questions.Save(model, client.Now);
            db.SaveChanges();
            if (!isLarge && data.OptionItems?.Length > 0)
            {
                BatchSaveOption(model, data.OptionItems);
            }
            if (data.AnalysisItems?.Length > 0)
            {
                BatchSaveAnalysis(model, data.AnalysisItems);
            }
            if (addKid && isLarge)
            {
                foreach (var item in data.Children)
                {
                    item.ParentId = model.Id;
                    item.CourseId = model.CourseId;
                    item.CourseGrade = model.CourseGrade;
                    item.Easiness = model.Easiness;
                    item.Dynamic = data.Dynamic;
                    if (item.Type == 4 && string.IsNullOrWhiteSpace(item.Content))
                    {
                        item.Content = item.Title;
                    }
                    Save(item, user);
                }
            }
            return OperationResult.Ok(model);
        }

        private void BatchSaveAnalysis(QuestionEntity model, AnalysisForm[] items)
        {
            var idItems = db.QuestionAnalysis.Where(i => i.QuestionId == model.Id)
                .Pluck(i => i.Id);
            var exist = new List<int>();
            foreach (var item in items)
            {
                if (string.IsNullOrWhiteSpace(item.Content))
                {
                    continue;
                }
                if (item.Id > 0 && idItems.Contains(item.Id))
                {
                    exist.Add(item.Id);
                    db.QuestionAnalysis.Where(i => i.QuestionId == model.Id && i.Id == item.Id)
                        .ExecuteUpdate(setters => setters.SetProperty(i => i.Type, item.Type)
                        .SetProperty(i => i.Content, item.Content));
                    continue;
                }
                db.QuestionAnalysis.Add(new QuestionAnalysisEntity()
                {
                    QuestionId = model.Id,
                    Content = model.Content,
                    Type = item.Type,
                });
            }
            if (idItems.Length == 0)
            {
                db.SaveChanges();
                return;
            }
            var del = ModelHelper.Diff(idItems, exist.ToArray());
            if (del.Length == 0)
            {
                return;
            }
            db.QuestionAnalysis.Where(i => i.QuestionId == model.Id
                 && del.Contains(i.Id)).ExecuteDelete();
            db.SaveChanges();
        }

        private void BatchSaveOption(QuestionEntity model, OptionForm[] items)
        {
            if (model.Type > 1 && model.Type < 4)
            {
                db.QuestionOptions.Where(i => i.QuestionId == model.Id).ExecuteDelete();
                db.SaveChanges();
                return;
            }
            var idItems = db.QuestionOptions.Where(i => i.QuestionId == model.Id)
                .Pluck(i => i.Id);
            var exist = new List<int>();
            var hasRight = false;
            foreach (var item in items)
            {
                if (string.IsNullOrWhiteSpace(item.Content))
                {
                    continue;
                }
                if (model.Type < 1 && hasRight && item.IsRight > 0)
                {
                    item.IsRight = 0;
                }
                if (item.IsRight > 0)
                {
                    hasRight = true;
                }
                if (item.Id > 0 && idItems.Contains(item.Id))
                {
                    exist.Add(item.Id);
                    db.QuestionOptions.Where(i => i.QuestionId == model.Id && i.Id == item.Id)
                        .ExecuteUpdate(setters => setters.SetProperty(i => i.Type, item.Type)
                        .SetProperty(i => i.Content, item.Content)
                        .SetProperty(i => i.IsRight, item.IsRight));
                    return;
                }
                db.QuestionOptions.Add(new()
                {
                    QuestionId = model.Id,
                    Content = model.Content,
                    Type = item.Type,
                    IsRight = item.IsRight,
                });
            }
            if (idItems.Length == 0)
            {
                db.SaveChanges();
                return;
            }
            var del = ModelHelper.Diff(idItems, exist.ToArray());
            if (del.Length == 0)
            {
                return;
            }
            db.QuestionOptions.Where(i => i.QuestionId == model.Id
                 && del.Contains(i.Id)).ExecuteDelete();
            db.SaveChanges();
        }

        public bool CheckRepeat(QuestionForm data)
        {
            var userId = client.UserId;
            return db.Questions.Where(i => i.Type == data.Type 
                && i.Title == data.Title && i.UserId == userId)
                .When(data.CourseId > 0, i => i.CourseId == data.CourseId)
                .When(string.IsNullOrWhiteSpace(data.Content), i => i.Content == data.Content)
                .When(string.IsNullOrWhiteSpace(data.Image), i => i.Image == data.Image)
                .Any();
        }

        public IOperationResult<QuestionEntity> SelfSave(QuestionForm data, bool addKid = true)
        {
            return Save(data, client.UserId, false, addKid);
        }

        public IOperationResult Remove(int id, int user = 0)
        {
            var model = db.Questions.Where(i => i.Id == id)
                .When(user > 0, i => i.UserId == user).SingleOrDefault();
            if (model is null)
            {
                return OperationResult.Fail("无权限删除");
            }
            db.Questions.Remove(model);
            var idItems = new List<int>() 
            {
                id
            };
            if (model.Type == 5)
            {
                idItems.AddRange(db.Questions.Where(i => i.ParentId == model.Id)
                    .Select(i => i.Id).ToArray());
            }
            db.QuestionAnswers.Where(i => idItems.Contains(i.QuestionId)).ExecuteDelete();
            db.QuestionAnalysis.Where(i => idItems.Contains(i.QuestionId)).ExecuteDelete();
            db.QuestionOptions.Where(i => idItems.Contains(i.QuestionId)).ExecuteDelete();
            db.SaveChanges();
            return OperationResult.Ok();
        }

        public IOperationResult SelfRemove(int id)
        {
            return Remove(id, client.UserId);
        }

        public IPage<QuestionListItem> Search(QueryForm form, int[] idItems)
        {
            return db.Questions.Search(form.Keywords, "title")
                .Where(i => idItems.Contains(i.Id))
                .ToPage<QuestionEntity, QuestionListItem>(form);
        }

        public QuestionListItem[] Check(string title, int id = 0)
        {
            var res = db.Questions.Where(i => i.Title == title && i.Id != id)
                .OrderByDescending(i => i.Id)
                .Select(i => new QuestionListItem()
                {
                    Id = i.Id,
                    Title = title,
                    UserId = i.UserId,
                    CourseId = i.CourseId,
                })
                .ToArray();
            userStore.Include(res);
            CourseRepository.Include(db, res);
            return res;
        }

        public QuestionListItem[] Suggestion(string keywords, int course = 0)
        {
            var res = db.Questions
                    .When(course > 0, i => i.CourseId == course)
                    .Search(keywords, "title")
                    .OrderByDescending(i => i.Id)
                    .Take(5)
                    .Select(i => new QuestionListItem()
                    {
                        Id = i.Id,
                        Title = i.Title,
                        CourseId = i.CourseId,
                        Type = i.Type,
                        Easiness = i.Easiness,
                    }).ToArray();
            CourseRepository.Include(db, res);
            return res;
        }

        public IOperationResult<QuestionEntity> CrawlSave(QuestionForm data)
        {
            return Save(data);
        }

        internal FormattedQuestion Format(QuestionEntity question, 
            int order, 
            string content, bool hasAnswer = false, bool shuffle = true)
        {
            return Format(question, order, 
                JsonSerializer.Deserialize<JsonObject>(StrHelper.Base64Decode(content)), hasAnswer, shuffle);
        }

        internal FormattedQuestion Format(QuestionEntity question,
            int order,
            JsonObject dynamicItems, bool hasAnswer = false, bool shuffle = true)
        {
            var data = new FormattedQuestion()
            {
                Order = order == 0 ? question.Id : order,
                Id = question.Id,
                Title = QuestionCompiler.StrReplace(question.Title, dynamicItems),
                Image = question.Image,
                Content = QuestionCompiler.StrReplace(question.Content, dynamicItems),
                Dynamic = StrHelper.Base64Encode(JsonSerializer.Serialize(dynamicItems)),
                Type = question.Type,
            };
            if (question.MaterialId > 0)
            {
                data.Material = db.QuestionMaterials.Where(i => i.Id == question.MaterialId).SingleOrDefault();
            }
            if (question.ParentId > 0)
            {
                data.Parent = Format(db.Questions.Where(i => i.Id == question.ParentId).Single(), 0, dynamicItems, hasAnswer, shuffle);
            }
            var optionItems = db.QuestionOptions.Where(i => i.QuestionId == question.Id)
                .OrderBy(i => i.Id).ToArray();
            if (hasAnswer)
            {
                data.Answer = question.Type == 4
                    ? GetType4Answer(optionItems, dynamicItems)
                    : QuestionCompiler.StrReplace(question.Answer, dynamicItems);
                data.Analysis = QuestionCompiler.StrReplace(db.QuestionAnalysis.Where(i => i.QuestionId == question.Id && i.Type == 0)
                    .Value(i => i.Content), dynamicItems);
            }
            if (question.Type < 2)
            {
                var optionSorted = optionItems;
                if (shuffle)
                {
                    var random = new Random();
                    optionSorted = optionItems.OrderBy(_ => random.Next()).ToArray();
                }
                var i = 0;
                data.Option = new FormattedQuestionOption[optionItems.Length];
                foreach (var item in optionSorted)
                {
                    i++;
                    var option = new FormattedQuestionOption()
                    {
                        Id = item.Id,
                        Content = item.Type > 0 ? item.Content :
                        QuestionCompiler.StrReplace(item.Content, dynamicItems),
                        Order = QuestionCompiler.IntToChr(i),
                    };
                    if (hasAnswer)
                    {
                        option.IsRight = item.IsRight > 0;
                    }
                    data.Option[i] = option;
                }
            }
            return data;
        }

        private static JsonArray GetType4Answer(QuestionOptionEntity[] optionItems, JsonObject dynamic)
        {
            var items = new List<string>();
            foreach (var item in  optionItems)
            {
                var line = item.Content.Trim();
                if (line.StartsWith('='))
                {
                    line = QuestionCompiler.CompilerValue(line[1..], 
                        dynamic).ToString();
                }
                items.Add(line);
            }
            return [..items];
        }

        public static void IncludeAnalysis(ExamContext db, QuestionModel[] items)
        {
            if (items.Length == 0)
            {
                return;
            }
            var idItems = items.Select(i => i.Id).ToArray();
            var data = db.QuestionAnalysis.Where(i => idItems.Contains(i.QuestionId))
                .ToArray();
            if (data.Length == 0)
            {
                return;
            }
            foreach (var item in items)
            {
                item.AnalysisItems = data.Where(i => i.QuestionId == item.Id).ToArray();
            }
        }

        public static void IncludeOption(ExamContext db, QuestionModel[] items)
        {
            if (items.Length == 0)
            {
                return;
            }
            var idItems = items.Select(i => i.Id).ToArray();
            var data = db.QuestionOptions.Where(i => idItems.Contains(i.QuestionId))
                .ToArray();
            if (data.Length == 0)
            {
                return;
            }
            foreach (var item in items)
            {
                item.OptionItems = data.Where(i => i.QuestionId == item.Id).ToArray();
            }
        }

        public static void IncludeMaterial(ExamContext db, QuestionModel[] items)
        {
            if (items.Length == 0)
            {
                return;
            }
            var idItems = items.Select(i => i.MaterialId).ToArray();
            var data = db.QuestionMaterials.Where(i => idItems.Contains(i.Id))
                .ToDictionary(i => i.Id);
            if (data.Count == 0)
            {
                return;
            }
            foreach (var item in items)
            {
                if (item.MaterialId > 0 && data.TryGetValue(item.MaterialId, out var res))
                {
                    item.Material = res;
                }
            }
        }
    }
}
