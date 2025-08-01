using NetDream.Shared.Interfaces.Forms;

namespace NetDream.Shared.Models
{
    public class PaginationForm: IPaginationForm
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

    public class QueryForm: PaginationForm, IQueryForm
    {
        
        public string? Keywords { get; set; } = string.Empty;

        public string? Sort { get; set; } = string.Empty;

        public string? Order { get; set; } = string.Empty;

        public QueryForm()
        {
            
        }

        public QueryForm(QueryForm form)
            : base(form.Page, form.PerPage)
        {
            Keywords = form.Keywords;
            Sort = form.Sort;
            Order = form.Order;
        }
    } 
}
