using SKPLager.Shared.Models;
using SKPLager.Shared.Models.Paging;
using SKPLager.Shared.Wrappers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SKPLager.Services.API
{
    public interface IItemService
    {
        /// <summary>
        /// Gets all items in paged
        /// </summary>
        /// <returns>Return All entity in paged</returns>
        Task<PagedList<InventoryItem>> GetAll(int inventoryId, Pagination pagination);

        /// <summary>
        /// Get one item
        /// </summary>
        /// <returns>Return entity</returns>
        Task<InventoryItem> GetOne(int inventoryId, int itemId);

        /// <summary>
        /// Create item
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        Task Create(int inventoryId, InventoryItem item);

        /// <summary>
        /// Update item
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        Task Update(int inventoryId, int itemId, InventoryItem item);

        /// <summary>
        /// Delete item
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task Delete(int inventoryId, int itemId);
    }
}
