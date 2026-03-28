using MediatR;
using NetDream.Modules.Blog.Models;
using NetDream.Shared.Interfaces;
using NetDream.Shared.Interfaces.Entities;
using NetDream.Shared.Models;
using NetDream.Shared.Providers;
using NetDream.Shared.Providers.Forms;
using System;
using System.Collections.Generic;
using System.Linq;

namespace NetDream.Modules.Blog.Repositories
{
    public class CommentRepository(BlogContext db, 
        IUserRepository userStore,
        IClientContext client,
        IGlobeOption option,
        ILinkRuler ruler,
        IMediator mediator,
        ICommentRepository commentStore)
    {
        public IPage<ICommentItem> GetList(CommentQueryForm form)
        {
            return commentStore.Search(ModuleTargetType.Blog, form.Target, form);
        }

        private void WithBlog(IEnumerable<CommentListItem> items)
        {
            var idItems = items.Select(item => item.BlogId);
            if (!idItems.Any())
            {
                return;
            }
            var data = db.Blogs.Where(i => idItems.Contains(i.Id))
                .Select(i => new ListLabelItem()
                {
                    Id = i.Id,
                    Name = i.Title,
                }).ToArray();
            if (!data.Any())
            {
                return;
            }
            foreach (var item in items)
            {
                foreach (var it in data)
                {
                    if (item.BlogId == it.Id)
                    {
                        item.Blog = it;
                        break;
                    }
                }
            }
        }

        public IOperationResult Create(int target, ICommentForm form)
        {
            if (!CanComment(target))
            {
                return OperationResult.Fail("不允许评论！");
            }
            return commentStore.Create(client.UserId, ModuleTargetType.Blog, target, form);
        }

        public ICommentItem[] GetHot(int target, int limit = 4)
        {
            return commentStore.Get(ModuleTargetType.Blog, target, new QueryForm()
            {
                Sort = "hot",
                PerPage = 4,
            });
        }


        /// <summary>
        /// 前台删除，博主或发表人
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public IOperationResult RemoveSelf(int id)
        {
            return commentStore.Remove(client.UserId, ModuleTargetType.Blog, id);
        }

        public bool IsSelfBlog(int blogId)
        {
            return db.Blogs.Where(i => i.Id == blogId && i.UserId == client.UserId).Any();
        }

        public CommentListItem[] NewList()
        {
            var items = commentStore.Get(ModuleTargetType.Blog, new QueryForm()
            {
                Sort = "new",
                PerPage = 4,
            });
            WithBlog(items);
            return items;
        }

        public IOperationResult Report(int id)
        {
            return commentStore.Report(client.UserId, ModuleTargetType.Blog, id);
        }

        public IOperationResult<AgreeResult> Disagree(int id)
        {
            return commentStore.Toggle(client.UserId, ModuleTargetType.Blog, id, false);
        }

        public IOperationResult<AgreeResult> Agree(int id)
        {
            return commentStore.Toggle(client.UserId, ModuleTargetType.Blog, id, true);
        }

        public bool CanComment(int blogId)
        {
            var val = BlogCommentStatus(blogId);
            if (val == 2 && client.UserId == 0)
            {
                return false;
            }
            return val > 0;
        }

        public int CommentStatus(int status)
        {
            if (status < 1)
            {
                return 0;
            }
            var val = option.Get<int>("blog_comment");
            return val;
        }

        public int BlogCommentStatus(int blogId)
        {
            var val = db.Metas.Where(i => i.ItemId == blogId && i.Name == "comment_status")
                .Select(i => i.Content).Single();
            return CommentStatus(int.Parse(val));
        }

        /// <summary>
        /// 获取用户的最后使用信息
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        public IOperationResult<IUser> LastCommentator(string email)
        {
            return commentStore.LastCommentator(email);
        }
    }
}
