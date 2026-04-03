namespace NetDream.Shared.Events
{
    public interface IRequest
    {

    }

    public interface IRequest<out TResponse> : IRequest
    {

    }
}
