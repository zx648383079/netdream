using NetDream.Shared.Interfaces.Entities;
using NetDream.Shared.Interfaces;

namespace NetDream.Modules.Auth.Forms
{
    public interface IContextForm
    {

        public IOperationResult<IUser> Verify(AuthContext db);
    }
}
