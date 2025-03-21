using Microsoft.AspNetCore.Mvc;
using NetDream.Api.Base.Http;
using NetDream.Modules.Blog.Forms;
using NetDream.Modules.Blog.Repositories;

namespace NetDream.Api.Controllers.Blog
{
    [Route("open/blog/[controller]")]
    [ApiController]
    public class BatchController(
        BlogRepository repository,
        CommentRepository comment
    ) : JsonController
    {
        [HttpPost]
        public IActionResult Index([FromBody] BlogBatchForm form)
        {
            var res = new BlogBatchResult();
            if (form.Categories is not null)
            {
                res.Categories = repository.Categories();
            }
            if (form.NewBlog is not null)
            {
                res.NewBlog = repository.GetNewBlogs();
            }
            if (form.NewComment is not null)
            {
                res.NewComment = comment.NewList();
            }
            if (form.Detail is not null)
            {
                res.Detail = repository.GetBlog(form.Detail.Id, form.Detail.OpenKey);
            }
            if (form.Relation is not null)
            {
                res.Relation = repository.GetRelationBlogs(form.Relation.Blog);
            }
            return Render(res);
        }
    }
}
