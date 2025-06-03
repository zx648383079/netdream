using NetDream.Modules.UserProfile.Entities;

namespace NetDream.Modules.UserProfile.Models
{
    public class AddressListItem : AddressEntity, IWithRegionModel
    {
        
        public RegionEntity? Region { get; set; }

        public bool IsDefault { get; set; }
    }
}
