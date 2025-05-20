using Microsoft.EntityFrameworkCore;
using NetDream.Modules.Catering.Entities;
using NetDream.Modules.Catering.Migrations;

namespace NetDream.Modules.Catering;
public class CateringContext(DbContextOptions<CateringContext> options) : DbContext(options)
{
    public DbSet<AddressEntity> Address { get; set; }
    public DbSet<CartEntity> Cart { get; set; }
    public DbSet<CategoryEntity> Category { get; set; }
    public DbSet<GoodsEntity> Goods { get; set; }
    public DbSet<GoodsGalleryEntity> GoodsGallery { get; set; }
    public DbSet<MaterialEntity> Material { get; set; }
    public DbSet<MaterialPriceEntity> MaterialPrice { get; set; }
    public DbSet<MaterialUnitEntity> MaterialUnit { get; set; }
    public DbSet<OrderEntity> Order { get; set; }
    public DbSet<OrderGoodsEntity> OrderGoods { get; set; }
    public DbSet<PurchaseOrderEntity> PurchaseOrder { get; set; }
    public DbSet<PurchaseOrderGoodsEntity> PurchaseOrderGoods { get; set; }
    public DbSet<RecipeEntity> Recipe { get; set; }
    public DbSet<RecipeMaterialEntity> RecipeMaterial { get; set; }
    public DbSet<StoreEntity> Store { get; set; }
    public DbSet<StoreFloorEntity> StoreFloor { get; set; }
    public DbSet<StoreMetaEntity> StoreMeta { get; set; }
    public DbSet<StorePatronEntity> StorePatron { get; set; }
    public DbSet<StorePatronGroupEntity> StorePatronGroup { get; set; }
    public DbSet<StorePlaceEntity> StorePlace { get; set; }
    public DbSet<StoreRoleEntity> StoreRole { get; set; }
    public DbSet<StoreStaffEntity> StoreStaff { get; set; }
    public DbSet<StoreStockEntity> StoreStock { get; set; }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new AddressEntityTypeConfiguration());
        modelBuilder.ApplyConfiguration(new CartEntityTypeConfiguration());
        modelBuilder.ApplyConfiguration(new CategoryEntityTypeConfiguration());
        modelBuilder.ApplyConfiguration(new GoodsEntityTypeConfiguration());
        modelBuilder.ApplyConfiguration(new GoodsGalleryEntityTypeConfiguration());
        modelBuilder.ApplyConfiguration(new MaterialEntityTypeConfiguration());
        modelBuilder.ApplyConfiguration(new MaterialPriceEntityTypeConfiguration());
        modelBuilder.ApplyConfiguration(new MaterialUnitEntityTypeConfiguration());
        modelBuilder.ApplyConfiguration(new OrderEntityTypeConfiguration());
        modelBuilder.ApplyConfiguration(new OrderGoodsEntityTypeConfiguration());
        modelBuilder.ApplyConfiguration(new PurchaseOrderEntityTypeConfiguration());
        modelBuilder.ApplyConfiguration(new PurchaseOrderGoodsEntityTypeConfiguration());
        modelBuilder.ApplyConfiguration(new RecipeEntityTypeConfiguration());
        modelBuilder.ApplyConfiguration(new RecipeMaterialEntityTypeConfiguration());
        modelBuilder.ApplyConfiguration(new StoreEntityTypeConfiguration());
        modelBuilder.ApplyConfiguration(new StoreFloorEntityTypeConfiguration());
        modelBuilder.ApplyConfiguration(new StoreMetaEntityTypeConfiguration());
        modelBuilder.ApplyConfiguration(new StorePatronEntityTypeConfiguration());
        modelBuilder.ApplyConfiguration(new StorePatronGroupEntityTypeConfiguration());
        modelBuilder.ApplyConfiguration(new StorePlaceEntityTypeConfiguration());
        modelBuilder.ApplyConfiguration(new StoreRoleEntityTypeConfiguration());
        modelBuilder.ApplyConfiguration(new StoreStaffEntityTypeConfiguration());
        modelBuilder.ApplyConfiguration(new StoreStockEntityTypeConfiguration());
        base.OnModelCreating(modelBuilder);
    }
}