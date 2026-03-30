using NetDream.Modules.Article.Entities;
using NetDream.Shared.Interfaces.Entities;

namespace NetDream.Modules.Article.Models
{
    public class AuthorLabelItem: IUser
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Avatar { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Url { get; set; } = string.Empty;


        public AuthorLabelItem()
        {
            
        }

        public AuthorLabelItem(AuthorEntity entity)
        {
            Id = entity.Id;
            UserId = entity.UserId;
            Name = entity.Name;
            Avatar = entity.Avatar;
            Description = entity.Description;
            Email = entity.Email;
            Url = entity.Url;
        }
    }
}
