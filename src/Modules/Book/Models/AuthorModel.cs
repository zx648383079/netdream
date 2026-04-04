using NetDream.Shared.Interfaces;

namespace NetDream.Modules.Book.Models
{
    public class AuthorModel
    {
        public IUser User { get; set; }

        public int BookCount { get; set; }

        public int WordCount { get; set; }

        public int CollectCount { get; set; }

        public AuthorModel()
        {
            
        }
        public AuthorModel(IUser entity)
        {
            User = entity;
        }
    }
}
