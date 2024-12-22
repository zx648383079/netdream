using Microsoft.EntityFrameworkCore;
using NetDream.Modules.Shop.Entities;
using NetDream.Modules.Shop.Migrations;

namespace NetDream.Modules.Shop
{
    public class ShopContext(DbContextOptions<ShopContext> options): DbContext(options)
    {
        public DbSet<ActivityEntity> Activitys {get; set; }
        public DbSet<ActivityTimeEntity> ActivityTimes {get; set; }
        public DbSet<AddressEntity> Addresss {get; set; }
        public DbSet<AdEntity> Ads {get; set; }
        public DbSet<AdPositionEntity> AdPositions {get; set; }
        public DbSet<AffiliateLogEntity> AffiliateLogs {get; set; }
        public DbSet<ArticleCategoryEntity> ArticleCategorys {get; set; }
        public DbSet<ArticleEntity> Articles {get; set; }
        public DbSet<AttributeEntity> Attributes {get; set; }
        public DbSet<AttributeGroupEntity> AttributeGroups {get; set; }
        public DbSet<AuctionLogEntity> AuctionLogs {get; set; }
        public DbSet<BankCardEntity> BankCards {get; set; }
        public DbSet<BargainLogEntity> BargainLogs {get; set; }
        public DbSet<BargainUserEntity> BargainUsers {get; set; }
        public DbSet<BrandEntity> Brands {get; set; }
        public DbSet<CartEntity> Carts {get; set; }
        public DbSet<CategoryEntity> Categorys {get; set; }
        public DbSet<CertificationEntity> Certifications {get; set; }
        public DbSet<CollectEntity> Collects {get; set; }
        public DbSet<CommentEntity> Comments {get; set; }
        public DbSet<CommentImageEntity> CommentImages {get; set; }
        public DbSet<CouponEntity> Coupons {get; set; }
        public DbSet<CouponLogEntity> CouponLogs {get; set; }
        public DbSet<DeliveryEntity> Deliverys {get; set; }
        public DbSet<DeliveryGoodsEntity> DeliveryGoodss {get; set; }
        public DbSet<GoodsAttributeEntity> GoodsAttributes {get; set; }
        public DbSet<GoodsCardEntity> GoodsCards {get; set; }
        public DbSet<GoodsEntity> Goodss {get; set; }
        public DbSet<GoodsGalleryEntity> GoodsGallerys {get; set; }
        public DbSet<GoodsIssueEntity> GoodsIssues {get; set; }
        public DbSet<GoodsMetaEntity> GoodsMetas {get; set; }
        public DbSet<GroupBuyLogEntity> GroupBuyLogs {get; set; }
        public DbSet<InvoiceEntity> Invoices {get; set; }
        public DbSet<InvoiceTitleEntity> InvoiceTitles {get; set; }
        public DbSet<LotteryLogEntity> LotteryLogs {get; set; }
        public DbSet<NavigationEntity> Navigations {get; set; }
        public DbSet<OrderActivityEntity> OrderActivitys {get; set; }
        public DbSet<OrderAddressEntity> OrderAddresss {get; set; }
        public DbSet<OrderCouponEntity> OrderCoupons {get; set; }
        public DbSet<OrderEntity> Orders {get; set; }
        public DbSet<OrderGoodsEntity> OrderGoodss {get; set; }
        public DbSet<OrderLogEntity> OrderLogs {get; set; }
        public DbSet<OrderRefundEntity> OrderRefunds {get; set; }
        public DbSet<PayLogEntity> PayLogs {get; set; }
        public DbSet<PaymentEntity> Payments {get; set; }
        public DbSet<PluginEntity> Plugins {get; set; }
        public DbSet<PresaleLogEntity> PresaleLogs {get; set; }
        public DbSet<ProductEntity> Products {get; set; }
        public DbSet<RegionEntity> Regions {get; set; }
        public DbSet<SeckillGoodsEntity> SeckillGoodss {get; set; }
        public DbSet<ShippingEntity> Shippings {get; set; }
        public DbSet<ShippingGroupEntity> ShippingGroups {get; set; }
        public DbSet<ShippingRegionEntity> ShippingRegions {get; set; }
        public DbSet<TrialLogEntity> TrialLogs {get; set; }
        public DbSet<TrialReportEntity> TrialReports {get; set; }
        public DbSet<WarehouseEntity> Warehouses {get; set; }
        public DbSet<WarehouseGoodsEntity> WarehouseGoodss {get; set; }
        public DbSet<WarehouseLogEntity> WarehouseLogs {get; set; }
        public DbSet<WarehouseRegionEntity> WarehouseRegions {get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new ActivityEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new ActivityTimeEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new AddressEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new AdEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new AdPositionEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new AffiliateLogEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new ArticleCategoryEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new ArticleEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new AttributeEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new AttributeGroupEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new AuctionLogEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new BankCardEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new BargainLogEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new BargainUserEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new BrandEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new CartEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new CategoryEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new CertificationEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new CollectEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new CommentEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new CommentImageEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new CouponEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new CouponLogEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new DeliveryEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new DeliveryGoodsEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new GoodsAttributeEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new GoodsCardEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new GoodsEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new GoodsGalleryEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new GoodsIssueEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new GoodsMetaEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new GroupBuyLogEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new InvoiceEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new InvoiceTitleEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new LotteryLogEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new NavigationEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new OrderActivityEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new OrderAddressEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new OrderCouponEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new OrderEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new OrderGoodsEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new OrderLogEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new OrderRefundEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new PayLogEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new PaymentEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new PluginEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new PresaleLogEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new ProductEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new RegionEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new SeckillGoodsEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new ShippingEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new ShippingGroupEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new ShippingRegionEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new TrialLogEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new TrialReportEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new WarehouseEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new WarehouseGoodsEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new WarehouseLogEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new WarehouseRegionEntityTypeConfiguration());
            base.OnModelCreating(modelBuilder);
        }
    }
}