using Microsoft.EntityFrameworkCore;

namespace NetDream.Modules.Clan
{
    public class ClanContext(DbContextOptions<ClanContext> options) : DbContext(options)
    {
    }
}
