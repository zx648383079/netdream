using NetDream.Modules.Book.Entities;

namespace NetDream.Modules.Book.Models
{
    public class AuthorModel: AuthorEntity
    {

        public int BookCount { get; set; }

        public int WordCount { get; set; }

        public int CollectCount { get; set; }

        public AuthorModel()
        {
            
        }
        public AuthorModel(AuthorEntity entity)
        {
            Id = entity.Id;
            Name = entity.Name;
            Avatar = entity.Avatar;
            Description = entity.Description;
            UserId = entity.UserId;
            CreatedAt = entity.CreatedAt;
            Status = entity.Status;
            UpdatedAt = entity.UpdatedAt;
        }
    }
}
