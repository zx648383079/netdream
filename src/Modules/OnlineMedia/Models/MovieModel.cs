using NetDream.Modules.OnlineMedia.Entities;

namespace NetDream.Modules.OnlineMedia.Models
{
    public class MovieModel: MovieEntity
    {
        public string[] Tags { get; internal set; }
        public CategoryEntity? Category { get; internal set; }
        public AreaEntity? Area { get; internal set; }
        public MovieSeriesEntity[] Series { get; internal set; }
        public MovieFileEntity[] Files { get; internal set; }
    }
}
