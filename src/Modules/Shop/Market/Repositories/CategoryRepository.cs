using NetDream.Modules.Shop.Entities;
using NetDream.Modules.Shop.Market.Models;
using NetDream.Modules.Shop.Models;
using NetDream.Shared.Helpers;
using NetDream.Shared.Interfaces.Entities;
using System;
using System.Linq;

namespace NetDream.Modules.Shop.Market.Repositories
{
    public class CategoryRepository(ShopContext db)
    {
        /// <summary>
        /// 根据分类id 获取当前分类的id 路径
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public int[] Path(int id)
        {
            var items = db.Categories.Select(i => new TreeListItem()
            {
                Id = i.Id,
                ParentId = i.ParentId
            }).ToArray();
            return TreeHelper.GetParent(items, id);
        }

        public CategoryFloorItem[] GetHomeFloor()
        {
            var categories_tree = Tree();
            return categories_tree.Select(item => {
                var children = TreeHelper.GetChildren(categories_tree, item.Id, true);
                return new CategoryFloorItem()
                {
                    Id = item.Id,
                    ParentId = item.ParentId,
                    Children = item.Children,
                    Level = item.Level,
                    Name = ((TreeListItem)item).Name,
                    GoodsList = GoodsRepository.AsList(db.Goods.Where(i => i.IsHot == 1 && children.Contains(i.CatId))).Take(4).ToArray(),
                    Url = "./category?id=" + item.Id
                };
            }).ToArray();
        }

        public TreeListItem[] LevelTree(int[] excludes)
        {
            var data = db.Categories.Where(i =>
                !excludes.Contains(i.Id) && !excludes.Contains(i.ParentId))
                .Select(i => new TreeListItem()
                {
                    Id = i.Id,
                    ParentId = i.ParentId,
                    Name = i.Name,
                }).ToArray();
            return TreeHelper.Sort(data);
        }

        public ITreeItem[] Tree()
        {
            var data = db.Categories.Select(i => new TreeListItem()
            {
                Id = i.Id,
                ParentId = i.ParentId,
                Name = i.Name,
            }).OrderBy(i => i.ParentId).ToArray();
            return TreeHelper.Create(data);
        }

        public CategoryEntity[] GetList(int parent = 0)
        {
            return db.Categories.Where(i => i.ParentId == parent).ToArray();
        }

    }
}
