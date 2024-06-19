using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetDream.Modules.Bot.Repositories
{
    public class MediaRepository
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
    }
}
