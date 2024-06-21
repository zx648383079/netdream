using NetDream.Shared.Interfaces.Entities;
using NPoco;

namespace NetDream.Shared.Interfaces.Forms
{
    public interface ISignInForm
    {

        public string Account { get; }
        public IOperationResult<IUser> Verify(IDatabase db);
    }
}
