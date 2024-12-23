using NetDream.Modules.SEO.Entities;
using System.Collections.Generic;

namespace NetDream.Modules.SEO.Models
{
    public class OptionTreeModel: OptionEntity
    {
        public List<OptionTreeModel> Children { get; set; } = [];

        public bool TryAdd(OptionEntity entity)
        {
            if (entity.ParentId != Id)
            {
                return false;
            }
            Children.Add(entity is OptionTreeModel o ? o : new OptionTreeModel(entity));
            return true;
        }

        public OptionTreeModel(OptionEntity entity)
        {
            Id = entity.Id;
            Name = entity.Name;
            Value = entity.Value;
            Visibility = entity.Visibility;
            Code = entity.Code;
            ParentId = entity.ParentId;
            Type = entity.Type;
            DefaultValue = entity.DefaultValue;
        }
    }
}
