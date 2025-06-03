using Microsoft.EntityFrameworkCore;
using NetDream.Modules.UserProfile.Entities;
using NetDream.Modules.UserProfile.Migrations;

namespace NetDream.Modules.UserProfile
{
    public class ProfileContext(DbContextOptions<ProfileContext> options) : DbContext(options)
    {
        public DbSet<RegionEntity> Regions { get; set; }
        public DbSet<AddressEntity> Address { get; set; }
        public DbSet<BankCardEntity> BankCards { get; set; }
        public DbSet<CertificationEntity> Certifications { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new RegionEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new AddressEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new BankCardEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new CertificationEntityTypeConfiguration());
            base.OnModelCreating(modelBuilder);
        }
    }
}
