namespace NetDream.Shared.Models
{
    public class PaginationForm
    {
        public int Page { get; set; } = 1;

        public int PerPage { get; set; } = 20;

        public PaginationForm()
        {
            
        }

        public PaginationForm(int page, int perPage = 20)
        {
            Page = page;
            PerPage = perPage;
        }
    }
}
