using NetDream.Shared.Http;
using NetDream.Shared.Interfaces;
using NetDream.Shared.Securities;
using System;

namespace NetDream.Razor.Base.Http
{
    public class JsonResponse : IJsonResponse
    {
        public DateTime ClientTime { get; set; }
        public DateTime ServerTime { get; set; }
        /// <summary>
        /// 编码服务端响应
        /// </summary>
        public ISecurity Encoder { get; } = new NoneEncoder();
        /// <summary>
        /// 解码接收客户端
        /// </summary>
        public ISecurity Decoder { get; } = new NoneEncoder();
        public object Render(object data)
        {
            return data;
        }

        public object RenderData<T>(T data)
        {
            return Render(new
            {
                code = 200,
                status = "success",
                data
            });
        }

        public object RenderData<T>(T data, string message)
        {
            return Render(new
            {
                code = 200,
                status = "success",
                data,
                message
            });
        }

        public object RenderFailure(string message, int code)
        {
            return new
            {
                code,
                status = "failure",
                message
            };
        }

        public object RenderFailure(string message)
        {
            return RenderFailure(message, 404);
        }

        public object RenderFailure(object message)
        {
            return new {
                code = 404,
                status = "failure",
                message
            };
        }

        public object RenderPage<T>(IPage<T> page)
        {

            return Render(new
            {
                code = 200,
                status = "success",
                data = page.Items,
                paging = new
                {
                    limit = page.ItemsPerPage,
                    offset = page.CurrentPage,
                    total = page.TotalItems,
                    more = page.CurrentPage < page.TotalPages
                }
            });
        }
    }
}
