using MatBlazor;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using SKPLager.Shared.Helpers;
using SKPLager.Shared.Models;
using SKPLager.Shared.Models.Enums;
using SKPLager.Shared.Models.User;
using SKPLager.Shared.Wrappers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SKPLager.Web.Pages.Admin
{
    public partial class InventoryManagement
    {
        [Inject]
        public AuthenticationStateProvider AuthStateProvider { get; set; }
        List<Department> departments = new List<Department>();

        [Inject]
        public NavigationManager navigation { get; set; }

        bool newInventoryDialog = false;
        string newInventoryName = "";
        string newSelectedInventoryType = "";

        bool deleteInventoryDialog = false;
        FrontEndInventory selectedInventory = new FrontEndInventory();

        void createInventory()
        {
            if (newInventoryName != "" && newSelectedInventoryType != "")
            {
                if (newSelectedInventoryType == "Udlån")
                {
                    Inventories.Add(new Inventory
                    {
                        Id = Inventories.Count + 1,
                        Name = newInventoryName,
                        Type = InventoryType.Loan
                    });
                }
                else if (newSelectedInventoryType == "Forbrug")
                {
                    Inventories.Add(new Inventory
                    {
                        Id = Inventories.Count + 1,
                        Name = newInventoryName,
                        Type = InventoryType.Consumption
                    });
                }
            }

            newInventoryDialog = false;
            newInventoryName = "";
            newSelectedInventoryType = "";
            GetInventories();
        }

        class FrontEndInventory
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public string Type { get; set; }
        }

        List<string> inventoryTypes = new List<string>
        {
            "Udlån",
            "Forbrug"
        };

        List<Inventory> Inventories = new List<Inventory>();

        List<FrontEndInventory> outPutList = new List<FrontEndInventory>();
        FrontEndInventory[] sortedOutPutList = null;

        PagedList<FrontEndInventory> pagedOutPutList;

        void GetInventories()
        {
            outPutList = new List<FrontEndInventory>();
            string inventoryType = "";
            foreach (var item in Inventories)
            {
                if (item.Type == InventoryType.Loan)
                    inventoryType = "Udlån";
                else if (item.Type == InventoryType.Consumption)
                    inventoryType = "Forbrug";

                outPutList.Add(new FrontEndInventory
                {
                    Id = item.Id,
                    Name = item.Name,
                    Type = inventoryType
                });
            }

            SortData(null);
        }

        void SortData(MatSortChangedEvent sort)
        {
            sortedOutPutList = outPutList.ToArray();
            if (!(sort == null || sort.Direction == MatSortDirection.None || string.IsNullOrEmpty(sort.SortId)))
            {
                Comparison<FrontEndInventory> comparison = null;
                switch (sort.SortId)
                {
                    case "name":
                        comparison = (s1, s2) => string.Compare(s1.Name, s2.Name, StringComparison.InvariantCultureIgnoreCase);
                        break;
                    case "type":
                        comparison = (s1, s2) => s1.Type.CompareTo(s2.Type);
                        break;
                }
                if (comparison != null)
                {
                    if (sort.Direction == MatSortDirection.Desc)
                        Array.Sort(sortedOutPutList, (s1, s2) => -1 * comparison(s1, s2));
                    else
                        Array.Sort(sortedOutPutList, comparison);
                }
            }

            pagedOutPutList = new PagedList<FrontEndInventory>();
            pagedOutPutList.AddRange(sortedOutPutList);
        }

        void InventoryInfo(string inventoryName)
        {
            navigation.NavigateTo($"inventory/{inventoryName}");
        }

        protected override async Task OnInitializedAsync()
        {
            var user = (await AuthStateProvider.GetAuthenticationStateAsync()).User;
            departments = user.ToDepartments();

            if (departments.Count == 0)
            {
                departments.Add(new Department
                {
                    Name = "Data"
                });

                departments.Add(new Department
                {
                    Name = "Test"
                });
            }

            Inventories = new List<Inventory>();
            outPutList = new List<FrontEndInventory>();
            sortedOutPutList = null;

            Inventories.Add(new Inventory
            {
                Id = 1,
                Name = "TestUdlån",
                Type = InventoryType.Loan,
                CreatedAt = DateTime.Now,
            });

            Inventories.Add(new Inventory
            {
                Id = 2,
                Name = "TestForbrug",
                Type = InventoryType.Consumption,
                CreatedAt = DateTime.Now,
            });

            GetInventories();
            SortData(null);

            pagedOutPutList = new PagedList<FrontEndInventory>();
            pagedOutPutList.AddRange(sortedOutPutList);
        }
    }
}