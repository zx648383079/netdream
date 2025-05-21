using NetDream.Modules.Legwork.Models;
using System.Linq;

namespace NetDream.Modules.Legwork.Repositories
{
    public class ServiceRepository
    {
        public const byte STATUS_NONE = 0;
        public const byte STATUS_ALLOW = 1;
        public const byte STATUS_DISALLOW = 2;

        internal static void Include(LegworkContext db, IWithServiceModel[] items)
        {
            var idItems = items.Select(i => i.ServiceId).Distinct().ToArray();
            if (idItems.Length == 0)
            {
                return;
            }
            var data = db.Service.Where(i => idItems.Contains(i.Id))
                .Select(i => new ServiceLabelItem()
                {
                    Id = i.Id,
                    Name = i.Name,
                    Price = i.Price,
                    Thumb = i.Thumb,
                }).ToDictionary(i => i.Id);
            if (data.Count == 0)
            {
                return;
            }
            foreach ( var item in items)
            {
                if (data.TryGetValue(item.ServiceId, out var it))
                {
                    item.Service = it;
                }
            }
        }
    }
}