using NPoco;

namespace NetDream.Web.Base.Http
{
    public interface IJsonResponse
    {
        public object Render(object data);

        public object RenderData(object data);

        public object RenderData(object data, string message);

        public object RenderPage<T>(Page<T> page);

        public object RenderFailure(string message, int code);

        public object RenderFailure(string message);
    }
}
