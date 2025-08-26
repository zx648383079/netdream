using NetDream.Modules.Catering.Entities;
using NetDream.Shared.Converters;
using System.Collections.Generic;

namespace NetDream.Modules.Catering.Models
{
    public class StoreModel : StoreEntity
    {
        [JsonMeta]
        public IDictionary<string, string>? MetaItems { get; set; }
    }
}
