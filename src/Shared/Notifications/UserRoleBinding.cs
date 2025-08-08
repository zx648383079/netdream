using MediatR;

namespace NetDream.Shared.Notifications
{
    public record UserRoleBinding(int User, int[] Roles) : IRequest
    {
    }
}
