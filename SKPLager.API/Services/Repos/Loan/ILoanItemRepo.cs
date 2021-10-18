using SKPLager.Shared.Models;
using SKPLager.Shared.Models.Paging;
using SKPLager.Shared.Wrappers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SKPLager.API.Services.Repos
{
    public interface ILoanItemRepo : IRepo<LoanItem, int>
    {
        Task<PagedList<LoanItem>> GetAsPagedList(int inventoryId, Pagination pagination);

        Task<PagedList<LoanItem>> GetUserHistory(int inventoryId, string userId, Pagination pagination);
        Task<LoanItem> GetWithIncludesAsync(int itemId);

        Task ReturnItem(int itemId);
    }
}
