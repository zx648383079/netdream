using NetDream.Modules.Navigation.Entities;

namespace NetDream.Modules.Navigation.Models
{
    public class CollectGroupModel
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public byte Position { get; set; }

        public CollectModel[] Items { get; set; } = [];

        public CollectGroupModel()
        {
            
        }

        public CollectGroupModel(CollectGroupEntity entity)
        {
            Id = entity.Id;
            Name = entity.Name;
            Position = entity.Position;
        }
    }
    public class CollectModel
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Link { get; set; } = string.Empty;
        public byte Position { get; set; }

        public CollectModel()
        {
            
        }

        public CollectModel(CollectEntity entity)
        {
            Id  = entity.Id;
            Name = entity.Name;
            Link = entity.Link;
            Position = entity.Position;
        }
    }
}
