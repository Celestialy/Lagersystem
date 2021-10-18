using SKPLager.Shared.Models;
using SKPLager.Shared.Models.Paging;
using SKPLager.Shared.Wrappers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SKPLager.API.Services.Repos
{
    public interface IInventoryRepo : IRepo<Inventory, int>
    {
        Task<PagedList<Inventory>> GetAsPagedList(Pagination pagination);
    }
}
