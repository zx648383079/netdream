namespace NetDream.Shared.Models
{
    public class LinkLabelItem
    {
        public string Title { get; set; } = string.Empty;

        public string Url { get; set; } = string.Empty;

        public LinkLabelItem()
        {
            
        }

        public LinkLabelItem(string url)
        {
            Url = url;
        }
        public LinkLabelItem(string title, string url)
        {
            Title = title;
            Url = url;
        }
    }
}
