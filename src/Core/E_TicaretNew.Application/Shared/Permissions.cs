namespace E_TicaretNew.Application.Shared;

public static class Permissions
{
    public static class Category
    {
        public const string Create = "Category.Create";
        public const string Update = "Category.Update";
        public const string Delete = "Category.Delete";
     

        public static List<string> All = new()
        { 
           Create,
            Update,
            Delete,
        };
    }
    public static class Order
    {
        public const string Create = "Order.Create";
        public const string Update = "Order.Update";
        public const string Delete = "Order.Delete";
        public const string GetMy = "Order.GetMy";
        public const string GetDetail = "Order.GetDetail";
        public const string GetMySales = "Order.GetMySales";

        public static List<string> All = new()
        {
            Create,
            Update,
            Delete,
            GetMy,
            GetDetail,
            GetMySales,
        };
     }

    public static class Product
    {
        public const string Create = "Product.Create";
        public const string Update = "Product.Update";
        public const string Delete = "Product.Delete";
        public const string GetMy = "Product.GetMy";
        public const string DeleteProductImage = "Product.DeleteProductImage";
        public const string AddProductImage = "Product.AddProductImage";
        public const string AddProductFavourite = "Product.AddProductFavourite";

        public static List<string> All = new()
        { 
            Create,
            Update,
            Delete,
            GetMy,
            DeleteProductImage,
            AddProductImage,
            AddProductFavourite,


        };
    }
    public static class Account
    {
        public const string AddRole = "Account.AddRole";
        public const string Create = "Account.Create";

        public static List<string> All = new()
        { 
              AddRole,
              Create
        };
    }

    public static class Role
    {
        public const string Create = "Role.Create";
        public const string Update = "Role.Update";
        public const string Delete = "Role.Delete";
        public const string GetAllPermission = "Role.GetAllPermission";

        public static List<string> All = new()
        {
           Create,
            Update,
            Delete,
        };

    }
}
