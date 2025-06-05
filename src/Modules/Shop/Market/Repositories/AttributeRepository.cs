using NetDream.Modules.Shop.Entities;
using NetDream.Shared.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace NetDream.Modules.Shop.Market.Repositories
{
    public class AttributeRepository(ShopContext db)
    {
        /**
     * 根据属性选择值获取货品和附加属性
     * @param int[] properties
     * @param int goods
     * @return array{product_properties: array, properties: array, properties_price: float, properties_label: string, total_properties_price: float, product: ProductModel|null}
     *     properties_price 未排除货品属性之后的价格
     *     total_properties_price 全部选中属性的价格
     */
    public static array GetProductAndPriceWithProperties(array properties,int goods) {
        return CartRepository.Cache().GetOrSet(implode(",", properties), goods,
            use (properties, goods) () {
                list(product_properties, properties, properties_price, properties_label, total_properties_price) = self.SplitProperties(properties);
                product = empty(product_properties) ? null :
                    self.GetProduct(product_properties, goods);
                return compact("product_properties", "properties", "properties_price", "product", "properties_label", "total_properties_price");
        });
    }

    /**
     * 根据属性值获取货品
     * @param array properties
     * @param int goods
     * @return ProductModel|null
     */
    private static  GetProduct(array properties,int goods) {
        sort(properties);
        attributes = implode(ProductModel.ATTRIBUTE_LINK, properties);
        return ProductModel.Where("attributes", attributes).Where("goods_id", goods).First();
    }

    public static array FormatPostProperties(object properties) {
        data = [];
        if (empty(properties)) {
            return data;
        }
        if (!is_array(properties)) {
            properties = Json.Decode(properties);
        }
        foreach ((array)properties as item) {
            args = explode(":", item);
            value = intval(count(args) > 1 ? args[1] : args[0]);
            if (value < 1 || in_array(value, data)) {
                continue;
            }
            data[] = value;
        }
        sort(data);
        return data;
    }

    /**
     * 分离商品规格和附加属性
     * @param int[] properties
     * @return array [product: [], properties: [], properties_price: 0, properties_label: string.Empty, total_properties_price]
     */
    public static  SplitProperties(array properties) {
        if (empty(properties)) {
            return [[], [], 0, string.Empty, 0];
        }
        items = GoodsAttributeEntity.WhereIn("id", properties).Get("attribute_id", "id", "price", "value");
        if (empty(data)) {
            return [[], [], 0, string.Empty, 0];
        }

        attrId = [];
        data = [];
        foreach (items as item) {
            attrId[] = item["attribute_id"];
            if (!isset(data[item["attribute_id"]])) {
                data[item["attribute_id"]] = [
                    "price" => 0,
                    "items" => [],
                    "label" => []
                ];
            }
            data[item["attribute_id"]]["price"] += item["price"];
            data[item["attribute_id"]]["items"][] = item["id"];
            data[item["attribute_id"]]["label"][] = item["value"];
        }
        properties_price = 0;
        total_properties_price = 0;
        properties_label = [];
        attr_list = AttributeEntity.WhereIn("id", attrId).Get("id", "type", "name");
        properties = product_properties = [];
        foreach (attr_list as item) {
            group = data[item["id"]];
            total_properties_price += group["price"];
            properties_label[] = sprintf("[%s]:%s", item["name"], implode(",", group["label"]));
            if (item.Type == 2) {
                properties = array_merge(properties, group["items"]);
                properties_price += group["price"];
                continue;
            }
            product_properties = array_merge(product_properties, group["items"]);
        }
        return [product_properties, properties, properties_price, implode(";", properties_label), total_properties_price];
    }

    public static array GetProperties(int group,int goods) {
        if (group < 1) {
            return [];
        }
        attr_list = AttributeModel.Where("group_id", group)
            .Where("type", ">", 0).OrderBy("position asc").OrderBy("type asc")
            .Get("id", "name", "type");
        if (empty(attr_list)) {
            return [];
        }
        return array_filter(Relation.Create(attr_list, [
            "attr_items" => [
                "query" => GoodsAttributeModel.Where("goods_id", goods),
                "link" => ["id", "attribute_id"],
            ]
        ]),  (object item) {
            return empty(item.AttrItems);
        });
    }

    public static  GetStaticProperties(int group,int goods) {
        if (group < 1) {
            return [];
        }
        attr_list = AttributeUniqueModel.Where("group_id", group)
            .Where("type", 0).OrderBy("position asc").OrderBy("type asc")
            .Get("id", "name", "property_group");
        if (empty(attr_list)) {
            return [];
        }
        items = Relation.Create(attr_list, [
            "attr_item" => [
                "query" => GoodsAttributeModel.Where("goods_id", goods),
                "link" => ["id", "attribute_id"],
                "type" => Relation.TYPE_ONE
            ]
        ]);
        data = [];
        foreach (items as item) {
            if (empty(item["attr_item"])) {
                continue;
            }
            groupName = trim(item["property_group"]);
            if (!isset(data[groupName])) {
                data[groupName] = [
                    "name" => groupName,
                    "items" => []
                ];
            }
            data[groupName]["items"][] = item;
        }
        return array_values(data);
    }

    /**
     * 一次性获取属性及静态属性
     * @param int group
     * @param int goods
     * @return array{properties: array, static_properties: array}
     * @throws \Exception
     */
    public static array BatchProperties(int group,int goods) {
        properties = [];
        static_properties = [];
        if (group < 1) {
            return compact("properties", "static_properties");
        }
        attr_list = AttributeEntity.Where("group_id", group)
            .OrderBy("position asc").OrderBy("type asc")
            .Get("id", "name", "property_group", "type");
        if (empty(attr_list)) {
            return compact("properties", "static_properties");
        }
        attrId = [];
        foreach (attr_list as item) {
            attrId[] = item["id"];
            if (item["type"] > 0) {
                properties[item["id"]] = [
                    "id" => item["id"],
                    "name" => item["name"],
                    "type" => intval(item["type"]),
                    "attr_items" => []
                ];
                continue;
            }
            groupName = trim(item["property_group"]);
            if (!isset(static_properties[groupName])) {
                static_properties[groupName] = [
                    "name" => groupName,
                    "items" => []
                ];
            }
            static_properties[groupName]["items"][item["id"]] = [
                "id" => item["id"],
                "name" => item["name"],
                "group" => groupName,
                "attr_item" => null,
            ];
        }
        items = GoodsAttributeModel.Where("goods_id", goods)
            .WhereIn("attribute_id", attrId).Get();
        foreach (items as item) {
            attrId = intval(item["attribute_id"]);
            if (isset(properties[attrId])) {
                properties[item["attribute_id"]]["attr_items"][] = item;
                continue;
            }
            foreach (static_properties as n => group) {
                if (isset(group["items"][attrId])) {
                    static_properties[n]["items"][attrId]["attr_item"] = item;
                }
            }
        }
        items = [];
        foreach (properties as item) {
            if (!empty(item["attr_items"])) {
                items[] = item;
            }
        }
        properties = items;
        items = [];
        foreach (static_properties as group) {
            children = [];
            foreach (group["items"] as item) {
                if (!empty(item["attr_item"])) {
                    children[] = item;
                }
            }
            if (!empty(children)) {
                group["items"] = children;
                items[] = group;
            }
        }
        static_properties = items;
        return compact("properties", "static_properties");
    }/**
     * 根据属性选择值获取货品和附加属性
     * @param int[] properties
     * @param int goods
     * @return array{product_properties: array, properties: array, properties_price: float, properties_label: string, total_properties_price: float, product: ProductModel|null}
     *     properties_price 未排除货品属性之后的价格
     *     total_properties_price 全部选中属性的价格
     */
    public static array GetProductAndPriceWithProperties(array properties,int goods) {
        return CartRepository.Cache().GetOrSet(implode(",", properties), goods,
            use (properties, goods) () {
                list(product_properties, properties, properties_price, properties_label, total_properties_price) = self.SplitProperties(properties);
                product = empty(product_properties) ? null :
                    self.GetProduct(product_properties, goods);
                return compact("product_properties", "properties", "properties_price", "product", "properties_label", "total_properties_price");
        });
    }

    /**
     * 根据属性值获取货品
     * @param array properties
     * @param int goods
     * @return ProductModel|null
     */
    private static  GetProduct(array properties,int goods) {
        sort(properties);
        attributes = implode(ProductModel.ATTRIBUTE_LINK, properties);
        return ProductModel.Where("attributes", attributes).Where("goods_id", goods).First();
    }

    public static array FormatPostProperties(object properties) {
        data = [];
        if (empty(properties)) {
            return data;
        }
        if (!is_array(properties)) {
            properties = Json.Decode(properties);
        }
        foreach ((array)properties as item) {
            args = explode(":", item);
            value = intval(count(args) > 1 ? args[1] : args[0]);
            if (value < 1 || in_array(value, data)) {
                continue;
            }
            data[] = value;
        }
        sort(data);
        return data;
    }

    /**
     * 分离商品规格和附加属性
     * @param int[] properties
     * @return array [product: [], properties: [], properties_price: 0, properties_label: string.Empty, total_properties_price]
     */
    public static  SplitProperties(array properties) {
        if (empty(properties)) {
            return [[], [], 0, string.Empty, 0];
        }
        items = GoodsAttributeEntity.WhereIn("id", properties).Get("attribute_id", "id", "price", "value");
        if (empty(data)) {
            return [[], [], 0, string.Empty, 0];
        }

        attrId = [];
        data = [];
        foreach (items as item) {
            attrId[] = item["attribute_id"];
            if (!isset(data[item["attribute_id"]])) {
                data[item["attribute_id"]] = [
                    "price" => 0,
                    "items" => [],
                    "label" => []
                ];
            }
            data[item["attribute_id"]]["price"] += item["price"];
            data[item["attribute_id"]]["items"][] = item["id"];
            data[item["attribute_id"]]["label"][] = item["value"];
        }
        properties_price = 0;
        total_properties_price = 0;
        properties_label = [];
        attr_list = AttributeEntity.WhereIn("id", attrId).Get("id", "type", "name");
        properties = product_properties = [];
        foreach (attr_list as item) {
            group = data[item["id"]];
            total_properties_price += group["price"];
            properties_label[] = sprintf("[%s]:%s", item["name"], implode(",", group["label"]));
            if (item.Type == 2) {
                properties = array_merge(properties, group["items"]);
                properties_price += group["price"];
                continue;
            }
            product_properties = array_merge(product_properties, group["items"]);
        }
        return [product_properties, properties, properties_price, implode(";", properties_label), total_properties_price];
    }

    public static array GetProperties(int group,int goods) {
        if (group < 1) {
            return [];
        }
        attr_list = AttributeModel.Where("group_id", group)
            .Where("type", ">", 0).OrderBy("position asc").OrderBy("type asc")
            .Get("id", "name", "type");
        if (empty(attr_list)) {
            return [];
        }
        return array_filter(Relation.Create(attr_list, [
            "attr_items" => [
                "query" => GoodsAttributeModel.Where("goods_id", goods),
                "link" => ["id", "attribute_id"],
            ]
        ]),  (object item) {
            return empty(item.AttrItems);
        });
    }

    public static  GetStaticProperties(int group,int goods) {
        if (group < 1) {
            return [];
        }
        attr_list = AttributeUniqueModel.Where("group_id", group)
            .Where("type", 0).OrderBy("position asc").OrderBy("type asc")
            .Get("id", "name", "property_group");
        if (empty(attr_list)) {
            return [];
        }
        items = Relation.Create(attr_list, [
            "attr_item" => [
                "query" => GoodsAttributeModel.Where("goods_id", goods),
                "link" => ["id", "attribute_id"],
                "type" => Relation.TYPE_ONE
            ]
        ]);
        data = [];
        foreach (items as item) {
            if (empty(item["attr_item"])) {
                continue;
            }
            groupName = trim(item["property_group"]);
            if (!isset(data[groupName])) {
                data[groupName] = [
                    "name" => groupName,
                    "items" => []
                ];
            }
            data[groupName]["items"][] = item;
        }
        return array_values(data);
    }

    /**
     * 一次性获取属性及静态属性
     * @param int group
     * @param int goods
     * @return array{properties: array, static_properties: array}
     * @throws \Exception
     */
    public static array BatchProperties(int group,int goods) {
        properties = [];
        static_properties = [];
        if (group < 1) {
            return compact("properties", "static_properties");
        }
        attr_list = AttributeEntity.Where("group_id", group)
            .OrderBy("position asc").OrderBy("type asc")
            .Get("id", "name", "property_group", "type");
        if (empty(attr_list)) {
            return compact("properties", "static_properties");
        }
        attrId = [];
        foreach (attr_list as item) {
            attrId[] = item["id"];
            if (item["type"] > 0) {
                properties[item["id"]] = [
                    "id" => item["id"],
                    "name" => item["name"],
                    "type" => intval(item["type"]),
                    "attr_items" => []
                ];
                continue;
            }
            groupName = trim(item["property_group"]);
            if (!isset(static_properties[groupName])) {
                static_properties[groupName] = [
                    "name" => groupName,
                    "items" => []
                ];
            }
            static_properties[groupName]["items"][item["id"]] = [
                "id" => item["id"],
                "name" => item["name"],
                "group" => groupName,
                "attr_item" => null,
            ];
        }
        items = GoodsAttributeModel.Where("goods_id", goods)
            .WhereIn("attribute_id", attrId).Get();
        foreach (items as item) {
            attrId = intval(item["attribute_id"]);
            if (isset(properties[attrId])) {
                properties[item["attribute_id"]]["attr_items"][] = item;
                continue;
            }
            foreach (static_properties as n => group) {
                if (isset(group["items"][attrId])) {
                    static_properties[n]["items"][attrId]["attr_item"] = item;
                }
            }
        }
        items = [];
        foreach (properties as item) {
            if (!empty(item["attr_items"])) {
                items[] = item;
            }
        }
        properties = items;
        items = [];
        foreach (static_properties as group) {
            children = [];
            foreach (group["items"] as item) {
                if (!empty(item["attr_item"])) {
                    children[] = item;
                }
            }
            if (!empty(children)) {
                group["items"] = children;
                items[] = group;
            }
        }
        static_properties = items;
        return compact("properties", "static_properties");
    }
    }
}
