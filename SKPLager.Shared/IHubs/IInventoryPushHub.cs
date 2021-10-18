using SKPLager.Shared.Models;
using SKPLager.Shared.Models.Paging;
using SKPLager.Shared.Wrappers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SKPLager.Shared.IHubs
{
    public interface IInventoryPushHub
    {
        #region Inventories
        Task PagedInventories(PagedList<Inventory> inventories);
        Task CreatedInventory(Inventory inventory);
        //Task UpdatedInventory(Inventory inventory);
        Task DeletedInventory(int inventoryId);
        #endregion
        #region Images
        Task PagedImages(PagedList<Image> images);
        Task CreatedImage(Image image);
        Task DeletedImage(int imageId);
        #endregion
        #region Categories
        Task PagedCategories(PagedList<Category> categories);
        Task CreatedCategory(Category category);
        Task DeletedCategory(int categoryId);
        #endregion
        #region Items
        Task PagedItems(PagedList<InventoryItem> Items);
        Task CreatedItem(InventoryItem item);
        Task UpdatedItem(InventoryItem item);
        Task DeletedItem(int inventoryItemId);
        Task ScannedItem(InventoryItem item);
        #endregion
        #region Loan
        Task PagedLoans(PagedList<LoanItem> loans);
        Task CreatedLoan(LoanItem loan);
        Task UpdatedLoan(LoanItem loan);
        Task DeletedLoan(int loanId);
        #endregion
        #region Consumption
        Task PagedConsumptions(PagedList<ConsumptionItem> consumptions);
        Task CreatedConsumption(ConsumptionItem consumption);
        Task UpdatedConsumption(ConsumptionItem consumption);
        Task DeletedConsumption(int consumptionId);
        #endregion
        Task ScannedBarcode(string barcode);
    }
}
