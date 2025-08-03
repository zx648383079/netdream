using NetDream.Shared.Interfaces;
using System;
using System.Collections.Generic;
using System.Web;

namespace NetDream.Shared.Helpers
{
    public class Deeplink(IEnvironment environment): IDeeplink
    {
        protected string Schema() 
        {
            return environment.Deeplink;
        }

        public string Encode(string path, IDictionary<string, object>? queries = null) 
        {
            var link = Schema() + "://" + path;
            if (queries is null || queries.Count == 0) {
                return link;
            }
            var data = HttpUtility.ParseQueryString(string.Empty);
            foreach (var item in queries)
            {
                data.Add(item.Key, item.Value.ToString());
            }
            return link + "?" + data.ToString();
        }

        protected bool IsBackend(string host)
        {
            return host switch
            {
                "b" or "admin" or"backend" or "system" => true,
                _ => false,
            };
        }

        protected bool IsMember(string host)
        {
            return host switch
            {
                "u" or "user" or "member" or "space" => true,
                _ => false,
            };
        }

        public string Decode(string link) 
        {
            if (string.IsNullOrWhiteSpace(link) || link.StartsWith('#') 
                || link.StartsWith("javascript:")) {
                return link;
            }
            var data = new Uri(link);
            if (string.IsNullOrWhiteSpace(data.Scheme) || data.Scheme != Schema()) 
            {
                return link;
            }
            if (string.IsNullOrWhiteSpace(data.Host)) {
                return string.Empty;
            }
            var host = data.Host;
            if (host == "chat") {
                return "/chat";
            }
            var isBackend = IsBackend(host);
            var queries = HttpUtility.ParseQueryString(data.Query);
            var args = data.AbsolutePath.Trim('/').Split('/');
            var path = args.Length > 0 ? args[0] : string.Empty;
            if (string.IsNullOrWhiteSpace(path)) {
                return "/";
            }
            if (isBackend) {
                if (path == "user" && args.Length > 1 && Validator.IsInt(args[1])) {
                    return "/auth/admin/user/edit?id=" + args[1];
                }
                if (path == "friend_link") {
                    return "/contact/admin/friend_link";
                }
                if (path == "order" && args.Length > 1 && Validator.IsInt(args[1])) {
                    return "/shop/admin/order/info?id=" + args[1];
                }
                return string.Empty;
            }

            if (host == "micro" && Validator.IsInt(path)) {
                return "/micro?id=" + path;
            }
            if (host == "blog") {
                if (Validator.IsInt(path)) {
                    return "/blog?id=" + path;
                }
                if (path == "search") {
                    return "/blog?" + queries.ToString();
                }
            }

            return string.Empty;
        }
        
    }
}
