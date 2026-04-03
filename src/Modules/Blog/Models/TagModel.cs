namespace NetDream.Modules.Blog.Models
{
    public class TagModel
    {
        public string Name { get; set; }
        public int BlogCount { get; set; }

        public string FontSize
        {
            get
            {
                return (BlogCount + 12) + "px";
            }
        }
    }
}
