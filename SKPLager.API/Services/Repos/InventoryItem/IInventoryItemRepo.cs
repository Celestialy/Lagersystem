using SKPLager.Shared.Models;
using SKPLager.Shared.Models.Paging;
using SKPLager.Shared.Wrappers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SKPLager.API.Services.Repos
{
    public interface IInventoryItemRepo : IRepo<InventoryItem, int>
    {
        Task<PagedList<InventoryItem>> GetAsPagedList(int inventoryId,Pagination pagination);
        Task<InventoryItem> GetWithIncludesAsync(int itemId);
        Task<InventoryItem> GetItemFromBarcode(string barcode);
    }
}
