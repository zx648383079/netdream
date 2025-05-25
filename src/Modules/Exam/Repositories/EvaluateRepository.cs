using NetDream.Modules.Exam.Entities;
using NetDream.Modules.Exam.Models;
using NetDream.Shared.Interfaces;
using NetDream.Shared.Models;
using NetDream.Shared.Providers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace NetDream.Modules.Exam.Repositories
{
    public class EvaluateRepository(ExamContext db, IClientContext client)
    {
        public const byte STATUS_NONE = 0;
        public const byte STATUS_SUBMIT = 1; // 已交卷
        public const byte STATUS_SCORING = 2; // 阅卷中
        public const byte STATUS_FINISH = 3; // 已完成
        public IPage<PageEvaluateEntity> GetList(QueryForm form, int user = 0)
        {
            var pageId = Array.Empty<int>();
            if (string.IsNullOrWhiteSpace(form.Keywords))
            {
                pageId = db.Pages.Search(form.Keywords, "name").Select(i => i.Id).ToArray();
                if (pageId.Length == 0)
                {
                    return new Page<PageEvaluateEntity>();
                }
            }
            return db.PageEvaluates
                .When(user > 0, i => i.UserId == user)
                .When(pageId.Length > 0, i => pageId.Contains(i.PageId))
                .OrderByDescending(i => i.Id).ToPage(form);
        }

        public IPage<PageEvaluateEntity> SelfList(QueryForm form)
        {
            return GetList(form, client.UserId);
        }

        public IOperationResult<PageEvaluateModel> Get(int id, int user = 0)
        {
            var model = db.PageEvaluates.Where(i => i.Id == id).SingleOrDefault();
            if (model == null)
            {
                return OperationResult<PageEvaluateModel>.Fail("数据错误");
            }
            if (user > 0 && model.UserId != user)
            {
                return OperationResult<PageEvaluateModel>.Fail("数据错误");
            }
            var res = model.CopyTo<PageEvaluateModel>();
            res.Page = db.Pages.Where(i => i.Id == model.PageId).SingleOrDefault();
            res.QuestionItems = db.PageQuestions.Where(i => i.EvaluateId == model.Id).ToArray();
            return OperationResult.Ok(res);
        }

        public IOperationResult<PageEvaluateModel> GetSelf(int id)
        {
            return Get(id, client.UserId);
        }
    }
}
