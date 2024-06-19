using NPoco;

namespace NetDream.Core.Http
{
    public interface IJsonResponse
    {
        public object Render(object data);

        public object RenderData<T>(T data);

        public object RenderData<T>(T data, string message);

        public object RenderPage<T>(Page<T> page);

        public object RenderFailure(string message, int code);

        public object RenderFailure(string message);
        public object RenderFailure(object message);
    }
}
