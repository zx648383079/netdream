using NetDream.Shared.Http;
using NetDream.Modules.OpenPlatform.Entities;
using NetDream.Modules.OpenPlatform.Models;
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

        public object RenderData<T>(T data)
        {
            return Render(new DataOneResponse<T>(data)
            {
                Appid = Platform?.Appid,
            });
        }

        public object RenderData<T>(T data, string message)
        {
            return Render(new DataOneResponse<T>(data)
            {
                Appid = Platform?.Appid,
                Message = message
            });
        }

        public object RenderFailure(string message, int code)
        {
            return new FailureResponse(code, message);
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

        public object RenderPage<T>(Page<T> page)
        {

            return Render(new PageResponse<T>(page));
        }
    }
}
