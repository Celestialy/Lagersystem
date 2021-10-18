using Microsoft.EntityFrameworkCore;
using SKPLager.API.Database;
using SKPLager.API.Helpers;
using SKPLager.Shared.Models;
using SKPLager.Shared.Models.Paging;
using SKPLager.Shared.Wrappers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SKPLager.API.Services.Repos
{
    public class InventoryRepo : Repo<Inventory, int>, IInventoryRepo
    {
        public InventoryRepo(DBContext context, ICurrentUserDepartmentService currentDepartment) : base(context, currentDepartment)
        {
        }

        public async Task<PagedList<Inventory>> GetAsPagedList(Pagination pagination)
        {
            if (pagination == null)
            {
                throw new ArgumentNullException(nameof(pagination));
            }
            pagination.PreparePaging();
            var source = GetAll();
            if (!string.IsNullOrWhiteSpace(pagination.SearchQuery))
            {
                source = source.Where(x => x.Name.Contains(pagination.SearchQuery));
            }
            if (!string.IsNullOrWhiteSpace(pagination.OrderBy))
            {
                source = source.TryOrderBy<Inventory>(pagination.OrderBy);
            }
            if (pagination.DepartmentId != null)
            {
                if (_CurrentDepartment.IsInDepartment(pagination.DepartmentId.Value))
                {
                    source = source.Where(x => x.DepartmentId == pagination.DepartmentId.Value);
                }
                else
                {
                    throw new ArgumentOutOfRangeException(nameof(pagination.DepartmentId));
                }
            }

            return await PagedList<Inventory>.CreateAsync(source, pagination);
        }

        public override async Task<Inventory> GetAsync(int OId)
        {
            return await _context.Inventories.Include(ii => ii.Items).ThenInclude(i => i.Item).SingleOrDefaultAsync(x => x.Id == OId);

        }
    }
}
