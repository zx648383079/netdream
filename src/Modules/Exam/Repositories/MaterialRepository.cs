using Microsoft.EntityFrameworkCore;
using NetDream.Modules.Exam.Entities;
using NetDream.Modules.Exam.Forms;
using NetDream.Modules.Exam.Models;
using NetDream.Shared.Interfaces;
using NetDream.Shared.Models;
using NetDream.Shared.Providers;
using System.Linq;

namespace NetDream.Modules.Exam.Repositories
{
    public class MaterialRepository(ExamContext db, IClientContext client)
    {
        public IPage<MaterialListItem> GetList(QueryForm form, 
            int course = 0, bool full = false)
        {
            var res = db.QuestionMaterials
                .When(course > 0, i => i.CourseId == course)
                .Search(form.Keywords, "title").ToPage<QuestionMaterialEntity, MaterialListItem>(form);
            if (full)
            {
                // TODO
            }
            return res;
        }

        public IOperationResult<QuestionMaterialEntity> Get(int id)
        {
            var model = db.QuestionMaterials.Where(i => i.Id == id).SingleOrDefault();
            if (model == null)
            {
                return OperationResult<QuestionMaterialEntity>.Fail("数据有误");
            }
            return OperationResult.Ok(model);
        }

        public IOperationResult<QuestionMaterialEntity> Save(MaterialForm data)
        {
            var model = data.Id > 0 ? db.QuestionMaterials.Where(i => i.Id == data.Id).SingleOrDefault() 
                : new QuestionMaterialEntity();
            if (model == null)
            {
                return OperationResult<QuestionMaterialEntity>.Fail("数据有误");
            }
            model.CourseId = data.CourseId;
            model.Title = data.Title;
            model.Description = data.Description;
            model.Type = data.Type;
            model.Content = data.Content;
            db.QuestionMaterials.Save(model);
            db.SaveChanges();
            return OperationResult.Ok(model);
        }

        public void Remove(int id)
        {
            db.QuestionMaterials.Where(i => i.Id == id).ExecuteDelete();
        }

    }
}
