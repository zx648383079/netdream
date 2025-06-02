using NetDream.Shared.Interfaces.Entities;
using NetDream.Shared.Interfaces;
using NetDream.Modules.Auth.Repositories;

namespace NetDream.Modules.Auth.Forms
{
    public interface IContextForm
    {

        public IOperationResult<IUserProfile> Verify(IContextRepository db);
    }

    
}
