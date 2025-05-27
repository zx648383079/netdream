using NetDream.Modules.OnlineMedia.Entities;
using NetDream.Shared.Providers.Entities;

namespace NetDream.Modules.OnlineMedia.Models
{
    public class MovieModel: MovieEntity
    {
        public TagEntity[] Tags { get; internal set; }
        public CategoryEntity? Category { get; internal set; }
        public AreaEntity? Area { get; internal set; }
        public MovieSeriesEntity[] Series { get; internal set; }
        public MovieFileEntity[] Files { get; internal set; }
    }
}
