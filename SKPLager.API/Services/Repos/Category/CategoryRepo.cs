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
    public class CategoryRepo : Repo<Category, int>, ICategoryRepo
    {
        public CategoryRepo(DBContext context, ICurrentUserDepartmentService currentDepartment) : base(context, currentDepartment)
        {
        }



        public async Task<IEnumerable<Category>> GetCategoriesFromInventory(int inventoryId)
        {
            return await _context.InventoryItems.Where(x => x.InventoryId == inventoryId && _CurrentDepartment.Ids.Contains(x.DepartmentId)).Select(x => x.Item.Category).Distinct().ToListAsync();
        }

        public async Task<PagedList<Category>> GetAsPagedList(Pagination pagination)
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
                source = source.TryOrderBy<Category>(pagination.OrderBy);
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
            return await PagedList<Category>.CreateAsync(source, pagination);
        }
    }
}
