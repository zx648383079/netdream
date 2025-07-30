using NetDream.Shared.Interfaces;
using NetDream.Shared.Models;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;

namespace NetDream.Shared.Helpers
{
    public class LinkRule(IDeeplink deeplink)
    {
        public static JsonSerializerOptions SerializeOptions = new()
        {
            PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower
        };

        public string Render(string content, string rules)
        {
            if (string.IsNullOrEmpty(content) || string.IsNullOrWhiteSpace(rules))
            {
                return content;
            }
            return Render(content, JsonSerializer.Deserialize<IEnumerable<LinkExtraRule>>(rules));
        }
        public string Render(string content, IEnumerable<LinkExtraRule>? rules)
        {
            if (string.IsNullOrWhiteSpace(content) || rules is null || !rules.Any())
            {
                return content;
            }
            foreach (var item in rules)
            {
                content = content.Replace(item.Word, RenderReplace(item));
            }
            return content;
        }

        protected string RenderReplace(LinkExtraRule rule)
        {
            if (rule.TryGet(LinkExtraRule.IMAGE_KEY, out var val))
            {
                return string.Format("<img src=\"{0}\" alt=\"{1}\">", val, rule.Word);
            }
            if (rule.TryGet(LinkExtraRule.FILE_KEY, out val))
            {
                return string.Format("<a href=\"{0}\" download>{1}</a>", val, rule.Word);
            }
            if (rule.User > 0)
            {
                return RenderUser(rule);
            }
            if (rule.TryGet(LinkExtraRule.LINK_KEY, out val))
            {
                return string.Format("<a href=\"{0}\">{1}</a>", deeplink.Decode(val), rule.Word);
            }
            return RenderExtra(rule);
        }

        protected virtual string RenderExtra(LinkExtraRule rule)
        {
            return string.Empty;
        }

        protected string RenderUser(LinkExtraRule rule)
        {
            return string.Format("<a href=\"{0}\">{1}</a>", rule.User, rule.Word);
        }

        public LinkExtraRule FormatRule(string word, LinkExtraRule rule)
        {
            rule.Word = word;
            return rule;
        }

        public LinkExtraRule FormatUser(string word, int user)
        {
            return LinkExtraRule.CreateUser(word, user);
        }

        public LinkExtraRule FormatImage(string word, string image)
        {
            return LinkExtraRule.CreateImage(word, image);
        }

        public LinkExtraRule FormatFile(string word, string file)
        {
            return LinkExtraRule.CreateFile(word, file);
        }

        /// <summary>
        /// 跳转到id
        /// </summary>
        /// <param name="word"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public LinkExtraRule FormatId(string word, string id)
        {
            return LinkExtraRule.CreateLink(word, "#" + id);
        }

        public LinkExtraRule FormatLink(string word, string link, IDictionary<string, object>? queries = null)
        {
            return LinkExtraRule.CreateLink(word, 
                link.Contains("://") && (queries is null || queries.Count == 0) ? 
                link : deeplink.Encode(link, queries)
            );
        }

    }
}
