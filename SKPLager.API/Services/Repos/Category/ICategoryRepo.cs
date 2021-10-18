using SKPLager.Shared.Models;
using SKPLager.Shared.Models.Paging;
using SKPLager.Shared.Wrappers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SKPLager.API.Services.Repos
{
    public interface ICategoryRepo : IRepo<Category, int>
    {
        Task<PagedList<Category>> GetAsPagedList(Pagination pagination);
        Task<IEnumerable<Category>> GetCategoriesFromInventory(int inventoryId);
    }
}
