using NetDream.Core.Interfaces.Entities;
using NPoco;

namespace NetDream.Core.Interfaces.Forms
{
    public interface ISignUpForm
    {

        public IOperationResult<IUser> Verify(IDatabase db);
    }
}
