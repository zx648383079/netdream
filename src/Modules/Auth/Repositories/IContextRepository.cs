using NetDream.Modules.UserAccount.Entities;
using NetDream.Shared.Interfaces;
using NetDream.Shared.Interfaces.Entities;
using System;
using System.Linq.Expressions;

namespace NetDream.Modules.Auth.Repositories
{
    public interface IContextRepository
    {
        public IOperationResult<IUserProfile> Create(UserEntity user, string inviteCode = "");
        public bool Exists(Expression<Func<UserEntity, bool>> where);
        public IOperationResult<IUserProfile> Find(Expression<Func<UserEntity, bool>> where);
        public IOperationResult<IUserProfile> Find(Expression<Func<UserEntity, bool>> where, string password);
        public bool VerifyCode(string target, string code);
    }
}
