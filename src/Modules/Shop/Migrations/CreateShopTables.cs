using NetDream.Core.Extensions;
using NetDream.Core.Interfaces;
using NetDream.Core.Interfaces.Entities;
using NetDream.Core.Migrations;
using NetDream.Modules.Shop.Entities;
using NetDream.Modules.Shop.Repositories;
using NPoco;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetDream.Modules.Shop.Migrations
{
    public class CreateShopTables(IDatabase db, IPrivilegeManager privilege, IGlobeOption option) : Migration(db)
    {
        public override void Up()
        {
            CreateArticle();
            Append<AddressEntity>(table => {
                table.Id();
                table.String("name", 30);
                table.Uint("region_id");
                table.Uint("user_id");
                table.Char("tel", 11);
                table.String("address");
                table.Timestamps();
            }).Append<CouponEntity>(table => {
                table.Comment("优惠券");
                table.Id();
                table.String("name", 30);
                table.String("thumb").Default(string.Empty);
                table.Uint("type", 2).Default(0).Comment("优惠类型");
                table.Uint("rule", 2).Default(0).Comment("使用的商品");
                table.String("rule_value").Default(string.Empty).Comment("使用的商品");
                table.Decimal("min_money", 8, 2).Default(0).Comment("满多少可用");
                table.Decimal("money", 8, 2).Default(0).Comment("几折或优惠金额");
                table.Uint("send_type").Default(0).Comment("发放类型");
                table.Uint("send_value").Default(0).Comment("发放条件或数量");
                table.Uint("every_amount").Default(0).Comment("每个用户能领取数量");
                table.Timestamp("start_at").Comment("有效期开始时间");
                table.Timestamp("end_at").Comment("有效期结束时间");
                table.Timestamps();
            }).Append<CouponLogEntity>(table => {
                table.Comment("优惠券记录");
                table.Id();
                table.Uint("coupon_id");
                table.String("serial_number", 30).Default(string.Empty);
                table.Uint("user_id").Default(0);
                table.Uint("order_id").Default(0);
                table.Timestamp("used_at");
                table.Timestamps();
            }).Append<InvoiceEntity>(table => {
                table.Comment("开票记录");
                table.Id();
                table.Uint("title_type", 1).Default(0).Comment("发票抬头类型");
                table.Uint("type", 1).Default(0).Comment("发票类型");
                table.String("title", 100).Comment("抬头");
                table.String("tax_no", 20).Comment("税务登记号");
                table.Char("tel", 11).Comment("注册场所电话");
                table.String("bank", 100).Comment("开户银行");
                table.String("account", 60).Comment("基本开户账号");
                table.String("address").Comment("注册场所地址");
                table.Uint("user_id");
                table.Decimal("money", 10, 2).Default(0).Comment("开票金额");
                table.Uint("status", 2).Default(0).Comment("开票状态");
                table.Bool("invoice_type").Default(0).Comment("电子发票/纸质发票");
                table.String("receiver_email", 100).Default(string.Empty);
                table.String("receiver_name", 100).Default(string.Empty);
                table.String("receiver_tel", 100).Default(string.Empty);
                table.Uint("receiver_region_id").Default(0);
                table.String("receiver_region_name").Default(string.Empty);
                table.String("receiver_address").Default(string.Empty);
                table.Timestamps();
            }).Append<InvoiceTitleEntity>(table => {
                table.Comment("用户发票抬头");
                table.Id();
                table.Uint("title_type", 1).Default(0).Comment("发票抬头类型");
                table.Uint("type", 1).Default(0).Comment("发票类型");
                table.String("title", 100).Comment("抬头");
                table.String("tax_no", 20).Comment("税务登记号");
                table.Char("tel", 11).Comment("注册场所电话");
                table.String("bank", 100).Comment("开户银行");
                table.String("account", 60).Comment("基本开户账号");
                table.String("address").Comment("注册场所地址");
                table.Uint("user_id");
                table.Timestamps();
            }).Append<CertificationEntity>(table => {
                table.Comment("用户实名表");
                table.Id();
                table.Uint("user_id");
                table.String("name", 20).Comment("真实姓名");
                table.Enum("sex", ["男", "女"]).Default("男").Comment("性别");
                table.String("country", 20).Default(string.Empty).Comment("国家或地区");
                table.Uint("type", 1).Default(0).Comment("证件类型");
                table.String("card_no", 30).Comment("证件号码");
                table.String("expiry_date", 30).Default(string.Empty).Comment("证件有效期");
                table.String("profession", 30).Default(string.Empty).Comment("行业");
                table.String("address", 200).Default(string.Empty).Comment("地址");
                table.String("front_side", 200).Default(string.Empty).Comment("正面照");
                table.String("back_side", 200).Default(string.Empty).Comment("反面照");
                table.Uint("status", 2).Default(0).Comment("审核状态");
                table.Timestamps();
            }).Append<BankCardEntity>(table => {
                table.Comment("用户银行卡表");
                table.Id();
                table.Uint("user_id");
                table.String("bank", 50).Comment("银行名");
                table.Uint("type", 1).Default(0).Comment("卡类型: 0 储蓄卡 1 信用卡");
                table.String("card_no", 30).Comment("卡号码");
                table.String("expiry_date", 30).Default(string.Empty).Comment("卡有效期");
                table.Uint("status", 2).Default(0).Comment("审核状态");
                table.Timestamps();
            });
            CreateAttribute();
            CreateComment();
            CreatePlugin();
            Append<NavigationEntity>(table => {
                table.Id();
                table.String("type", 10).Default("middle");
                table.String("name", 100);
                table.String("url", 200);
                table.String("target", 10);
                table.Bool("visible").Default(1);
                table.Uint("position", 2).Default(99);
            });

            CreateGoods();
            CreateOrder();
            CreateLogistics();
            CreatePay();
            Append<RegionEntity>(table => {
                table.Id();
                table.String("name", 30);
                table.Uint("parent_id").Default(0);
                table.String("full_name", 100).Default(string.Empty);
            });
            CreateShipping();
            CreateWarehouse();
            CreateActivity();
            AutoUp();
        }



        public override void Seed()
        {
            privilege.AddRole("shop_admin", "商城管理员", new Dictionary<string, string>() {
                {"shop_manage", "商城管理"}
            });
            option.AddGroup("商城设置", () => {
                return new OptionConfigureItem[] {
                    new ("shop_open_status", "商城开启状态", "switch", "1"),
                    new ("shop_guest_buy", "开启游客购买", "switch", "0", 2),
                    new ("shop_warehouse", "开启仓库", "switch", "0"),
                    new ("shop_store", "扣库存时间", "radio", 
                    "不扣库存\n下单时\n支付时\n发货时"),
                    new ("shop_order_expire", "未支付订单过期时间/s", "text", "180"),
                };
            });
            FindOrNewById(new ArticleCategoryEntity() {
                Id = 1,
                Name = "通知中心",
            });
            FindOrNewById(new ArticleCategoryEntity() {
                Id = 2,
                Name = "帮助中心",
            });
        }

        private void FindOrNewById<T>(T data) where T : class, IIdEntity
        {
            var count = db.FindCount<T>("id=@0", data.Id);
            if (count > 0)
            {
                return;
            }
            db.Insert(data);
        }


        public void CreatePlugin()
        {
            Append<PluginEntity>(table => {
                table.Comment("插件列表及配置文件");
                table.Id();
                table.String("code", 20).Comment("插件别名");
                table.Text("setting").Comment("配置信息");
                table.Bool("status").Default(0).Comment("开始状态");
                table.Timestamps();
            }).Append<AffiliateLogEntity>(table => {
                table.Comment("用户分销记录表");
                table.Id();
                table.Uint("user_id");
                table.Uint("item_type", 1).Default(0).Comment("类型: 0 用户 1 订单");
                table.Uint("item_id").Comment("订单号/被推荐的用户");
                table.String("order_sn", 30).Default(string.Empty).Comment("订单号");
                table.Decimal("order_amount", 8, 2).Default(0).Comment("订单金额");
                table.Decimal("money", 8, 2).Default(0).Comment("佣金");
                table.Uint("status", 2).Default(0).Comment("审核状态");
                table.Timestamps();
            });
        }


        /**
         * 创建订单
         */
        public void CreateOrder()
        {
            Append<OrderEntity>(table => {
                table.Id();
                table.String("series_number", 100);
                table.Uint("user_id");
                table.Uint("status").Default(0);
                table.Uint("payment_id").Default(0);
                table.String("payment_name", 30).Default(0);
                table.Uint("shipping_id").Default(0);
                table.Uint("invoice_id").Default(0).Comment("发票");
                table.String("shipping_name", 30).Default(0);
                table.Decimal("goods_amount", 8, 2).Default(0);
                table.Decimal("order_amount", 8, 2).Default(0);
                table.Decimal("discount", 8, 2).Default(0);
                table.Decimal("shipping_fee", 8, 2).Default(0);
                table.Decimal("pay_fee", 8, 2).Default(0);
                table.Uint("reference_type", 1).Default(0).Comment("来源类型");
                table.Uint("reference_id").Default(0).Comment("来源相关id");
                table.Timestamp("pay_at").Comment("支付时间");
                table.Timestamp("shipping_at").Comment("发货时间");
                table.Timestamp("receive_at").Comment("签收时间");
                table.Timestamp("finish_at").Comment("完成时间");
                table.Timestamps();
            }).Append<OrderGoodsEntity>(table => {
                table.Id();
                table.Uint("order_id");
                table.Uint("goods_id");
                table.Uint("product_id").Default(0);
                table.Uint("user_id");
                table.String("name", 100).Comment("商品名");
                table.String("series_number", 100);
                table.String("thumb", 200).Comment("缩略图");
                table.Uint("amount").Default(1);
                table.Decimal("price", 8, 2);
                table.Decimal("discount", 8, 2).Default(0)
                    .Comment("已享受的折扣");
                table.Uint("refund_id").Default(0);
                table.Uint("status").Default(0);
                table.Uint("after_sale_status").Default(0);
                table.Uint("comment_id").Default(0).Comment("评论id");
                table.String("type_remark").Default(string.Empty).Comment("商品类型备注信息");
            }).Append<OrderLogEntity>(table => {
                table.Id();
                table.Uint("order_id");
                table.Uint("user_id");
                table.Uint("status").Default(1);
                table.String("remark").Default(string.Empty);
                table.Timestamp(MigrationTable.COLUMN_CREATED_AT);
            }).Append<OrderAddressEntity>(table => {
                table.Id();
                table.Uint("order_id");
                table.String("name", 30);
                table.Uint("region_id");
                table.String("region_name");
                table.Char("tel", 11);
                table.String("address");
                table.String("best_time").Default(string.Empty);
            }).Append<OrderCouponEntity>(table => {
                table.Id();
                table.Uint("order_id");
                table.Uint("coupon_id");
                table.String("name").Default(string.Empty);
                table.String("type").Default(string.Empty);
            }).Append<OrderActivityEntity>(table => {
                table.Id();
                table.Uint("order_id");
                table.Uint("product_id").Default(0);
                table.Uint("type");
                table.Decimal("amount", 8, 2).Default(0);
                table.String("tag").Default(string.Empty);
                table.String("name").Default(string.Empty);
            }).Append<OrderRefundEntity>(table => {
                table.Comment("订单售后服务");
                table.Id();
                table.Uint("user_id");
                table.Uint("order_id");
                table.Uint("order_goods_id").Default(0);
                table.Uint("goods_id");
                table.Uint("product_id").Default(0);
                table.String("title");
                table.Uint("amount").Default(1).Comment("数量");
                table.Uint("type", 2).Default(0);
                table.Uint("status", 2).Default(0);
                table.String("reason").Default(string.Empty);
                table.String("description").Default(string.Empty);
                table.String("evidence").Default(string.Empty).Comment("证据,json格式");
                table.String("explanation").Default(string.Empty).Comment("平台回复");
                table.Decimal("money", 10, 2).Default(0);
                table.Decimal("order_price", 10, 2).Default(0);
                table.Uint("freight", 2)
                    .Default(0).Comment("退款方式");
                table.Timestamps();
            });
        }

        /**
         * 物流发货
         */
        public void CreateLogistics()
        {
            Append<DeliveryEntity>(table => {
                table.Id();
                table.Uint("user_id");
                table.Uint("order_id");
                table.Uint("status").Default(0);
                table.Uint("shipping_id").Default(0);
                table.String("shipping_name", 30).Default(0);
                table.Decimal("shipping_fee", 8, 2).Default(0);
                table.String("logistics_number", 30).Default(string.Empty).Comment("物流单号");
                table.Text("logistics_content").Nullable().Comment("物流信息");
                table.String("name", 30);
                table.Uint("region_id");
                table.String("region_name");
                table.Char("tel", 11);
                table.String("address");
                table.String("best_time").Default(string.Empty);
                table.Timestamps();
            }).Append<DeliveryGoodsEntity>(table => {
                table.Id();
                table.Uint("delivery_id");
                table.Uint("order_goods_id");
                table.Uint("goods_id");
                table.Uint("product_id").Default(0);
                table.String("name", 100).Comment("商品名");
                table.String("thumb");
                table.String("series_number", 100);
                table.Uint("amount").Default(1);
                table.String("type_remark").Default(string.Empty).Comment("商品类型备注信息");
            });
        }

        /**
         * 创建文章
         */
        public void CreateArticle()
        {
            Append<ArticleEntity>(table => {
                table.Id();
                table.Uint("cat_id");
                table.String("title", 100).Comment("文章名");
                table.String("keywords", 200).Default(string.Empty).Comment("关键字");
                table.String("thumb", 200).Default(string.Empty).Comment("缩略图");
                table.String("description", 200).Default(string.Empty).Comment("关键字");
                table.String("brief", 200).Default(string.Empty).Comment("简介");
                table.String("url", 200).Default(string.Empty).Comment("链接");
                table.String("file", 200).Default(string.Empty).Comment("下载内容");
                table.Text("content").Comment("内容");
                table.Timestamps();
            }).Append<ArticleCategoryEntity>(table => {
                table.Id();
                table.String("name", 100).Comment("文章分类名");
                table.String("keywords", 200).Default(string.Empty).Comment("关键字");
                table.String("description", 200).Default(string.Empty).Comment("关键字");
                table.Uint("parent_id").Default(0);
                table.Uint("position", 3).Default(99);
            });
        }

        /**
         * 创建属性
         */
        public void CreateAttribute()
        {
            Append<AttributeGroupEntity>(table => {
                table.Id();
                table.String("name", 30);
                table.String("property_groups").Default(string.Empty).Comment("静态属性的可选分组，以换行符分隔");
                table.Timestamps();
            }).Append<AttributeEntity>(table => {
                table.Id();
                table.String("name", 30);
                table.Uint("group_id");
                table.String("property_group", 20).Default(string.Empty).Comment("静态属性的分组");
                table.Uint("type", 1).Default(0);
                table.Uint("search_type", 1).Default(0);
                table.Uint("input_type", 1).Default(0);
                table.String("default_value").Default(string.Empty);
                table.Uint("position", 3).Default(99);
            }).Append<GoodsAttributeEntity>(table => {
                table.Id();
                table.Uint("goods_id").Default(0);
                table.Uint("attribute_id");
                table.String("value");
                table.Decimal("price", 10, 2).Default(0).Comment("附加服务，多选不算在");
            }).Append<ProductEntity>(table => {
                table.Id();
                table.Uint("goods_id");
                table.Decimal("price", 10, 2).Default(0);
                table.Decimal("market_price", 10, 2).Default(0);
                table.Uint("stock").Default(1);
                table.Float("weight", 8, 3).Default(0);
                table.String("series_number", 50).Default(string.Empty);
                table.String("attributes", 100).Default(string.Empty);
            });
        }

        /**
         * 创建物流
         */
        public void CreateShipping()
        {
            Append<ShippingEntity>(table => {
                table.Id();
                table.String("name", 30);
                table.String("code", 30);
                table.Uint("method", 2).Default(0).Comment("计费方式");
                table.String("icon", 100).Default(string.Empty);
                table.String("description").Default(string.Empty);
                table.Uint("position", 2).Default(50);
                table.Timestamps();
            }).Append<ShippingGroupEntity>(table => {
                table.Id();
                table.Uint("shipping_id");
                table.Bool("is_all").Default(0);
                table.Float("first_step", 10, 3).Default(0);
                table.Decimal("first_fee", 10, 2).Default(0);
                table.Float("additional", 10, 3).Default(0);
                table.Decimal("additional_fee", 10, 2).Default(0);
                table.Float("free_step", 10, 3).Default(0);
            }).Append<ShippingRegionEntity>(table => {
                table.Uint("shipping_id");
                table.Uint("group_id");
                table.Uint("region_id");
            });
        }

        /**
         * 创建评论
         */
        public void CreateComment()
        {
            Append<CommentEntity>(table => {
                table.Id();
                table.Uint("user_id");
                table.Uint("item_type", 2).Default(0);
                table.Uint("item_id");
                table.String("title");
                table.String("content");
                table.Uint("rank", 2).Default(10);
                table.Timestamps();
            }).Append<CommentImageEntity>(table => {
                table.Id();
                table.Uint("comment_id");
                table.String("image");
                table.Timestamps();
            });
        }

        /**
         * 创建商品
         */
        public void CreateGoods()
        {
            Append<CategoryEntity>(table => {
                table.Id();
                table.String("name", 100).Comment("分类名");
                table.String("keywords", 200).Comment("关键字");
                table.String("description", 200).Comment("关键字");
                table.String("icon", 200);
                table.String("banner", 200);
                table.String("app_banner", 200);
                table.Uint("parent_id").Default(0);
                table.Uint("position", 3).Default(99);
            }).Append<CollectEntity>(table => {
                table.Id();
                table.Uint("user_id");
                table.Uint("goods_id");
                table.Timestamp(MigrationTable.COLUMN_CREATED_AT);
            }).Append<BrandEntity>(table => {
                table.Id();
                table.String("name", 100).Comment("分类名");
                table.String("keywords", 200).Comment("关键字");
                table.String("description", 200).Comment("关键字");
                table.String("logo", 200).Comment("LOGO");
                table.String("app_logo", 200).Comment("LOGO");
                table.String("url", 200).Comment("官网");
            }).Append<GoodsEntity>(table => {
                table.Id();
                table.Uint("cat_id");
                table.Uint("brand_id");
                table.String("name", 100).Comment("商品名");
                table.String("series_number", 100);
                table.String("keywords", 200).Comment("关键字");
                table.String("thumb", 200).Comment("缩略图");
                table.String("picture", 200).Comment("主图");
                table.String("description", 200).Comment("关键字");
                table.String("brief", 200).Comment("简介");
                table.Text("content").Nullable().Comment("内容");
                table.Decimal("price", 8, 2).Default(0).Comment("销售价");
                table.Decimal("market_price", 8, 2).Default(0).Comment("原价/市场价");
                table.Decimal("cost_price", 8, 2).Default(0).Comment("成本价");
                table.Uint("stock").Default(1);
                table.Uint("attribute_group_id").Default(0);
                table.Float("weight", 8, 3).Default(0);
                table.Uint("shipping_id").Default(0).Comment("配送方式");
                table.Uint("sales").Default(0).Comment("销量");
                table.Bool("is_best").Default(0);
                table.Bool("is_hot").Default(0);
                table.Bool("is_new").Default(0);
                table.Uint("status", 2).Default(10);
                table.String("admin_note").Default(string.Empty).Comment("管理员备注，只后台显示");
                table.Uint("type", 1).Default(0).Comment("商品类型");
                table.Uint("position", 1).Default(99).Comment("排序");
                table.Uint("dynamic_position", 1)
                    .Default(0).Comment("动态排序");
                table.SoftDeletes();
                table.Timestamps();
            }).Append<GoodsMetaEntity>(table => {
                table.Id();
                table.Uint("goods_id");
                table.String("name", 50);
                table.Text("content");
            }).Append<GoodsCardEntity>(table => {
                table.Id();
                table.Uint("goods_id");
                table.String("card_no");
                table.String("card_pwd");
                table.Uint("order_id").Default(0);
            }).Append<CartEntity>(table => {
                table.Id();
                table.Uint("type", 1).Default(0);
                table.Uint("user_id");
                table.Uint("goods_id");
                table.Uint("product_id").Default(0);
                table.Uint("amount").Default(1);
                table.Decimal("price", 8, 2);
                table.Bool("is_checked").Default(0).Comment("是否选中");
                table.Uint("selected_activity").Default(0).Comment("选择的活动");
                table.String("attribute_id").Default(string.Empty).Comment("选择的属性");
                table.String("attribute_value").Default(string.Empty).Comment("选择的属性值");
                table.Timestamp("expired_at").Comment("过期时间");
            }).Append<GoodsIssueEntity>(table => {
                table.Id();
                table.Uint("goods_id");
                table.String("question");
                table.String("answer").Default(string.Empty);
                table.Uint("ask_id");
                table.Uint("answer_id").Default(0);
                table.Uint("status", 1).Default(0).Comment("问题状态，待解决，已关闭，已删除，已置顶");
                table.Timestamps();
            }).Append<GoodsGalleryEntity>(table => {
                table.Id();
                table.Uint("goods_id");
                table.Uint("type", 1).Default(0).Comment("文件类型，图片或视频");
                table.String("thumb");
                table.String("file");
            });
        }

        protected void CreateWarehouse()
        {
            Append<WarehouseEntity>(table => {
                table.Id();
                table.String("name", 30);
                table.String("tel", 30);
                table.String("link_user", 30).Default(string.Empty);
                table.String("address").Default(string.Empty);
                table.String("longitude", 50).Comment("经度");
                table.String("latitude", 50).Comment("纬度");
                table.String("remark").Default(string.Empty);
                table.Timestamps();
            }).Append<WarehouseGoodsEntity>(table => {
                table.Id();
                table.Uint("warehouse_id");
                table.Uint("goods_id");
                table.Uint("product_id").Default(0);
                table.Uint("amount").Default(0);
            }).Append<WarehouseRegionEntity>(table => {
                table.Uint("warehouse_id");
                table.Uint("region_id");
            }).Append<WarehouseLogEntity>(table => {
                table.Id();
                table.Uint("warehouse_id");
                table.Uint("user_id");
                table.Uint("goods_id");
                table.Uint("product_id").Default(0);
                table.Uint("amount");
                table.Uint("order_id").Default(0);
                table.String("remark").Default(string.Empty);
                table.Timestamp(MigrationTable.COLUMN_CREATED_AT);
            });
        }


        protected void CreateActivity()
        {
            Append<ActivityEntity>(table => {
                table.Id();
                table.String("name", 40);
                table.String("thumb", 200).Default(string.Empty);
                table.String("description").Default(string.Empty);
                table.Uint("type", 2).Default(ActivityRepository.TYPE_AUCTION);
                table.Uint("scope_type", 1).Default(0).Comment("商品范围类型");
                table.Text("scope").Nullable().Comment("商品范围值");
                table.Text("configure").Nullable().Comment("其他配置信息");
                table.Bool("status").Default(ActivityRepository.STATUS_NONE).Comment("开启关闭");
                table.Timestamp("start_at");
                table.Timestamp("end_at");
                table.Timestamps();
            }).Append<ActivityTimeEntity>(table => {
                table.Id();
                table.String("title", 40);
                table.Time("start_at");
                table.Time("end_at");
            }).Append<SeckillGoodsEntity>(table => {
                table.Id();
                table.Uint("act_id");
                table.Uint("time_id");
                table.Uint("goods_id");
                table.Decimal("price", 8, 2).Default(0);
                table.Uint("amount", 5).Default(0);
                table.Uint("every_amount", 2).Default(0);
            }).Append<AuctionLogEntity>(table => {
                table.Id();
                table.Uint("act_id");
                table.Uint("user_id");
                table.Decimal("bid", 8, 2).Default(0)
                    .Comment("出价");
                table.Uint("amount").Default(1).Comment("出价数量");
                table.Uint("status", 1).Default(ActivityRepository.AUCTION_STATUS_NONE);
                table.Timestamp(MigrationTable.COLUMN_CREATED_AT);
            }).Append<PresaleLogEntity>(table => {
                table.Comment("预售记录");
                table.Id();
                table.Uint("act_id");
                table.Uint("user_id");
                table.Uint("order_id");
                table.Uint("order_goods_id");
                table.Decimal("order_amount", 8, 2)
                    .Default(0).Comment("预售总价");
                table.Decimal("deposit", 8, 2)
                    .Default(0).Comment("预售定金");
                table.Decimal("final_payment", 8, 2)
                    .Default(0).Comment("预售尾款");
                table.Uint("status", 2).Default(0).Comment("判断预售订单处于那个状态");
                table.Timestamp("predetermined_at").Comment("支付定金时间");
                table.Timestamp("final_at").Comment("尾款支付时间");
                table.Timestamp("ship_at").Comment("发货时间");
                table.Timestamps();
            }).Append<GroupBuyLogEntity>(table => {
                table.Comment("团购记录");
                table.Id();
                table.Uint("act_id");
                table.Uint("user_id");
                table.Uint("order_id");
                table.Uint("order_goods_id");
                table.Decimal("deposit", 8, 2)
                    .Default(0).Comment("定金");
                table.Decimal("final_payment", 8, 2)
                    .Default(0).Comment("尾款");
                table.Uint("status", 2).Default(0).Comment("判断预售订单处于那个状态");
                table.Timestamp("predetermined_at").Comment("支付定金时间");
                table.Timestamp("final_at").Comment("尾款支付时间");
                table.Timestamps();
            }).Append<BargainUserEntity>(table => {
                table.Comment("用户参与砍价");
                table.Id();
                table.Uint("act_id");
                table.Uint("user_id");
                table.Uint("goods_id");
                table.Decimal("price", 10, 2).Comment("当前价格");
                table.Uint("status", 1).Default(0);
                table.Timestamps();
            }).Append<BargainLogEntity>(table => {
                table.Comment("用户帮砍记录");
                table.Id();
                table.Uint("bargain_id");
                table.Uint("user_id");
                table.Decimal("amount", 8, 2).Default(0).Comment("砍掉的价格");
                table.Decimal("price", 8, 2).Default(0)
                    .Comment("砍掉之后剩余的价格");
                table.Timestamp(MigrationTable.COLUMN_CREATED_AT);
            }).Append<TrialLogEntity>(table => {
                table.Comment("用户申请试用记录");
                table.Id();
                table.Uint("act_id");
                table.Uint("user_id");
                table.Uint("status", 1).Default(0);
                table.Timestamps();
            }).Append<TrialReportEntity>(table => {
                table.Comment("用户试用报告");
                table.Id();
                table.Uint("act_id");
                table.Uint("user_id");
                table.Uint("goods_id");
                table.String("title");
                table.Text("content").Nullable();
                table.Uint("status", 1).Default(0);
                table.Timestamps();
            }).Append<LotteryLogEntity>(table => {
                table.Comment("用户抽奖记录");
                table.Id();
                table.Uint("act_id");
                table.Uint("user_id");
                table.Uint("item_type", 1).Default(0);
                table.Uint("item_id").Default(0);
                table.Uint("amount").Default(0);
                table.Uint("status", 1).Default(0);
                table.Timestamps();
            });
        }

        private void CreatePay()
        {
            Append<PaymentEntity>(table => {
                table.Id();
                table.String("name", 30);
                table.String("code", 30);
                table.String("icon", 100).Default(string.Empty);
                table.String("description").Default(string.Empty);
                table.Text("settings").Nullable();
            }).Append<PayLogEntity>(table => {
                table.Id().Ai(10000001);
                table.Uint("payment_id");
                table.String("payment_name", 30).Default(string.Empty);
                table.Uint("type", 1).Default(0);
                table.Uint("user_id");
                table.String("data").Default(string.Empty).Comment("可以接受多个订单号");
                table.Uint("status", 2).Default(0);
                table.Decimal("amount", 10, 2).Default(0);
                table.String("currency", 10).Default(string.Empty).Comment("货币");
                table.Decimal("currency_money", 10, 2).Default(0).Comment("货币金额");
                table.String("trade_no", 100).Default(string.Empty).Comment("外部订单号");
                table.Timestamp("begin_at").Comment("开始时间");
                table.Timestamp("confirm_at").Comment("确认支付时间");
                table.Timestamps();
            });
        }

    }
}
