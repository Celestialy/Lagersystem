using SKPLager.Shared.Models;
using SKPLager.Shared.Models.Paging;
using SKPLager.Shared.Wrappers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SKPLager.API.Services.Repos
{
    public interface IImageRepo : IRepo<Image, int>
    {
        Task<PagedList<Image>> GetAsPagedList(Pagination pagination);
    }
}
