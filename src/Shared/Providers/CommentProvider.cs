using Microsoft.EntityFrameworkCore;
using NetDream.Shared.Helpers;
using NetDream.Shared.Interfaces;
using NetDream.Shared.Models;
using NetDream.Shared.Providers.Context;
using NetDream.Shared.Providers.Entities;
using NetDream.Shared.Providers.Forms;
using NetDream.Shared.Providers.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;

namespace NetDream.Shared.Providers
{
    public class CommentProvider(
        ICommentContext db, 
        IClientContext client,
        IUserRepository userStore)
    {
        public const byte LOG_TYPE_COMMENT = 6;
        public const byte LOG_ACTION_AGREE = 1;
        public const byte LOG_ACTION_DISAGREE = 2;

        private ActionLogProvider _logger => new(db, client);

        public IPage<CommentListItem> Search(CommentQueryForm form)
        {
            var (sort, order) = SearchHelper.CheckSortOrder(form.Sort, form.Order, [
                "id",
                "created_at",
                "agree_count"
            ]);
            var items = db.Comments.Search(form.Keywords, "content")
                .When(form.User > 0, i => i.UserId == form.User)
                .When(form.Target > 0, i => i.TargetId == form.Target)
                .When(form.Parent >= 0, i => i.ParentId == form.Parent)
                .OrderBy<CommentEntity, int>(sort, order)
                .ToPage(form);
            var res = new Page<CommentListItem>(items)
            {
                Items = Format(items.Items)
            };
            userStore.Include(res.Items);
            return res;
        }

        private CommentListItem[] Format(CommentEntity[] data)
        {
            if (data.Length == 0)
            {
                return [];
            }
            var idItems = data.Select(i => i.Id).ToArray();
            var count = db.Comments.Where(i => idItems.Contains(i.ParentId))
                .GroupBy(i => i.ParentId)
                .Select(i => new KeyValuePair<int, int>(i.Key, i.Count())).ToDictionary();
            var agreeLog = db.Logs.Where(i => idItems.Contains(i.ItemId)
                    && i.ItemType == LOG_TYPE_COMMENT
                    && (i.Action == LOG_ACTION_AGREE || i.Action == LOG_ACTION_DISAGREE))
                .Select(i => new KeyValuePair<int, byte>(i.ItemId, i.Action)).ToDictionary();
            return data.Select(i => new CommentListItem(i)
            {
                ReplyCount = count.TryGetValue(i.Id, out var c) ? c : 0,
                AgreeType = agreeLog.TryGetValue(i.Id, out var a) ? a : (byte)0,
            }).ToArray();
        }

        public IOperationResult Remove(int id)
        {
            db.Comments.Where(i => i.Id == id).ExecuteDelete();
            db.SaveChanges();
            return OperationResult.Ok();
        }

        public IOperationResult<CommentListItem> Insert(CommentForm data)
        {
            var model = new CommentEntity()
            {
                Content = data.Content,
                TargetId = data.Target,
                ParentId = data.Parent,
                UserId = client.UserId,
                CreatedAt = client.Now,
            };
            db.Comments.Add(model);
            db.SaveChanges();
            if (model.Id == 0)
            {
                return OperationResult<CommentListItem>.Fail("insert error");
            }
            var res = new CommentListItem(model);
            userStore.Include(res);
            return OperationResult.Ok(res);
        }

        public CommentEntity? Get(int id)
        {
            return db.Comments.Where(i => i.Id == id).Single();
        }

        public IOperationResult<CommentListItem> Save(CommentEditForm data)
        {
            var model = data.Id > 0 ? db.Comments.Where(i => i.Id == data.Id).SingleOrDefault() : 
                new CommentEntity();
            if (model is null)
            {
                return OperationResult.Fail<CommentListItem>("id is error");
            }
            model.Content = data.Content;
            model.ExtraRule = JsonSerializer.Serialize(data.ExtraRule);
            model.TargetId = data.TargetId;
            model.ParentId = data.ParentId;
            if (model.UserId == 0)
            {
                model.UserId = client.UserId;
            }
            db.Comments.Save(model, client.Now);
            db.SaveChanges();
            var res = new CommentListItem(model);
            userStore.Include(res);
            return OperationResult.Ok(res);
        }

        public void RemoveByTarget(int id)
        {
            db.Comments.Where(i => i.TargetId == id).ExecuteDelete();
        }

        public IOperationResult RemoveBySelf(int id)
        {
            if (client.UserId == 0) 
            {
                return OperationResult.Fail("无权限");
            }
            db.Comments.Where(i => i.Id == id && i.UserId == client.UserId).ExecuteDelete();
            return OperationResult.Ok();
        }

        public IOperationResult<AgreeResult> Agree(int id)
        {
            var model = Get(id);
            if (model is null)
            {
                return OperationResult<AgreeResult>.Fail("评论不存在");
            }
            var res = _logger.ToggleLog(LOG_TYPE_COMMENT,
                LOG_ACTION_AGREE, id,
                [LOG_ACTION_AGREE, LOG_ACTION_DISAGREE]);
            var data = new AgreeResult()
            {
                AgreeCount = model.AgreeCount,
                DisagreeCount = model.DisagreeCount,
            };
            if (res < 1)
            {
                data.AgreeCount--;
                data.AgreeType = 0;
            }
            else if (res == 1)
            {
                data.AgreeCount++;
                data.DisagreeCount--;
                data.AgreeType = 1;
            }
            else if (res == 2)
            {
                data.AgreeCount++;
                data.AgreeType = 1;
            }
            db.Comments.Where(i => i.Id == model.Id)
                .ExecuteUpdate(setters => setters
                .SetProperty(i => i.AgreeCount, data.AgreeCount)
                .SetProperty(i => i.DisagreeCount, data.DisagreeCount));
            return OperationResult.Ok(data);
        }

        public IOperationResult<AgreeResult> Disagree(int id)
        {
            var model = Get(id);
            if (model is null)
            {
                return OperationResult<AgreeResult>.Fail("评论不存在");
            }
            var res = _logger.ToggleLog(LOG_TYPE_COMMENT,
                LOG_ACTION_DISAGREE, id,
                [LOG_ACTION_AGREE, LOG_ACTION_DISAGREE]);
            var data = new AgreeResult()
            {
                AgreeCount = model.AgreeCount,
                DisagreeCount = model.DisagreeCount,
            };
            if (res < 1)
            {
                data.DisagreeCount--;
                data.AgreeType = 0;
            }
            else if (res == 1)
            {
                data.AgreeCount--;
                data.DisagreeCount++;
                data.AgreeType = 2;
            }
            else if (res == 2)
            {
                data.DisagreeCount++;
                data.AgreeType = 2;
            }
            db.Comments.Where(i => i.Id == model.Id)
                   .ExecuteUpdate(setters => setters
                   .SetProperty(i => i.AgreeCount, data.AgreeCount)
                   .SetProperty(i => i.DisagreeCount, data.DisagreeCount));
            return OperationResult.Ok(data);
        }

        /// <summary>
        /// 获取用户同意的类型
        /// </summary>
        /// <param name="comment"></param>
        /// <returns></returns>
        public byte GetAgreeType(int comment)
        {
            if (client.UserId == 0)
            {
                return 0;
            }
            var log = _logger.GetAction(LOG_TYPE_COMMENT, comment,
                [LOG_ACTION_AGREE, LOG_ACTION_DISAGREE]);
            return (byte)(log is null ? 0 : log);
        }
    }
}
