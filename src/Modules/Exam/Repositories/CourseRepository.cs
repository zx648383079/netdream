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

namespace NetDream.Modules.Exam.Repositories
{
    public class CourseRepository(ExamContext db, IClientContext client)
    {
        public IPage<CourseEntity> GetList(QueryForm form, int parent = 0)
        {
            return db.Courses
                .Where(i => i.ParentId == parent)
                .Search(form.Keywords, "name").ToPage(form);
        }

        public IOperationResult<CourseEntity> Get(int id)
        {
            var model = db.Courses.Where(i => i.Id == id).SingleOrDefault();
            if (model is null)
            {
                return OperationResult<CourseEntity>.Fail("数据有误");
            }
            return OperationResult.Ok(model);
        }

        public IOperationResult<CourseEntity> Save(CourseForm data)
        {
            var model = data.Id > 0 ? 
                db.Courses.Where(i => i.Id == data.Id).SingleOrDefault()
                : new CourseEntity();
            if (model is null)
            {
                return OperationResult<CourseEntity>.Fail("数据有误");
            }
            model.Name = data.Name;
            model.Thumb = data.Thumb;
            model.Description = data.Description;
            model.ParentId = data.ParentId;
            db.Courses.Save(model, client.Now);
            db.SaveChanges();
            return OperationResult.Ok(model);
        }

        public void Remove(int id)
        {
            db.Courses.Where(i => i.Id == id).ExecuteDelete();
            db.SaveChanges();
        }

        public CourseListItem[] Children(int id)
        {
            var items = db.Courses.Where(i => i.ParentId == id).ToArray().CopyTo<CourseEntity, CourseListItem>();
            foreach (var item in items)
            {
                item.Children = db.Courses.Where(i => i.ParentId == item.Id).ToArray();
            }
            return items;
        }

        public CourseTreeItem[] All(bool full = false)
        {
            var data = db.Courses.ToArray()
                .CopyTo<CourseEntity, CourseTreeItem>();
            if (full)
            {
                var items = db.Questions.GroupBy(i => i.CourseId)
                    .Select(i => new KeyValuePair<int, int>(i.Key, i.Count()))
                    .ToDictionary();
                foreach (var item in data)
                {
                    if (items.TryGetValue(item.Id, out var c))
                    {
                        item.QuestionCount = c;
                    }
                }
            }
            return TreeHelper.Sort(data);
        }

        public IPage<CourseEntity> Search(QueryForm form, int[] idItems)
        {
            return db.Courses.Search(form.Keywords, "name")
                .Where(i => idItems.Contains(i.Id))
                .ToPage(form);
        }

        public OptionItem<int>[] GradeAll(int course)
        {
            var items = db.CourseGrades.When(course > 0, i => i.CourseId == course || i.CourseId == 0,
                i => i.CourseId == 0)
                .OrderBy(i => i.CourseId)
                .OrderBy(i => i.Grade).ToArray();
            var data = new List<OptionItem<int>>();
            var exist = new HashSet<int>();
            foreach (var item in items)
            {
                if (exist.Contains(item.Grade))
                {
                    continue;
                }
                data.Add(new OptionItem<int>(item.Name, item.Grade));
                exist.Add(item.Grade);
            }
            return data.OrderBy(i => i.Value).ToArray();
        }

        public IPage<GradeListItem> GradeList(QueryForm form, int course = 0)
        {
            var res = db.CourseGrades
                .When(course > 0, i => i.CourseId == course)
                .Search(form.Keywords, "name").ToPage<CourseGradeEntity, GradeListItem>(form);
            Include(db, res.Items);
            return res;
        }

        public IOperationResult<CourseGradeEntity> GradeSave(GradeForm data)
        {
            CourseGradeEntity? model;
            if (data.Id > 0)
            {
                model = db.CourseGrades.Where(i => i.Id == data.Id).SingleOrDefault();
            }
            else
            {
                model = db.CourseGrades.Where(i => i.CourseId == data.CourseId && i.Grade == data.Grade)
                    .FirstOrDefault();
            }
            model ??= new CourseGradeEntity();
            model.Name = data.Name;
            model.CourseId = data.CourseId;
            model.Grade = data.Grade;
            db.CourseGrades.Save(model);
            db.SaveChanges();
            return OperationResult.Ok(model);
        }

        public void GradeRemove(int id)
        {
            db.CourseGrades.Where(i => i.Id == id).ExecuteDelete();
            db.SaveChanges();
        }

        public string FormatGrade(int course, int grade)
        {
            if (grade < 1)
            {
                return string.Empty;
            }
            return db.CourseGrades.Where(i => i.Grade == grade)
                .When(course > 0, i => i.CourseId == course || i.CourseId == 0, 
                i => i.CourseId == 0).OrderByDescending(i => i.CourseId)
                .Select(i => i.Name).FirstOrDefault() ?? string.Empty;
        }

        public static void Include(ExamContext db, IWithCourseModel[] items)
        {
            var idItems = items.Select(item => item.CourseId).Where(i => i > 0)
                .Distinct().ToArray();
            if (idItems.Length == 0)
            {
                return;
            }
            var data = db.Courses.Where(i => idItems.Contains(i.Id))
                .Select(i => new ListLabelItem(i.Id, i.Name))
                .ToDictionary(i => i.Id);
            if (data.Count == 0)
            {
                return;
            }
            foreach (var item in items)
            {
                if (item.CourseId > 0 && data.TryGetValue(item.CourseId, out var res))
                {
                    item.Course = res;
                }
            }
        }
    }
}
