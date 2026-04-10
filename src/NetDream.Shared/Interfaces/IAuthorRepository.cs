using System.Collections.Generic;

namespace NetDream.Shared.Interfaces
{
    public interface IAuthorRepository
    {
        public int Count();
        public IOperationResult<IUser> Get(int id);

        public IOperationResult<IUser> From(int user);

        public IOperationResult Create(int user);

        public bool Exist(int user);

        public void Include(IEnumerable<IWithUserModel> items);
    }
}
