using NetDream.Shared.Interfaces.Entities;
using NPoco;

namespace NetDream.Shared.Interfaces.Forms
{
    public interface ISignUpForm
    {

        public IOperationResult<IUser> Verify(IDatabase db);
    }
}
