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
    public interface ICategoryService
    {
        /// <summary>
        /// Gets all categories in paged
        /// </summary>
        /// <returns>Return All entity in paged</returns>
        Task<PagedList<Category>> GetAll(Pagination pagination);

        /// <summary>
        /// Gets all categories from inventory
        /// </summary>
        /// <returns>Return All entity in paged</returns>
        Task<List<Category>> GetAllFromInventory(int inventoryId);
        /// <summary>
        /// Get one category
        /// </summary>
        /// <returns>Return entity</returns>
        Task<Category> GetOne(int id);

        /// <summary>
        /// Create category
        /// </summary>
        /// <param name="category"></param>
        /// <returns></returns>
        Task Create(Category category);

        /// <summary>
        /// Delete category
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task Delete(int id);
    }
}
