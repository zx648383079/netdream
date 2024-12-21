using NetDream.Shared.Interfaces;

namespace NetDream.Shared.Http
{
    public interface IJsonResponse
    {
        public object Render(object data);

        public object RenderData<T>(T data);

        public object RenderData<T>(T data, string message);

        public object RenderPage<T>(IPage<T> page);

        public object RenderFailure(string message, int code);

        public object RenderFailure(string message);
        public object RenderFailure(object message);
    }
}
