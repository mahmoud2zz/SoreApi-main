using System;
namespace CoffeeStoreApi.Authorization
{
    public static class Permissions
    {
        // Category Permissions
        public const string AddCategory = "category:add";
        public const string RemoveCategory = "category:remove";
        public const string UpdateCategory = "category:update";
        // Product Permission
        public const string AddProduct = "product:add";
        public const string RemoveProduct = "product:remove";
        public const string UpdateProduct = "product:update";
        public const string GetProducts = "product:get";
        public const string GetProductById = "product:getById";
        // Order Permission
        public const string CreateOrder = "order:create";
        public const string GetOrders = "order:get";
        public const string UpdateOrder = "order:update";
        public const string RemoveOrder = "order:remove";


        public static readonly string[] All =
        {
        AddCategory,
        RemoveCategory,
        UpdateCategory,
        AddProduct,
        RemoveProduct,
        UpdateProduct,
        GetProducts,
        GetProductById,
        CreateOrder,
        GetOrders,
        UpdateOrder,
        RemoveOrder
    };
    }
}

