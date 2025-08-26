using NetDream.Shared.Models;

namespace NetDream.Modules.Book.Forms
{
    public class BookQueryForm : QueryForm
    {
        public int Category { get; set; }

        public bool Top { get; set; }

        public int Status { get; set; }

        public int Author { get; set; }

        public int Classify { get; set; }
    }
}
