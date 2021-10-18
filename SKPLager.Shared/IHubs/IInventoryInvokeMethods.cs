using SKPLager.Shared.Models.Paging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SKPLager.Shared.IHubs
{
    public interface IInventoryInvokeMethods
    {
        #region Groups
        Task AddToGroup(string groupName);
        Task RemoveFromGroup(string groupName);
        Task LinkWithPhone(string code);
        Task DisconnectFromPhone(string code);
        #endregion
        #region Inventories
        Task GetInventories(Pagination pagination);

        #endregion
        #region Images
        Task GetImages(Pagination pagination);
        #endregion
        #region Categories
        Task GetCategories(Pagination pagination);
        #endregion
        #region Items
        Task GetItems(int inventoryId, Pagination pagination);
        Task GetItemFromBarcode(string linkCode, string barcode);
        #endregion
        #region Loan
        Task GetLoans(int inventoryId, Pagination pagination);
        Task GetLoanHistory(int inventoryId, Pagination pagination);
        Task GetUserLoanHistory(int inventoryId, string userId, Pagination pagination);
        #endregion
        #region Loan
        Task GetConsumptions(int inventoryId, Pagination pagination);
        Task GetConsumptionHistory(int inventoryId, Pagination pagination);
        Task GetUserConsumptionHistory(int inventoryId, string userId, Pagination pagination);
        #endregion

        Task ScannedBarcode(string linkCode, string barcode);
    }
}
