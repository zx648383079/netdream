using Microsoft.EntityFrameworkCore;
using NetDream.Modules.Exam.Entities;
using NetDream.Modules.Exam.Forms;
using NetDream.Modules.Exam.Models;
using NetDream.Shared.Interfaces;
using NetDream.Shared.Models;
using NetDream.Shared.Providers;
using System;
using System.Linq;

namespace NetDream.Modules.Exam.Repositories
{
    public class UpgradeRepository(ExamContext db, 
        IClientContext client,
        IUserRepository userStore)
    {
        public IPage<UpgradeEntity> GetList(QueryForm form, int course = 0)
        {
            return db.Upgrades
                .When(course > 0, i => i.CourseId == course)
                .Search(form.Keywords, "name").ToPage(form);
        }

        public IOperationResult<UpgradeEntity> Get(int id)
        {
            var model = db.Upgrades.Where(i => i.Id == id).SingleOrDefault();
            if (model == null)
            {
                return OperationResult<UpgradeEntity>.Fail("数据有误");
            }
            return OperationResult.Ok(model);
        }

        public IOperationResult<UpgradeEntity> Save(UpgradeForm data)
        {
            var model = data.Id > 0 ? db.Upgrades.Where(i => i.Id == data.Id).SingleOrDefault() :
                new UpgradeEntity();
            if (model == null)
            {
                return OperationResult<UpgradeEntity>.Fail("数据有误");
            }
            model.CourseId = data.CourseId;
            model.CourseGrade = data.CourseGrade;
            model.Name = data.Name;
            model.Description = data.Description;
            model.Icon = data.Icon;
            db.Upgrades.Save(model, client.Now);
            db.SaveChanges();
            return OperationResult.Ok(model);
        }

        public void Remove(int id)
        {
            db.Upgrades.Where(i => i.Id == id).ExecuteDelete();
            db.UpgradePaths.Where(i => i.ItemId == id && i.ItemType == 0).ExecuteDelete();
            db.UpgradeUsers.Where(i => i.UpgradeId == id).ExecuteDelete();
        }

        public IPage<UpgradeLogListItem> LogList(QueryForm form, 
            int course = 0, int upgrade = 0, int user = 0)
        {
            var res = db.UpgradeUsers
                .When(upgrade > 0, i => i.UpgradeId == upgrade)
                .When(user > 0, i => i.UserId == user)
                .ToPage<UpgradeUserEntity, UpgradeLogListItem>(form);
            userStore.Include(res.Items);
            Include(db, res.Items);
            return res;
        }

        public void LogRemove(int id)
        {
            db.UpgradeUsers.Where(i => i.Id == id).ExecuteDelete();
        }
        public static void Include(ExamContext db, IWithUpgradeModel[] items)
        {
            var idItems = items.Select(item => item.UpgradeId).Where(i => i > 0)
                .Distinct().ToArray();
            if (idItems.Length == 0)
            {
                return;
            }
            var data = db.Upgrades.Where(i => idItems.Contains(i.Id))
                .ToDictionary(i => i.Id);
            if (data.Count == 0)
            {
                return;
            }
            foreach (var item in items)
            {
                if (item.UpgradeId > 0 && data.TryGetValue(item.UpgradeId, out var res))
                {
                    item.Upgrade = res;
                }
            }
        }
    }
}
