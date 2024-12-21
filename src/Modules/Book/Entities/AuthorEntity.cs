
namespace NetDream.Modules.Book.Entities
{
    
    public class AuthorEntity
    {
        
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Avatar { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        
        public int UserId { get; set; }
        public int Status { get; set; }
        
        public int UpdatedAt { get; set; }
        
        public int CreatedAt { get; set; }

        public AuthorEntity()
        {
            
        }

        public AuthorEntity(string name)
        {
            Name = name;
        }

        public AuthorEntity(string name, int status): this(name)
        {
            Status = status;
        }
    }
}
