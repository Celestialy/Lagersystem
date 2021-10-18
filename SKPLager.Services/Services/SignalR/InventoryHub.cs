using Microsoft.AspNetCore.SignalR.Client;
using SKPLager.Shared.Helpers;
using SKPLager.Shared.IHubs;
using SKPLager.Shared.Models;
using SKPLager.Shared.Models.Paging;
using SKPLager.Shared.Wrappers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SKPLager.Services.SignalR
{
    public class InventoryHub : IInventoryInvokeMethods
    {
        private HubConnection connection { get; }
        public InventoryHub(HubConnection hubConnection)
        {
            connection = hubConnection;
            
        }
        #region registries
        #region Inventories
        public IDisposable RegisterOnPagedInventories(Action<PagedList<Inventory>> PagedInventories)
            => connection.BindOnInterface(x => x.PagedInventories, PagedInventories);
        public IDisposable RegisterOnCreatedInventory(Action<Inventory> CreatedInventory)
            => connection.BindOnInterface(x => x.CreatedInventory, CreatedInventory);
        public IDisposable RegisterOnDeletedInventory(Action<int> DeletedInventoryId)
            => connection.BindOnInterface(x => x.DeletedInventory, DeletedInventoryId);
        #endregion
        #region Categories
        public IDisposable RegisterOnPagedCategories(Action<PagedList<Category>> PagedCategories)
            => connection.BindOnInterface(x => x.PagedCategories, PagedCategories);
        public IDisposable RegisterOnCreatedCategory(Action<Category> CreatedCategory)
            => connection.BindOnInterface(x => x.CreatedCategory, CreatedCategory);
        public IDisposable RegisterOnDeletedCategory(Action<int> DeletedCategoryId)
            => connection.BindOnInterface(x => x.DeletedCategory, DeletedCategoryId);
        #endregion
        #region Images
        public IDisposable RegisterOnPagedImages(Action<PagedList<Image>> PagedImages)
            => connection.BindOnInterface(x => x.PagedImages, PagedImages);
        public IDisposable RegisterOnCreatedImage(Action<Image> CreatedImage)
            => connection.BindOnInterface(x => x.CreatedImage, CreatedImage);
        public IDisposable RegisterOnDeletedImage(Action<int> DeletedImageId)
            => connection.BindOnInterface(x => x.DeletedImage, DeletedImageId);
        #endregion
        #region Items
        public IDisposable RegisterOnPagedItems(Action<PagedList<InventoryItem>> PagedItems)
            => connection.BindOnInterface(x => x.PagedItems, PagedItems);
        public IDisposable RegisterOnCreatedItem(Action<InventoryItem> CreatedItem)
            => connection.BindOnInterface(x => x.CreatedItem, CreatedItem);
        public IDisposable RegisterOnDeletedItem(Action<int> DeletedItemId)
            => connection.BindOnInterface(x => x.DeletedItem, DeletedItemId);
        public IDisposable RegisterOnUpdatedItem(Action<InventoryItem> UpdatedItem)
            => connection.BindOnInterface(x => x.UpdatedItem, UpdatedItem);
        public IDisposable RegisterOnScannedItem(Action<InventoryItem> ScannedItem)
            => connection.BindOnInterface(x => x.ScannedItem, ScannedItem);
        #endregion
        #region Loan
        public IDisposable RegisterOnPagedLoans(Action<PagedList<LoanItem>> PagedLoans)
            => connection.BindOnInterface(x => x.PagedLoans, PagedLoans);
        public IDisposable RegisterOnCreatedLoan(Action<LoanItem> CreatedLoan)
            => connection.BindOnInterface(x => x.CreatedLoan, CreatedLoan);
        public IDisposable RegisterOnDeletedLoan(Action<int> DeletedLoanId)
            => connection.BindOnInterface(x => x.DeletedLoan, DeletedLoanId);
        public IDisposable RegisterOnUpdatedLoan(Action<LoanItem> UpdatedLoan)
            => connection.BindOnInterface(x => x.UpdatedLoan, UpdatedLoan);
        #endregion
        #region Consumption
        public IDisposable RegisterOnPagedConsumptions(Action<PagedList<ConsumptionItem>> PagedConsumptions)
            => connection.BindOnInterface(x => x.PagedConsumptions, PagedConsumptions);
        public IDisposable RegisterOnCreatedConsumption(Action<ConsumptionItem> CreatedConsumption)
            => connection.BindOnInterface(x => x.CreatedConsumption, CreatedConsumption);
        public IDisposable RegisterOnDeletedConsumption(Action<int> DeletedConsumptionId)
            => connection.BindOnInterface(x => x.DeletedConsumption, DeletedConsumptionId);
        public IDisposable RegisterOnUpdatedLoan(Action<ConsumptionItem> UpdatedConsumption)
            => connection.BindOnInterface(x => x.UpdatedConsumption, UpdatedConsumption);
        #endregion
        public IDisposable RegisterOnScannedBarcode(Action<string> Barcode)
            => connection.BindOnInterface(x => x.ScannedBarcode, Barcode);
        #endregion
        #region Invoke
        #region Signalr management
        public Task AddToGroup(string groupName)
            => connection.InvokeAsync(nameof(IInventoryInvokeMethods.AddToGroup), groupName);

        public Task RemoveFromGroup(string groupName)
            => connection.InvokeAsync(nameof(IInventoryInvokeMethods.RemoveFromGroup), groupName);

        public Task DisconnectFromPhone(string code)
            => connection.InvokeAsync(nameof(IInventoryInvokeMethods.DisconnectFromPhone), code);
        public Task LinkWithPhone(string code)
            => connection.InvokeAsync(nameof(IInventoryInvokeMethods.LinkWithPhone), code);
        #endregion

        public Task GetCategories(Pagination pagination)
            => connection.InvokeAsync(nameof(IInventoryInvokeMethods.GetCategories), pagination);

        public Task GetImages(Pagination pagination)
             => connection.InvokeAsync(nameof(IInventoryInvokeMethods.GetImages), pagination);
        
        public Task GetInventories(Pagination pagination)
             => connection.InvokeAsync(nameof(IInventoryInvokeMethods.GetInventories), pagination);
        
        public Task GetItemFromBarcode(string linkCode, string barcode)
             => connection.InvokeAsync(nameof(IInventoryInvokeMethods.GetItemFromBarcode), linkCode, barcode);

        public Task GetItems(int inventoryId, Pagination pagination)
            => connection.InvokeAsync(nameof(IInventoryInvokeMethods.GetItems), inventoryId, pagination);

        public Task ScannedBarcode(string linkCode, string barcode)
             => connection.InvokeAsync(nameof(IInventoryInvokeMethods.GetItemFromBarcode), linkCode, barcode);

        public Task GetLoans(int inventoryId, Pagination pagination)
            => connection.InvokeAsync(nameof(IInventoryInvokeMethods.GetLoans), inventoryId, pagination);

        public Task GetLoanHistory(int inventoryId, Pagination pagination)
            => connection.InvokeAsync(nameof(IInventoryInvokeMethods.GetLoanHistory), inventoryId, pagination);

        public Task GetUserLoanHistory(int inventoryId, string userId, Pagination pagination)
            => connection.InvokeAsync(nameof(IInventoryInvokeMethods.GetUserLoanHistory), inventoryId, userId ,pagination);

        public Task GetConsumptions(int inventoryId, Pagination pagination)
            => connection.InvokeAsync(nameof(IInventoryInvokeMethods.GetConsumptions), inventoryId, pagination);

        public Task GetConsumptionHistory(int inventoryId, Pagination pagination)
            => connection.InvokeAsync(nameof(IInventoryInvokeMethods.GetConsumptionHistory), inventoryId, pagination);

        public Task GetUserConsumptionHistory(int inventoryId, string userId, Pagination pagination)
            => connection.InvokeAsync(nameof(IInventoryInvokeMethods.GetUserConsumptionHistory), inventoryId, userId, pagination);
        #endregion
    }
}
