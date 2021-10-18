using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SKPLager.Shared.Configurations
{
    public static class Group
    {
        public static string Categories(int departmentId) => nameof(Categories) + departmentId;
        public static string Images(int departmentId) => nameof(Images) + departmentId;
        public static string InventoriesPage(int departmentId) => nameof(InventoriesPage) + departmentId;
        public static string InventoryPage(int inventoryId) => nameof(InventoryPage) + inventoryId;
        public static string Phone(string code) => nameof(Phone) + code;
    }
}
