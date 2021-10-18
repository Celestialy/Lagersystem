using SKPLager.Shared.Models;
using SKPLager.Shared.Models.Paging;
using SKPLager.Shared.Wrappers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SKPLager.API.Services.Repos
{
    public interface IConsumptionItemRepo : IRepo<ConsumptionItem, int>
    {
        Task<PagedList<ConsumptionItem>> GetAsPagedList(int inventoryId, Pagination pagination);

        Task<PagedList<ConsumptionItem>> GetUserHistory(int inventoryId, string userId, Pagination pagination);
        Task<ConsumptionItem> GetWithIncludesAsync(int itemId);
    }
}
