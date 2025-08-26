using NetDream.Modules.ResourceStore.Entities;
using NetDream.Shared.Converters;
using NetDream.Shared.Interfaces.Entities;
using NetDream.Shared.Models;
using NetDream.Shared.Providers.Entities;
using System.Collections.Generic;

namespace NetDream.Modules.ResourceStore.Models
{
    public class ResourceModel : ResourceEntity
    {

        public TagEntity[] Tags { get; set; }
        public ResourceFileEntity[] Files { get; set; }
        public IUser? User { get; internal set; }
        public ListLabelItem? Category { get; internal set; }
        public bool IsGradable { get; internal set; }

        [JsonMeta]
        public IDictionary<string, string>? MetaItems { get; set; }
     
        public CatalogItem[]? FileCatalog {  get; set; }
        public string PreviewUrl { get; internal set; }
    }
}
