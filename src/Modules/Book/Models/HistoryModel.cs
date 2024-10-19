using NetDream.Modules.Book.Entities;
using NPoco;

namespace NetDream.Modules.Book.Models
{
    public class HistoryModel: HistoryEntity, IWithBookModel
    {
        [Ignore]
        public BookEntity? Book { get; set; }

        [Ignore]
        public ChapterEntity? Chapter { get; set; }
    }
}
