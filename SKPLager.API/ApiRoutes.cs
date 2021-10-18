using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SKPLager.API
{
    public static class ApiRoutes
    {
        public const string Api = "";
        public const string SignalR = "hub/inventoryhub";
        public static class Inventory
        {
            private const string Id = "/{InventoryId}";
            private const string Base = "/Inventories";
            private const string Selected = Base + Id;

            public const string GetAll = Base;
            public const string GetOne = Base + Id;
            public const string Create = Base;
            public const string Delete = Base + Id;
            public static class Item
            {
                private const string Id = "/{ItemId}";
                private const string Base = Inventory.Selected + "/items";

                public const string GetAll = Base;
                public const string GetOne = Base + Id;
                public const string Create = Base;
                public const string Update = Base + Id;
                public const string Delete = Base + Id;
            }

            public static class Category
            {
                private const string Id = "/{CategoryId}";
                private const string Base = Inventory.Selected + "/Categories";

                public const string GetAll = Base;
            }

            public static class Loan
            {
                private const string Id = "/{LoanId}";
                private const string Base = Inventory.Selected + "/Loans";
                //private const string Selected = Base + Id;

                public const string GetAll = Base;
                public const string GetOne = Base + Id;
                public const string Create = Base;
                public const string Return = Base + Id;
                public const string Delete = Base + Id;
                public static class User
                {
                    private const string Id = "/{UserId}";
                    private const string Base = Loan.Base + "/User";

                    public const string GetHistory = Base;
                    public const string GetUserHistory = Base + Id;
                }

            }

            public static class Consumption
            {
                private const string Id = "/{ConsumptionId}";
                private const string Base = Inventory.Selected + "/Consumptions";
                //private const string Selected = Base + Id;

                public const string GetAll = Base;
                public const string GetOne = Base + Id;
                public const string Create = Base;
                public const string Delete = Base + Id;
                public static class User
                {
                    private const string Id = "/{UserId}";
                    private const string Base = Consumption.Base + "/User";

                    public const string GetHistory = Base;
                    public const string GetUserHistory = Base + Id;
                }
            }
        }

        public static class Image
        {
            private const string Id = "/{ImageId}";
            private const string Base = "/Images";

            public const string GetAll = Base;
            public const string GetOne = Base + Id;
            public const string Create = Base;
            public const string Delete = Base + Id;
        }

        public static class Category
        {
            private const string Id = "/{CategoryId}";
            private const string Base = "/Categories";

            public const string GetAll = Base;
            public const string GetOne = Base + Id;
            public const string Create = Base;
            public const string Delete = Base + Id;
        }

        public static class Loan
        {
            private const string Id = "/{LoanId}";
            private const string Base = "/Loans";

            public const string GetLoanHistory = Base;
        }
        public static class Department
        {
            private const string Id = "/{DepartmentId}";
            private const string Base = "/Departments";

            public static class Printer
            {

                private const string Base = "/Printers";

                public const string Print = Base + Id;
            }
        }
        

    }
}
