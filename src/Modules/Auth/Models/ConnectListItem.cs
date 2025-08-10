using NetDream.Modules.Auth.Entities;

namespace NetDream.Modules.Auth.Models
{
    public class ConnectListItem
    {
        public string Vendor { get; set; }

        public string Name { get; set; }
        public string Icon { get; set; }
        public string Platform { get; set; }

        public int Id { get; set; }

        public string Nickname { get; set; }
        public int CreatedAt { get; set; }


        public ConnectListItem(string vendor, string name, string icon)
        {
            Vendor = vendor;
            Name = name;
            Icon = icon;
        }

        public ConnectListItem(ConnectListItem source, OauthEntity entity)
            : this(entity.Vendor, source.Name, source.Icon)
        {
            TrySet(entity);
        }

        public bool IsMatch(OauthEntity entity)
        {
            return entity.Vendor == Vendor;
        }

        public bool TrySet(OauthEntity entity)
        {
            if (entity.Vendor != Vendor)
            {
                return false; 
            }
            Id = entity.Id;
            Nickname = entity.Nickname;
            CreatedAt = entity.CreatedAt;
            Platform = entity.PlatformId < 1 ? "主站" : "其他";
            return true;
        }
        public bool TrySet(OauthEntity entity, int current)
        {
            if (!TrySet(entity))
            {
                return false;
            }
            if (entity.PlatformId == current)
            {
                Platform = "当前";
            }
            return true;
        }

    }
}
