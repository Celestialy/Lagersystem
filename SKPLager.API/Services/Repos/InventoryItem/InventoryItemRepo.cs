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
    public class InventoryItemRepo : Repo<InventoryItem, int>, IInventoryItemRepo
    {
        public InventoryItemRepo(DBContext context, ICurrentUserDepartmentService currentDepartment) : base(context, currentDepartment)
        {
        }

        public override async Task AddAsync(InventoryItem entity)
        {
            if (entity.Item == null)
            {
                throw new ArgumentNullException(nameof(entity.Item));
            }
            else if (entity.Item.CategoryId == null)
            {
                throw new ArgumentNullException(nameof(entity.Item.CategoryId));
            }
            else if (!_context.Categories.Any(x => x.Id == entity.Item.CategoryId))
            {
                throw new ArgumentException(nameof(entity.Item.CategoryId) + " does not exciest");
            }
            else if (entity.Item.ImageId == null)
            {
                throw new ArgumentNullException(nameof(entity.Item.ImageId));
            }
            else if (!_context.Images.Any(x => x.Id == entity.Item.ImageId))
            {
                throw new ArgumentException(nameof(entity.Item.ImageId) + " does not exciest");
            }
            else if (!_context.Inventories.Any(x => x.Id == entity.InventoryId && x.DepartmentId == entity.DepartmentId))
            {
                throw new ArgumentException(nameof(entity.InventoryId) + " does not exciest");
            }
            await _context.InventoryItems.AddAsync(entity);
        }
        public async Task<PagedList<InventoryItem>> GetAsPagedList(int inventoryId, Pagination pagination)
        {
            if (pagination == null)
            {
                throw new ArgumentNullException(nameof(pagination));
            }

            pagination.PreparePaging();
            var source = GetAll();
            source.Where(x => x.InventoryId == inventoryId);
            if (pagination.CategoryId != null)
            {
                if (await _context.Categories.AnyAsync(x => x.Id == pagination.CategoryId && _CurrentDepartment.Ids.Contains(x.DepartmentId)))
                {
                    source = source.Where(x => x.Item.CategoryId == pagination.CategoryId);
                }
            }
            if (!string.IsNullOrWhiteSpace(pagination.SearchQuery))
            {
                source = source.Where(x => x.Item.Brand.Contains(pagination.SearchQuery) || x.Item.Model.Contains(pagination.SearchQuery));
            }
            if (!string.IsNullOrWhiteSpace(pagination.OrderBy))
            {
                source = source.TryOrderBy(pagination.OrderBy);
            }
            source = source.GetIncludes();
            return await PagedList<InventoryItem>.CreateAsync(source, pagination);
        }
        public override async Task<InventoryItem> GetAsync(int OId)
        {
            return  await _context.InventoryItems.Include(i => i.Item).FirstOrDefaultAsync(x => _CurrentDepartment.Ids.Contains(x.DepartmentId) && x.Id == OId);
        }

        public Task<InventoryItem> GetItemFromBarcode(string barcode)
        {
            return _context.InventoryItems.GetIncludes().FirstOrDefaultAsync(x => _CurrentDepartment.Ids.Contains(x.DepartmentId) && x.Item.Barcode == barcode);
        }

        public Task<InventoryItem> GetWithIncludesAsync(int itemId)
        {
            return _context.InventoryItems.GetIncludes().FirstOrDefaultAsync(x => _CurrentDepartment.Ids.Contains(x.DepartmentId) && x.Id == itemId);
        }
    }
}
