using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SKPLager.Services
{
    internal static class ApiRoutes
    {
        public const string Api = "";
        public const string SignalR = "hub/inventoryhub";
        public static class Inventory
        {
            private static string BaseWithId(int inventoryId) => Base + "/" + inventoryId;
            private const string Base = "/Inventories";

            public const string GetAll = Base;
            public static string GetOne(int id) => BaseWithId(id);
            public const string Create = Base;
            public static string Delete(int id) => BaseWithId(id);
            public static class Item
            {
                private static string BaseWithId(int inventoryId, int itemId) => Base(inventoryId) + "/" + itemId;
                private static string Base(int inventoryId) => Inventory.BaseWithId(inventoryId) + "/items";


                public static string GetAll(int inventoryId) => Base(inventoryId);
                public static string GetOne(int inventoryId, int itemId) => BaseWithId(inventoryId, itemId);
                public static string Create(int inventoryId) => Base(inventoryId);
                public static string Update(int inventoryId, int itemId) => BaseWithId(inventoryId, itemId);
                public static string Delete(int inventoryId, int itemId) => BaseWithId(inventoryId, itemId);
            }

            public static class Category
            {
                private static string BaseWithId(int inventoryId, int categoryId) => Base(inventoryId) + "/" + categoryId;
                private static string Base(int inventoryId) => Inventory.BaseWithId(inventoryId) + "/Categories";

                public static string GetAll(int inventoryId) => Base(inventoryId);
            }
        }

        public static class Image
        {
            private static string BaseWithId(int imageId) => Base + "/" + imageId;
            private const string Base = "/Images";

            public const string GetAll = Base;
            public static string GetOne(int imageId) => BaseWithId(imageId);
            public const string Create = Base;
            public static string Delete(int imageId) => BaseWithId(imageId);
        }

        public static class Category
        {
            private static string BaseWithId(int categoryId) => Base + "/" + categoryId;
            private const string Base = "/Categories";

            public const string GetAll = Base;
            public static string GetOne(int categoryId) => BaseWithId(categoryId);
            public const string Create = Base;
            public static string Delete(int categoryId) => BaseWithId(categoryId);
        }

    }
}
