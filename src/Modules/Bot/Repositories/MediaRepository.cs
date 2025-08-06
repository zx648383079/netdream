using NetDream.Modules.Bot.Entities;
using NetDream.Modules.Bot.Forms;
using NetDream.Modules.Bot.Models;
using NetDream.Shared.Interfaces;
using NetDream.Shared.Models;
using NetDream.Shared.Providers;
using System;
using System.Linq;

namespace NetDream.Modules.Bot.Repositories
{
    public class MediaRepository(BotContext db, IClientContext client)
    {
        /**
         * 媒体素材(图片, 音频, 视频, 缩略图)
         */
        public const string TYPE_MEDIA = "media";
        /**
         * 图文素材(永久)
         */
        public const string TYPE_NEWS = "news";
        /**
         * 图片素材
         */
        public const string TYPE_IMAGE = "image";
        /**
         * 音频素材
         */
        public const string TYPE_VOICE = "voice";
        /**
         * 视频素材
         */
        public const string TYPE_VIDEO = "video";
        /**
         * 缩略图素材
         */
        public const string TYPE_THUMB = "thumb";
        /**
         * 临时素材
         */
        public const int MATERIAL_TEMPORARY = 0;
        /**
         * 永久素材
         */
        public const int MATERIAL_PERMANENT = 1;

        public const int PUBLISH_NONE = 0;
        public const int PUBLISH_DRAFT = 6;
        public const int PUBLISH_WAITING = 7;
        public const int PUBLISH_SUCCESS = 8;

        public IPage<MediaListItem> GetList(int bot_id, MediaQueryForm form)
        {
            if (!AccountRepository.IsSelf(db, client, bot_id))
            {
                return new Page<MediaListItem>();
            }
            return db.Medias.Where(i => i.BotId == bot_id)
                .When(form.Type, i => i.Type == form.Type)
                .Search(form.Keywords, "title")
                .OrderByDescending(i => i.Id)
                .ToPage(form, i => i.SelectAs());
        }

        public IPage<MediaListItem> ManageList(int bot_id, MediaQueryForm form)
        {
            return db.Medias.When(bot_id > 0, i => i.BotId == bot_id)
                .When(form.Type, i => i.Type == form.Type)
                .Search(form.Keywords, "title")
                .OrderByDescending(i => i.Id)
                .ToPage(form, i => i.SelectAs());
        }

        public IOperationResult<MediaEntity> Get(int id)
        {
            var model = db.Medias.Where(i => i.Id == id).SingleOrDefault();
            return OperationResult.OkOrFail(model, "资源不存在");
        }

        public IOperationResult<MediaEntity> GetSelf(int id)
        {
            var model = db.Medias.Where(i => i.Id == id).SingleOrDefault();
            if (model == null || !AccountRepository.IsSelf(db, client, model.BotId))
            {
                return OperationResult.Fail<MediaEntity>("数据有误");
            }
            return OperationResult.Ok(model);
        }

        public IOperationResult Remove(int id)
        {
            var model = db.Medias.Where(i => i.Id == id).SingleOrDefault();
            if (model == null || !AccountRepository.IsSelf(db, client, model.BotId))
            {
                return OperationResult.Fail<ReplyEntity>("数据有误");
            }
            if (!string.IsNullOrWhiteSpace(model.MediaId) && model.MaterialType == MATERIAL_PERMANENT)
            {
                new BotRepository().Entry(model.BotId).DeleteMedia(model.MediaId);
            }
            db.Medias.Remove(model);
            db.SaveChanges();
            return OperationResult.Ok();
        }

        public IOperationResult<MediaEntity> Save(int bot_id, MediaForm data)
        {
            if (!AccountRepository.IsSelf(db, client, bot_id))
            {
                return OperationResult.Fail<MediaEntity>("权限不足");
            }
            var model = data.Id > 0 ? db.Medias.Where(i => i.Id == data.Id && i.BotId == bot_id)
                .SingleOrDefault() : new MediaEntity();
            if (model is null)
            {
                return OperationResult.Fail<MediaEntity>("数据错误");
            }
            model.Title = data.Title;
            model.PublishStatus = data.PublishStatus;
            model.MediaId = data.MediaId;
            model.BotId = bot_id;
            model.ParentId = data.ParentId;
            model.ExpiredAt = data.ExpiredAt;
            model.MaterialType = data.MaterialType;
            model.Content = data.Content;
            model.Type = data.Type;
            model.OnlyComment = data.OnlyComment;
            model.OpenComment = data.OpenComment;
            model.Thumb = data.Thumb;
            db.Medias.Save(model, client.Now);
            db.SaveChanges();
            return OperationResult.Ok(model);
        }

        public IOperationResult Sync(int id)
        {
            var model = db.Medias.Where(i => i.Id == id)
                .SingleOrDefault();
            if (model == null || !AccountRepository.IsSelf(db, client, model.BotId))
            {
                return OperationResult.Fail("数据有误");
            }
            if (!string.IsNullOrWhiteSpace(model.MediaId) &&
                (model.MaterialType == MATERIAL_PERMANENT || model.ExpiredAt > client.Now))
            {
                return OperationResult.Fail("不能重复创建");
            }
            var adapter = new BotRepository().Entry(model.BotId);
            if (model.Type == TYPE_NEWS)
            {
                return adapter.PushNews(model);
            }
            return adapter.PushMedia(model);
        }


        /// <summary>
        /// 转化为本地路径
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public string FormatFile(string path)
        {
            // TODO
            return path;
        }


        public IPage<MediaListItem> Search(int bot_id, MediaQueryForm form, int[] idItems)
        {
            if (!AccountRepository.IsSelf(db, client, bot_id))
            {
                return new Page<MediaListItem>();
            }
            return db.Medias.Where(i => i.BotId == bot_id)
                .When(form.Type, i => i.Type == form.Type)
                .Search(form.Keywords, "title")
                .When(idItems.Length > 0, i => idItems.Contains(i.Id))
                .ToPage(form, i => i.SelectAs());
        }

        /// <summary>
        /// 拉取全部素材
        /// </summary>
        /// <param name="bot_id"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public IOperationResult Pull(int bot_id, string type = "")
        {
            var adapter = new BotRepository().Entry(bot_id);
            if (string.IsNullOrWhiteSpace(type))
            {
                return adapter.PullMedia(type); ;
            }
            adapter.PullMedia(TYPE_IMAGE);
            adapter.PullMedia(TYPE_VIDEO);
            adapter.PullMedia(TYPE_VOICE);
            return adapter.PullMedia(TYPE_NEWS);
        }
    }
}
