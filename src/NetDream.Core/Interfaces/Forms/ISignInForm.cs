using NetDream.Core.Interfaces.Entities;
using NPoco;

namespace NetDream.Core.Interfaces.Forms
{
    public interface ISignInForm
    {

        public string Account { get; }
        public IOperationResult<IUser> Verify(IDatabase db);
    }
}
