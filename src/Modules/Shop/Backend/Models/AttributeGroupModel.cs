
using NetDream.Modules.Shop.Entities;
using System.Linq;

namespace NetDream.Modules.Shop.Backend.Models
{
    public class AttributeGroupModel
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;

        public string[] PropertyGroups { get; set; }

        public AttributeGroupModel(AttributeGroupEntity model)
        {
            Id = model.Id;
            Name = model.Name;
            PropertyGroups = model.PropertyGroups.Split('\n')
                .Where(i => !string.IsNullOrWhiteSpace(i)).ToArray();
        }
    }
}
