using NetDream.Shared.Interfaces;
using NetDream.Shared.Models;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;

namespace NetDream.Shared.Helpers
{
    public class LinkRule(IDeeplink deeplink)
    {
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
                content = content.Replace(item.S, RenderReplace(item));
            }
            return content;
        }

        protected string RenderReplace(LinkExtraRule rule)
        {
            if (!string.IsNullOrEmpty(rule.I))
            {
                return string.Format("<img src=\"{0}\" alt=\"{1}\">", rule.I, rule.S);
            }
            if (string.IsNullOrWhiteSpace(rule.F))
            {
                return string.Format("<a href=\"{0}\" download>{1}</a>", rule.F, rule.S);
            }
            if (rule.U > 0)
            {
                return RenderUser(rule);
            }
            if (string.IsNullOrWhiteSpace(rule.L))
            {
                return string.Format("<a href=\"{0}\">{1}</a>", deeplink.Decode(rule.L), rule.S);
            }
            return RenderExtra(rule);
        }

        protected virtual string RenderExtra(LinkExtraRule rule)
        {
            return string.Empty;
        }

        protected string RenderUser(LinkExtraRule rule)
        {
            return string.Format("<a href=\"{0}\">{1}</a>", rule.U, rule.S);
        }

        public LinkExtraRule FormatRule(string word, LinkExtraRule rule)
        {
            rule.S = word;
            return rule;
        }

        public LinkExtraRule FormatUser(string word, int user)
        {
            return new LinkExtraRule(word, user);
        }

        public LinkExtraRule FormatImage(string word, string image)
        {
            return new LinkExtraRule(word) { I = image };
        }

        public LinkExtraRule FormatFile(string word, string file)
        {
            return new LinkExtraRule(word) { F = file };
        }

        /// <summary>
        /// 跳转到id
        /// </summary>
        /// <param name="word"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public LinkExtraRule FormatId(string word, string id)
        {
            return new LinkExtraRule(word) { L = "#" + id };
        }

        public LinkExtraRule FormatLink(string word, string link, IDictionary<string, object>? queries = null)
        {
            return new LinkExtraRule(word) 
            {
                L = link.Contains("://") && (queries is null || queries.Count == 0) ? link : deeplink.Encode(link, queries)
            };
        }

    }
}
