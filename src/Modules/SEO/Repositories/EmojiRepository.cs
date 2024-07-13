﻿using NetDream.Modules.SEO.Entities;
using NetDream.Modules.SEO.Forms;
using NetDream.Modules.SEO.Models;
using NetDream.Shared.Extensions;
using NetDream.Shared.Helpers;
using NPoco;
using System.Data;
using System.Text.RegularExpressions;

namespace NetDream.Modules.SEO.Repositories
{
    public class EmojiRepository(IDatabase db)
    {
        public const string CACHE_KEY = "emoji_tree";

        public Page<EmojiModel> GetList(string keywords = "", int category = 0, int page = 1)
        {
            var sql = new Sql();
            sql.Select("*").From<EmojiEntity>(db);
            SearchHelper.Where(sql, "name", keywords);
            if (category > 0)
            {
                sql.Where("cat_id=@0", category);
            }
            sql.OrderBy("id DESC");
            var items = db.Page<EmojiModel>(page, 20, sql);
            WithCategory(items.Items);
            return items;
        }

        private void WithCategory(IEnumerable<EmojiModel> items)
        {
            var idItems = items.Select(item => item.CatId).Where(i => i > 0).Distinct();
            if (!idItems.Any())
            {
                return;
            }
            var data = db.Fetch<EmojiCategoryEntity>($"WHERE id IN ({string.Join(',', idItems)})");
            if (data.Count == 0)
            {
                return;
            }
            foreach (var item in items)
            {
                foreach (var it in data)
                {
                    if (item.CatId == it.Id)
                    {
                        item.Category = it;
                        break;
                    }
                }
            }
        }

        public EmojiEntity? Get(int id)
        {
            return db.SingleById<EmojiEntity>(id);
        }

        public EmojiEntity Save(EmojiForm data)
        {
            var model = data.Id > 0 ? db.SingleById<EmojiEntity>(data.Id) :
                new EmojiEntity();
            model.CatId = data.CatId;
            model.Name = data.Name;
            model.Type = data.Type;
            model.Content = data.Content;
            db.TrySave(model);
            return model;
        }

        public void Remove(int id)
        {
            db.Delete<EmojiEntity>(id);
        }
        public void Remove(int[] id)
        {
            if (id.Length == 0) 
            { 
                return;
            }
            db.Delete<EmojiEntity>($"WHERE id IN ({string.Join(',', id)})");
        }

        public IList<EmojiCategoryEntity> CatList(string keywords = "")
        {
            var sql = new Sql();
            sql.Select("*").From<EmojiCategoryEntity>(db);
            SearchHelper.Where(sql, "name", keywords);
            return db.Fetch<EmojiCategoryEntity>(sql);
        }

        public EmojiCategoryEntity? GetCategory(int id)
        {
            return db.SingleById<EmojiCategoryEntity>(id);
        }

        public EmojiCategoryEntity SaveCategory(EmojiCategoryForm data)
        {
            var model = data.Id > 0 ? db.SingleById<EmojiCategoryEntity>(data.Id) :
                new EmojiCategoryEntity();
            model.Name = data.Name;
            model.Icon = data.Icon;
            db.TrySave(model);
            return model;
        }

        public void RemoveCategory(int id)
        {
            db.Delete<EmojiCategoryEntity>(id);
        }

        public int FindOrNewCategory(string name, string icon = "")
        {
            if (string.IsNullOrEmpty(name))
            {
                return db.FindScalar<int, EmojiCategoryEntity>("MIN(id) as id", string.Empty);
            }
            var id = db.FindScalar<int, EmojiCategoryEntity>("id", "name=@0", name);
            if (id > 0)
            {
                return id;
            }
            id = (int)db.Insert(new EmojiCategoryEntity()
            {
                Name = name,
                Icon = icon
            });
            return id;
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
