using NetDream.Modules.Book.Entities;
using NetDream.Shared.Interfaces;

namespace NetDream.Modules.Book.Models
{
    public class ListModel: ListEntity, IWithUserModel
    {

        public IUser? User { get; set; }

        public bool IsCollected { get; set; }
    }
}
