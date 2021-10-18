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
    public partial class InventoryInfo:ComponentBase
    {
        [Parameter]
        public string inventoryName { get; set; }

        string dialogName = "";

        Output[] sortedOutPutList = null;
        PagedList<Output> pagedOutPutList;

        List<Output> outputList = new List<Output>();

        bool categoryFilterDrawer = false, hideClearFilterBtn = true;

        List<Output> unfilteredList = new List<Output>();

        public PagedList<Category> categories;
        public string selectedCategory;

        public bool newItemDialog = false, deleteItemDialog = false;

        InventoryItem selectedItemDialog = new InventoryItem();

        class Output
        {
            public int ID { get; set; }

            public string Brand { get; set; }

            public string Model { get; set; }

            public string Image { get; set; }

            public string Category { get; set; }

            public string Barcode { get; set; }

            public int Amount { get; set; }

            public int TotalAmount { get; set; }
        }

        /// <summary>
        /// Gets the items in the inventory from the API
        /// </summary>
        void GetItems()
        {
            outputList = new List<Output>();
            categories = new PagedList<Category>();

            for (int i = 0; i < 5; i++)
                categories.Add(new Category
                {
                    Id = (i + 1),
                    Name = "Test Kategori " + (i + 1)
                });

            Random rand = new Random();

            for (int i = 0; i < categories.Count; i++)
            {
                for (int j = 0; j < 1; j++)
                {
                    outputList.Add(new Output
                    {
                        ID = (i + 1),
                        Brand = "Test brand " + (i + 1),
                        Model = "Test Model " + (i + 1),
                        Image = "https://cdn.discordapp.com/attachments/458255623793016832/839388883417694229/LenovoUSBDock.png",
                        Category = categories[i].Name,
                        Barcode = "123456789",
                        Amount = rand.Next(1, 101),
                        TotalAmount = 100
                    });
                }
            }

            SortData(null);
        }

        /// <summary>
        /// Sorts the table
        /// </summary>
        /// <param name="sort"></param>
        void SortData(MatSortChangedEvent sort)
        {
            sortedOutPutList = outputList.ToArray();
            if (!(sort == null || sort.Direction == MatSortDirection.None || string.IsNullOrEmpty(sort.SortId)))
            {
                Comparison<Output> comparison = null;
                switch (sort.SortId)
                {
                    case "brand":
                        comparison = (s1, s2) => string.Compare(s1.Brand, s2.Brand, StringComparison.InvariantCultureIgnoreCase);
                        break;
                    case "model":
                        comparison = (s1, s2) => s1.Model.CompareTo(s2.Model);
                        break;
                    case "category":
                        comparison = (s1, s2) => s1.Category.CompareTo(s2.Category);
                        break;
                    case "currentAmount":
                        comparison = (s1, s2) => s1.Amount.CompareTo(s2.Amount);
                        break;
                    case "totalAmount":
                        comparison = (s1, s2) => s1.TotalAmount.CompareTo(s2.TotalAmount);
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

            pagedOutPutList = new PagedList<Output>();
            pagedOutPutList.AddRange(sortedOutPutList);
        }

        /// <summary>
        /// An event that triggers when a category is selected from the filter
        /// </summary>
        /// <param name="_selectedCategory"></param>
        void CategorySelectedEvent(Category _selectedCategory)
        {

            if (unfilteredList.Count == 0)
                unfilteredList = outputList;

            outputList = unfilteredList.Where(x => x.Category == _selectedCategory.Name).ToList();
            SortData(null);

            hideClearFilterBtn = false;

            categoryFilterDrawer = false;
        }

        /// <summary>
        /// Creates a new item
        /// </summary>
        /// <param name="_item"></param>
        public void CreateItem(InventoryItem _item)
        {
            _item.DepartmentId = 1;
            _item.Item.DepartmentId = 1;
            _item.InventoryId = 1;

            outputList.Add(new Output
            {
                ID = outputList.Count + 1,
                Brand = _item.Item.Brand,
                Model = _item.Item.Model,
                Image = "https://cdn.discordapp.com/attachments/458255623793016832/839388883417694229/LenovoUSBDock.png",
                Category = categories.FirstOrDefault(x => x.Id == _item.Item.CategoryId).Name,
                Barcode = "123456789",
                Amount = _item.Amount,
                TotalAmount = _item.TotalAmount
            });

            newItemDialog = false;

            SortData(null);
            StateHasChanged();
        }

        /// <summary>
        /// Opens the dialog to edit an item
        /// </summary>
        /// <param name="_selectedItem"></param>
        void OpenEditItemDialog(Output _selectedItem)
        {
            selectedItemDialog = new InventoryItem
            {
                ItemId = _selectedItem.ID,
                Item = new Item
                {                    
                    Brand = _selectedItem.Brand,
                    Model = _selectedItem.Model,
                    Image = new Image
                    {
                        Id = 1,
                        Uri = new Uri(_selectedItem.Image)
                    },
                    Category = categories.FirstOrDefault(x => x.Name == _selectedItem.Category),
                    Barocde = _selectedItem.Barcode
                },
                Amount = _selectedItem.Amount,
                TotalAmount = _selectedItem.TotalAmount
            };

            dialogName = $"Rediger {selectedItemDialog.Item.Brand} {selectedItemDialog.Item.Model}";

            newItemDialog = true;
        }

        /// <summary>
        /// Sends the edited item to the API
        /// </summary>
        /// <param name="_item"></param>
        public void EditItem(InventoryItem _item)
        {
            int index = outputList.IndexOf(outputList.FirstOrDefault(x => x.ID == _item.ItemId));

            var editedItem = new Output
            {
                ID = _item.ItemId,
                Brand = _item.Item.Brand,
                Model = _item.Item.Model,
                Image = "https://cdn.discordapp.com/attachments/458255623793016832/839388883417694229/LenovoUSBDock.png",
                Category = categories.FirstOrDefault(x => x.Id == _item.Item.Category.Id).Name,
                Barcode = "123456789",
                Amount = _item.Amount,
                TotalAmount = _item.TotalAmount
            };

            outputList[index] = editedItem;

            newItemDialog = false;

            SortData(null);
            StateHasChanged();
        }

        /// <summary>
        /// Opens the dialog to delete an item
        /// </summary>
        /// <param name="_selectedItem"></param>
        void OpenDeleteItemDialog(Output _selectedItem)
        {
            selectedItemDialog = new InventoryItem
            {
                ItemId = _selectedItem.ID,
                Item = new Item
                {
                    Brand = _selectedItem.Brand,
                    Model = _selectedItem.Model,
                    Image = new Image
                    {
                        Id = 1,
                        Uri = new Uri(_selectedItem.Image)
                    },
                    Category = categories.FirstOrDefault(x => x.Name == _selectedItem.Category),
                    Barocde = _selectedItem.Barcode
                }
            };

            deleteItemDialog = true;
        }

        /// <summary>
        /// Deletes the selected item
        /// </summary>
        /// <param name="_item"></param>
        void DeleteItem(InventoryItem _item)
        {
            int index = outputList.IndexOf(outputList.FirstOrDefault(x => x.ID == _item.ItemId));
            outputList.RemoveAt(index);

            deleteItemDialog = false;

            SortData(null);
            StateHasChanged();
        }

        protected override void OnInitialized()
        {
            selectedItemDialog = new InventoryItem();
            selectedItemDialog.Item = new Item
            {
                Barocde = "123456789"
            };

            GetItems();
            SortData(null);
        }
    }
}