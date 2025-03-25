using NetDream.Shared.Interfaces;
using System;

namespace NetDream.Shared.Http
{
    public interface IJsonResponse
    {
        public DateTime ClientTime { get; set; }
        public DateTime ServerTime { get; set; }
        /// <summary>
        /// 编码服务端响应
        /// </summary>
        public ISecurity Encoder { get; }
        /// <summary>
        /// 解码接收客户端
        /// </summary>
        public ISecurity Decoder { get; }

        public object Render(object data);

        public object RenderData<T>(T data);

        public object RenderData<T>(T data, string message);

        public object RenderPage<T>(IPage<T> page);

        public object RenderFailure(string message, int code);

        public object RenderFailure(string message);
        public object RenderFailure(object message);
    }
}
