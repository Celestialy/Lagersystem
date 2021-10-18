using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SKPLager.Shared.Models;
using SKPLager.Shared.Models.Paging;
using SKPLager.Shared.Wrappers;

namespace SKPLager.Services.API
{
    public interface IInventoryService 
    {
        /// <summary>
        /// Gets all inventories in paged
        /// </summary>
        /// <returns>Return All entity in paged</returns>
        Task<PagedList<Inventory>> GetAll(Pagination pagination);

        /// <summary>
        /// Get one inventory
        /// </summary>
        /// <returns>Return entity</returns>
        Task<Inventory> GetOne(int id);

        /// <summary>
        /// Create inventory
        /// </summary>
        /// <param name="inventory"></param>
        /// <returns></returns>
        Task Create(Inventory inventory);

        /// <summary>
        /// Delete inventory
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task Delete(int id);
    }
}
