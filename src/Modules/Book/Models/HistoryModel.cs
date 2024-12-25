using NetDream.Modules.Book.Entities;

namespace NetDream.Modules.Book.Models
{
    public class HistoryModel: HistoryEntity, IWithBookModel
    {
        public BookEntity? Book { get; set; }

        public ChapterEntity? Chapter { get; set; }
    }
}
