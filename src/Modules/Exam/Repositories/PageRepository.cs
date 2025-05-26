using Microsoft.EntityFrameworkCore;
using NetDream.Modules.Exam.Entities;
using NetDream.Modules.Exam.Forms;
using NetDream.Modules.Exam.Models;
using NetDream.Shared.Interfaces;
using NetDream.Shared.Models;
using NetDream.Shared.Providers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;

namespace NetDream.Modules.Exam.Repositories
{
    public class PageRepository(ExamContext db, 
        IClientContext client,
        IUserRepository userStore)
    {
        public IPage<PageListItem> GetList(QueryForm form, int user = 0, int course = 0, int grade = 0)
        {
            var res = db.Pages.Search(form.Keywords, "name")
                .When(user > 0, i => i.UserId == user)
                .When(course > 0, i => i.CourseId == course)
                .When(grade > 0, i => i.CourseGrade == grade)
                .OrderByDescending(i => i.EndAt)
                .OrderByDescending(i => i.Id)
                .ToPage<PageEntity, PageListItem>(form);
            userStore.Include(res.Items);
            CourseRepository.Include(db, res.Items);
            var provider = new CourseRepository(db, client);
            foreach (var item in res.Items)
            {
                item.CourseGradeFormat = provider.FormatGrade(item.CourseId, item.CourseGrade);
            }
            return res;
        }

        public IPage<PageListItem> SelfList(QueryForm form)
        {
            return GetList(form, client.UserId);
        }

        public IOperationResult<PageModel> Get(int id, int user = 0, bool full = false)
        {
            var model = db.Pages.Where(i => i.Id == id).SingleOrDefault();
            if (model is null || (user > 0 && model.UserId != user))
            {
                return OperationResult<PageModel>.Fail("数据有误");
            }
            var res = model.CopyTo<PageModel>();
            if (model.RuleType > 0 && !string.IsNullOrWhiteSpace(model.RuleValue))
            {
                res.RuleValue = FormatQuestion(
                    JsonSerializer.Deserialize<RuleQuestion[]>(model.RuleValue),
                    true);
            }
            return OperationResult.Ok(res);
        }

        public IRuleQuestion[] FormatQuestion(RuleQuestion[] items, bool full = false)
        {
            var idItems = items.ToDictionary(i => i.Id);
            if (!full)
            {
                var res = db.Questions.Where(i => idItems.Keys.Contains(i.Id))
                    .Select(i => new QuestionListItem()
                    {
                        Id = i.Id,
                        Type = i.Type,
                        Title = i.Title,
                    })
                    .ToArray();
                foreach (var item in res)
                {
                    item.Score = idItems[item.Id].Score;
                }
                return res;
            }
            else
            {
                var data = db.Questions.Where(i => idItems.Keys.Contains(i.Id))
                    .ToArray().CopyTo<QuestionEntity, QuestionModel>();
                QuestionRepository.IncludeAnalysis(db, data);
                QuestionRepository.IncludeOption(db, data);
                QuestionRepository.IncludeAnalysis(db, data);
                var parentItems = new Dictionary<int, List<QuestionModel>>();
                var res = new List<QuestionModel>();
                foreach (var item in data)
                {
                    item.Score = idItems[item.Id].Score;
                    item.Editable = item.UserId == client.UserId;
                    if (item.ParentId < 1)
                    {
                        res.Add(item);
                        continue;
                    }
                    if (!parentItems.TryGetValue(item.ParentId, out var child))
                    {
                        child = new List<QuestionModel>();
                        parentItems.Add(item.ParentId, child);
                    }
                    child.Add(item);
                }
                if (parentItems.Count == 0)
                {
                    return data;
                }
                var sourceItems = db.Questions.Where(i => parentItems.Keys.Contains(i.Id))
                    .ToArray().CopyTo<QuestionEntity, QuestionModel>();
                QuestionRepository.IncludeAnalysis(db, sourceItems);
                QuestionRepository.IncludeAnalysis(db, sourceItems);
                foreach (var item in sourceItems)
                {
                    item.Children = [..parentItems[item.Id]];
                    res.Add(item);
                }
                return [.. res];
            }
        }

        public IOperationResult<PageModel> GetSelf(int id)
        {
            return Get(id, client.UserId, true);
        }

        public IOperationResult<PageEntity> Save(PageForm data, int user = 0)
        {
            var model = data.Id > 0 ? 
                db.Pages.Where(i => i.Id == data.Id).SingleOrDefault()
                : new PageEntity();
            if (model is null || (data.Id > 0 && user > 0 && model.UserId != user))
            {
                return OperationResult<PageEntity>.Fail("无法保存");
            }
            model.Name = data.Name;
            model.LimitTime = data.LimitTime;
            model.CourseGrade = data.CourseGrade;
            IRuleObject[] rules = data.RuleType > 0 ?
                JsonSerializer.Deserialize<RuleQuestion[]>(JsonSerializer.Serialize(data.RuleValue))
                : JsonSerializer.Deserialize<RuleRandomQuestion[]>(JsonSerializer.Serialize(data.RuleValue));
            model.RuleValue = JsonSerializer.Serialize(rules);
            model.EndAt = data.EndAt;
            model.StartAt = data.StartAt;
            model.RuleType = data.RuleType;
            model.UserId = client.UserId;
            model.Score = 0;
            model.QuestionCount = 0;
            foreach (var item in rules)
            {
                model.Score += item.Score;
                model.QuestionCount += model.RuleType < 1 ? (item as RuleRandomQuestion).Amount : 1;
            }
            db.Pages.Save(model, client.Now);
            db.SaveChanges();
            return OperationResult.Ok(model);
        }

        public IOperationResult<PageEntity> SelfSave(PageForm data)
        {
            if (data.QuestionItems?.Length > 0)
            {
                data.RuleType = 1;
                data.RuleValue = [];
            }
            var res = Save(data, client.UserId);
            if (!res.Succeeded || data.QuestionItems is null 
                || data.QuestionItems.Length == 0)
            {
                return res;
            }
            var model = res.Result;
            var idItems = new List<RuleQuestion>();
            model.QuestionCount = 0;
            model.Score = 0;
            foreach (var item in data.QuestionItems)
            {
                var qItems = SaveQuestion(item);
                foreach (var q in qItems)
                {
                    model.QuestionCount++;
                    model.Score += q.Score;
                    idItems.Add(q);
                }
            }
            model.RuleValue = JsonSerializer.Serialize(idItems);
            db.Pages.Save(model, client.Now);
            db.SaveChanges();
            return res;
        }

        private RuleQuestion[] SaveQuestion(QuestionForm data)
        {
            var qId = data.Id;
            var score = data.Score;
            var provider = new QuestionRepository(db, client, userStore);
            var r = provider.SelfSave(data, false);
            var q = r.Result;
            if (r.Succeeded)
            {
                qId = r.Result.Id;
            }
            if (qId < 1)
            {
                return [];
            }
            if (data.Type != 5)
            {
                return [ new() { Id = qId, Score = score }];
            }
            q ??= db.Questions.Where(i => i.Id == qId).SingleOrDefault();
            var res = new List<RuleQuestion>();
            foreach (var item in data.Children)
            {
                item.ParentId = qId;
                item.CourseId = q.CourseId;
                data.CourseGrade = q.CourseGrade;
                data.Easiness = q.Easiness;
                if (item.Type == 4 && string.IsNullOrWhiteSpace(item.Content))
                {
                    item.Content = item.Title;
                }
                var k = provider.SelfSave(item);
                if (k.Succeeded)
                {
                    res.Add(new()
                    {
                        Id = k.Result.Id,
                        Score = item.Score
                    });
                }
            }
            return [..res];
        }

        public IOperationResult Remove(int id, int user = 0)
        {
            var model = db.Pages.Where(i => i.Id == id)
                .When(user > 0, i => i.UserId == user).SingleOrDefault();
            if (model is null)
            {
                return OperationResult.Fail("无权限删除");
            }
            db.Pages.Remove(model);
            db.PageQuestions.Where(i => i.PageId == id).ExecuteDelete();
            db.PageEvaluates.Where(i => i.PageId == id).ExecuteDelete();
            db.SaveChanges();
            return OperationResult.Ok();
        }

        public IOperationResult SelfRemove(int id)
        {
            return Remove(id, client.UserId);
        }

        public IPage<EvaluateListItem> EvaluateList(int id, QueryForm form)
        {
            var users = Array.Empty<int>(); 
            if (!string.IsNullOrWhiteSpace(form.Keywords))
            {
                var ids = db.PageEvaluates.Where(i => i.PageId == id).Pluck(i => i.UserId);
                if (ids.Length == 0)
                {
                    return new Page<EvaluateListItem>();
                }
                users = userStore.SearchUserId(form.Keywords, ids);
                if (users.Length == 0)
                {
                    return new Page<EvaluateListItem>();
                }
            }
            var res = db.PageEvaluates
                .Where(i => i.PageId == id)
                .When(users.Length > 0, i => users.Contains(i.UserId))
                .OrderByDescending(i => i.CreatedAt)
                .ToPage<PageEvaluateEntity, EvaluateListItem>(form);
            userStore.Include(res.Items);
            return res;
        }

        public IOperationResult EvaluateRemove(int id, int user = 0)
        {
            var model = db.PageEvaluates.Where(i => i.Id == id).SingleOrDefault();
            if (model is null || (user > 0 && Can(model.PageId)))
            {
                return OperationResult.Fail("无权限删除");
            }
            db.PageEvaluates.Remove(model);
            db.PageQuestions.Where(i => i.EvaluateId == id).ExecuteDelete();
            db.SaveChanges();
            return OperationResult.Ok();
        }

        public IOperationResult SelfEvaluateRemove(int id)
        {
            return EvaluateRemove(id, client.UserId);
        }

        public bool Can(int id)
        {
            return db.Pages.Where(i => i.Id == id && i.UserId == client.UserId)
                .Any();
        }

        public ListLabelItem[] Suggestion(string keywords)
        {
            return db.Pages
                .Search(keywords, "name")
                .OrderByDescending(i => i.EndAt)
                .Take(5).Select(i => new ListLabelItem(i.Id, i.Name)).ToArray();
        }

        public IOperationResult<PageEvaluateModel> EvaluateDetail(int id)
        {
            var model = db.PageEvaluates.Where(i => i.Id == id).SingleOrDefault();
            if (model is null)
            {
                return OperationResult.Fail<PageEvaluateModel>("数据不存在");
            }
            var res = model.CopyTo<PageEvaluateModel>();
            var items = db.PageQuestions.Where(i => i.EvaluateId == id)
                .OrderBy(i => i.Id).ToArray();
            res.User = userStore.Get(model.UserId);
            res.Page = db.Pages.Where(i => i.Id == model.PageId).SingleOrDefault();
            res.Data = items.Select((i, j) => FormatQuestion(i, j + 1, model.Status > 0)).ToArray();
            return OperationResult.Ok(res);
        }

        private FormattedQuestion FormatQuestion(PageQuestionEntity model, int index, bool finished = true)
        {
            var question = db.Questions.Where(i => i.Id == model.QuestionId).SingleOrDefault();
            var res = new QuestionRepository(db, client, null).Format(question, index, model.Content, finished);
            res.Log = model;
            return res;
        }

        public IOperationResult QuestionScoring(int id, Dictionary<int, EvaluateItemForm> data)
        {
            var model = db.PageEvaluates.Where(i => i.Id == id).SingleOrDefault();
            if (model is null)
            {
                return OperationResult.Fail("数据不存在");
            }
            if (model.Status == EvaluateRepository.STATUS_FINISH)
            {
                return OperationResult.Fail("无法更改试卷");
            }
            model.MarkerId = client.UserId;
            model.Status = EvaluateRepository.STATUS_SCORING;
            foreach (var item in data)
            {
                if (item.Value.Id == 0)
                {
                    item.Value.Id = item.Key;
                }
                db.PageQuestions.Where(i => i.EvaluateId == model.Id && i.QuestionId == item.Value.Id)
                    .ExecuteUpdate(setters => setters.SetProperty(i => i.Score, item.Value.Score)
                    .SetProperty(i => i.Remark, item.Value.Remark));
            }
            db.PageEvaluates.Save(model, client.Now);
            db.SaveChanges();
            return OperationResult.Ok();
        }

        public IOperationResult Scoring(int id, string remark = "")
        {
            var model = db.PageEvaluates.Where(i => i.Id == id).SingleOrDefault();
            if (model is null)
            {
                return OperationResult.Fail("数据不存在");
            }
            if (!string.IsNullOrWhiteSpace(remark))
            {
                model.Remark = remark;
            }
            model.MarkerId = client.UserId;
            model.Status = EvaluateRepository.STATUS_FINISH;
            model.Score = db.PageQuestions.Where(i => i.EvaluateId == model.Id)
                .Sum(i => i.Score);
            db.PageEvaluates.Save(model, client.Now);
            db.SaveChanges();
            return OperationResult.Ok();
        }
    }
}
