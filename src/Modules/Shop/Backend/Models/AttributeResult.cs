using NetDream.Modules.Shop.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetDream.Modules.Shop.Backend.Models
{
    public class AttributeResult
    {
        public AttributeFormatted[] AttrList { get; set; }

        public ProductEntity[] ProductList { get; set; }

        public class AttributeFormatted
        {
            public int Id { get; set; }
            public string Name { get; set; } = string.Empty;

            public int GroupId { get; set; }
            public int Type { get; set; }

            public int SearchType { get; set; }

            public int InputType { get; set; }

            public int Position { get; set; }
            public string PropertyGroup { get; set; } = string.Empty;

            public string[] DefaultValue { get; set; }

            public GoodsAttributeEntity[] AttrItems { get; set; } = [];

            public AttributeFormatted(AttributeEntity model)
            {
                Id = model.Id;
                Name = model.Name;
                GroupId = model.GroupId;
                Type = model.Type;
                SearchType = model.SearchType;
                InputType = model.InputType;
                Position = model.Position;
                PropertyGroup = model.PropertyGroup;
                DefaultValue = string.IsNullOrWhiteSpace(model.DefaultValue) || model.InputType < 1 ? [] : model.DefaultValue.Split('\n');
            }
        }
    }
}
