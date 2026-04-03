namespace NetDream.Shared.Events.Notifications
{
    public record UserRoleBinding(int User, int[] Roles) : IRequest
    {
    }
}
