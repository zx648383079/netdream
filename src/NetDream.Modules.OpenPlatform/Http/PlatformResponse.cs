using NetDream.Core.Http;
using NetDream.Modules.OpenPlatform.Entities;
using NPoco;

namespace NetDream.Modules.OpenPlatform.Http
{
    public class PlatformResponse : IJsonResponse
    {

        public PlatformEntity? Platform { get; set; }

        public object Render(object data)
        {
            return data;
        }

        public object RenderData(object data)
        {
            return Render(new {
                data,
                appid = Platform?.Appid
            });
        }

        public object RenderData(object data, string message)
        {
            return Render(new {
                data,
                message
            });
        }

        public object RenderFailure(string message, int code)
        {
            return new {
                code,
                message
            };
        }

        public object RenderFailure(string message)
        {
            return RenderFailure(message, 404);
        }

        public object RenderPage<T>(Page<T> page)
        {

            return Render(new {
                data = page.Items,
                paging = new {
                    limit = page.ItemsPerPage,
                    offset = page.CurrentPage,
                    total = page.TotalItems,
                    more = page.CurrentPage < page.TotalPages
                }
            });
        }
    }
}
