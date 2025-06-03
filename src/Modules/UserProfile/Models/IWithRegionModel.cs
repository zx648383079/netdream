using NetDream.Modules.UserProfile.Entities;

namespace NetDream.Modules.UserProfile.Models
{
    public interface IWithRegionModel
    {
        public int RegionId { get; }
        public RegionEntity? Region { set; }
    }
}
