using Microsoft.EntityFrameworkCore;
using NetDream.Modules.SEO.Entities;
using NetDream.Modules.SEO.Forms;
using NetDream.Shared.Helpers;
using NetDream.Shared.Interfaces;
using NetDream.Shared.Models;
using NetDream.Shared.Providers;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text.RegularExpressions;

namespace NetDream.Modules.SEO.Repositories
{
    public class EmojiRepository(SEOContext db)
    {
        public const string CACHE_KEY = "emoji_tree";

        public IPage<EmojiEntity> GetList(EmojiQueryForm form)
        {
            return db.Emojis.Include(i => i.Category)
                .Search(form.Keywords, "name")
                .When(form.Category > 0, i => i.CatId == form.Category)
                .OrderByDescending(i => i.Id).ToPage(form);
        }

        public IOperationResult<EmojiEntity> Get(int id)
        {
            var model = db.Emojis.Where(i => i.Id == id).SingleOrDefault();
            if (model == null)
            {
                return OperationResult.Fail<EmojiEntity>("id is error");
            }
            return OperationResult.Ok(model);
        }

        public IOperationResult<EmojiEntity> Save(EmojiForm data)
        {
            var model = data.Id > 0 ? db.Emojis.Where(i => i.Id == data.Id).Single() :
                new EmojiEntity();
            if (model is null)
            {
                return OperationResult.Fail<EmojiEntity>("id is error");
            }
            model.CatId = data.CatId;
            model.Name = data.Name;
            model.Type = data.Type;
            model.Content = data.Content;
            db.Emojis.Save(model);
            db.SaveChanges();
            return OperationResult.Ok(model);
        }

        public void Remove(int id)
        {
            db.Emojis.Where(i => i.Id == id).ExecuteDelete();
        }
        public void Remove(int[] id)
        {
            if (id.Length == 0) 
            { 
                return;
            }
            db.Emojis.Where(i => id.Contains(i.Id)).ExecuteDelete();
        }

        public EmojiCategoryEntity[] CatList(string keywords = "")
        {
            return db.EmojiCategories.Search(keywords, "name").ToArray();
        }

        public EmojiCategoryEntity? GetCategory(int id)
        {
            return db.EmojiCategories.Where(i => i.Id == id).Single();
        }

        public IOperationResult<EmojiCategoryEntity> SaveCategory(EmojiCategoryForm data)
        {
            var model = data.Id > 0 ? db.EmojiCategories.Where(i => i.Id == data.Id).SingleOrDefault() :
                new EmojiCategoryEntity();
            if (model is null)
            {
                return OperationResult<EmojiCategoryEntity>.Fail("数据错误");
            }
            model.Name = data.Name;
            model.Icon = data.Icon;
            db.EmojiCategories.Save(model);
            db.SaveChanges();
            return OperationResult.Ok(model);
        }

        public void RemoveCategory(int id)
        {
            db.EmojiCategories.Where(i => i.Id == id).ExecuteDelete();
        }

        public int FindOrNewCategory(string name, string icon = "")
        {
            if (string.IsNullOrEmpty(name))
            {
                return db.EmojiCategories.Min(i => i.Id);
            }
            var id = db.EmojiCategories.Where(i => i.Name == name).Select(i => i.Id).Single();
            if (id > 0)
            {
                return id;
            }
            var model = new EmojiCategoryEntity()
            {
                Name = name,
                Icon = icon
            };
            db.EmojiCategories.Add(model);
            db.SaveChanges();
            return model.Id;
        }

        /**
         * 生成html
         * @param string content
         * @return string
         */
        public string Render(string content)
        {
            if (string.IsNullOrWhiteSpace(content) || !content.Contains('['))
            {
                return content;
            }
            var maps = Maps();
            return Regex.Replace(content, @"\[(.+?)\]", match => {
                if (maps.TryGetValue(match.Groups[1].Value, out var res))
                {
                    return $"<img src=\"{res}\">";
                }
                return match.Value;
            });
        }

        /**
         * 提取规则
         * @param string content
         * @return array
         */
        public void RenderRule(string content)
        {
            //if (empty(content) || !str_contains(content, "["))
            //{
            //    return [];
            //}
            //if (!preg_match_all("/\[(.+?)\]/", content, matches, PREG_SET_ORDER))
            //{
            //    return [];
            //}
            //maps = static.Maps();
            //rules = [];
            //exist = [];
            //foreach (matches as match)
            //{
            //    if (in_array(match[1], exist))
            //    {
            //        continue;
            //    }
            //    if (!isset(maps[match[1]]))
            //    {
            //        continue;
            //    }
            //    rules[] = LinkRule.FormatImage(match[0], maps[match[1]]);
            //    exist[] = match[1];
            //}
            //return rules;
        }

        //public static Import(File file)
        //{
        //    zip = new ZipStream(file, \ZipArchive.RDONLY);
        //    folder = file.GetDirectory().Directory("emoji".time());
        //    folder.Create();
        //    zip.ExtractTo(folder);
        //    zip.Close();
        //    static.MapFolder(folder);
        //    folder.Delete();
        //    file.Delete();
        //    self.All(true);
        //}

        //protected static MapFolder(Directory folder)
        //{
        //    file = folder.File("map.json");
        //    if (file.Exist())
        //    {
        //        static.ImportBatch(file);
        //        return;
        //    }
        //    folder.Map((object file) {
        //        if (file instanceof Directory) {
        //            static.MapFolder(file);
        //        }
        //    });
        //}

        //protected static ImportBatch(File file)
        //{
        //    data = Json.Decode(file.Read());
        //    if (!isset(data["items"]) || empty(data["items"]))
        //    {
        //        return;
        //    }
        //    folder = "assets/upload/emoji/";
        //    category = static.FindOrNewCategory(data["name"],
        //        isset(data["icon"]) && !empty(data["icon"]) ?
        //            url().Asset(folder.data["icon"]) : string.Empty);
        //    EmojiModel.Query().Insert(array_map(use(category, folder)(object item) {
        //        type = isset(item["type"]) && item["type"] === "text" ? EmojiModel.TYPE_TEXT : EmojiModel.TYPE_IMAGE;
        //        return [
        //            "cat_id" => category,
        //        "name" => item["title"],
        //        "type" => type,
        //        "content" => type === EmojiModel.TYPE_TEXT ? item["title"]
        //            : url().Asset(folder.item["file"]),
        //    ];
        //    }, data["items"]));
        //    newFolder = public_path().Directory(folder);
        //    newFolder.Create();
        //    file.GetDirectory().Map(use(newFolder)(object item) {
        //        if (item instanceof File && item.GetExtension() !== "json") {
        //            item.Move(newFolder.File(item.GetName()));
        //        }
        //    });
        //}

        //public static All(bool refresh = false)
        //{
        //    if (refresh)
        //    {
        //        cache().Delete(self.CACHE_KEY);
        //    }
        //    return cache().GetOrSet(self.CACHE_KEY, () {
        //        return Arr.Format(EmojiCategoryModel.With("items")
        //            .OrderBy("id", "asc").Get());
        //    });
        //}

        public Dictionary<string, string> Maps()
        {
            var items = new Dictionary<string, string>();
            return items;
            //static cache;
            //if (!empty(cache))
            //{
            //    return cache;
            //}
            //cache = [];
            //data = static.All();
            //foreach (data as group)
            //{
            //    foreach (group["items"] as item)
            //    {
            //        if (item["type"] > 0)
            //        {
            //            continue;
            //        }
            //        cache[item["name"]] = item["content"];
            //    }
            //}
            // return cache;
        }
    }
}
