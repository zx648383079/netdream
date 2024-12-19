using NPoco;

namespace NetDream.Modules.OpenPlatform.Models
{
    public class PageResponse<T>: DataResponse<T>
    {
        public PagePaging Paging { get; set; }

        public PageResponse(Page<T> page): base(page.Items)
        {
            Paging = new()
            {
                Limit = page.ItemsPerPage,
                Offset = page.CurrentPage,
                Total = page.TotalItems,
                More = page.CurrentPage < page.TotalPages
            };
        }
    }

    public class PagePaging
    {
        /// <summary>
        /// perPage
        /// </summary>
        public long Limit { get; set; }
        /// <summary>
        /// page
        /// </summary>
        public long Offset { get; set; }

        public long Total { get; set; }

        public bool More { get; set; }
    }
}
