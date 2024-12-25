namespace NetDream.Modules.Book.Forms
{
    public class RoleForm
    {
        public int Id { get; set; }
        public int BookId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Avatar { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Character { get; set; } = string.Empty;
        public string X { get; set; } = string.Empty;
        public string Y { get; set; } = string.Empty;

        public int linkId { get; set; }

        public string linkTitle { get; set; } = string.Empty;
    }
}
