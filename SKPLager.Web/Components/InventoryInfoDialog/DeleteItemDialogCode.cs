using Microsoft.AspNetCore.Components;
using SKPLager.Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SKPLager.Web.Components
{
    public partial class DeleteItemDialog
    {
        [Parameter]
        public bool OpenDialog { get; set; }

        [Parameter]
        public Action<InventoryItem> ItemSubmitted { get; set; }

        [Parameter]
        public InventoryItem SelectedItem { get; set; }

        public void DeleteItem()
        {
            OpenDialog = false;

            ItemSubmitted?.Invoke(SelectedItem);
        }

        public void CloseDialog()
        {
            SelectedItem.Item.Brand = "";
            SelectedItem.Item.Model = "";

            OpenDialog = false;
        }
    }
}