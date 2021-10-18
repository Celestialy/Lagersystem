using Microsoft.AspNetCore.Components;
using System;
using SKPLager.Shared.Models;
using SKPLager.Shared.Wrappers;

namespace SKPLager.Web.Components
{
    public partial class NewItemDialog
    {
        [Parameter]
        public bool OpenDialog { get; set; }

        [Parameter]
        public Action<InventoryItem> ItemSubmitted { get; set; }

        [Parameter]
        public string DialogName { get; set; }

        [Parameter]
        public EventCallback<bool> openDialogChanged { get; set; }

        [Parameter]
        public InventoryItem SelectedItem { get; set; }

        [Parameter]
        public EventCallback<InventoryItem> selectedItemChanged { get; set; }

        [Parameter]
        public PagedList<Category> Categories { get; set; }

        [Parameter]
        public Action<string> CategorySearchChanged { get; set; }

        public Category SelectedCategory { get; set; }

        [Parameter]
        public PagedList<Image> Images { get; set; }

        [Parameter]
        public Action<string> ImageSearchChanged { get; set; }

        public Image SelectedImage { get; set; }

        [Parameter]
        public string InventoryName { get; set; }

        public void CloseDialog()
        {
            SelectedItem.Item.Brand = "";
            SelectedItem.Item.Model = "";
            SelectedItem.Item.Barocde = "123456789";

            SelectedItem.Amount = 0;
            SelectedItem.TotalAmount = 0;

            SelectedCategory = null;

            OpenDialog = false;
        }

        public void CreateItem()
        {
            SelectedItem = new InventoryItem
            {
                Amount = SelectedItem.Amount,
                TotalAmount = SelectedItem.TotalAmount,
                Item = new Item
                {
                    Brand = SelectedItem.Item.Brand,
                    Model = SelectedItem.Item.Model,
                    Barocde = SelectedItem.Item.Barocde,
                    CategoryId = SelectedCategory.Id,
                    ImageId = SelectedImage.Id
                },
            };

            OpenDialog = false;

            ItemSubmitted?.Invoke(SelectedItem);
        }

        [Parameter]
        public Action<InventoryItem> ItemEdited { get; set; }

        public void EditItem()
        {
            ItemEdited?.Invoke(SelectedItem);
        }

        protected override void OnInitialized()
        {
            SelectedImage = new Image
            {
                Id = 1,
                Uri = new Uri("https://cdn.discordapp.com/attachments/458255623793016832/839388883417694229/LenovoUSBDock.png")
            };
        }
    }
}
