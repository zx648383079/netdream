using NetDream.Shared.Interfaces;

namespace NetDream.Modules.Article.Entities
{
    public class AuthorEntity: IIdEntity, ITimestampEntity
    {

        public int Id { get; set; }
        public int UserId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Avatar { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Url { get; set; } = string.Empty;

        public byte Status { get; set; }

        public int UpdatedAt { get; set; }

        public int CreatedAt { get; set; }
    }
}
