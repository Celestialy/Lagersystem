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
    public class ConsumptionItemRepo : Repo<ConsumptionItem, int>, IConsumptionItemRepo
    {
        public ConsumptionItemRepo(DBContext context, ICurrentUserDepartmentService currentDepartment) : base(context, currentDepartment)
        {

        }

        public override async Task AddAsync(ConsumptionItem entity)
        {
            if (entity.ItemId == 0)
            {
                throw new ArgumentOutOfRangeException(nameof(entity.Item));
            }
            else if (!_context.Items.Any(x => x.Id == entity.ItemId && x.DepartmentId == entity.DepartmentId))
            {
                throw new ArgumentException(nameof(entity.ItemId) + " does not exciest");
            }
            else if (!_context.InventoryItems.Any(x => x.ItemId == entity.ItemId && x.Amount >= entity.Amount))
            {
                throw new ArgumentException(nameof(entity.ItemId) + " not enough items");
            }
            else if (!_context.Inventories.Any(x => x.Id == entity.InventoryId && x.DepartmentId == entity.DepartmentId))
            {
                throw new ArgumentException(nameof(entity.InventoryId) + " does not exciest");
            }
            var item = await _context.InventoryItems.Where(x => x.ItemId == entity.ItemId).FirstOrDefaultAsync();
            item.Amount--;
            item.TotalAmount--;
            item.UpdatedAt = DateTime.UtcNow;
            _context.Update(item);
            await _context.ConsumptionItems.AddAsync(entity);
        }

        public async Task<PagedList<ConsumptionItem>> GetAsPagedList(int inventoryId, Pagination pagination)
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
            return await PagedList<ConsumptionItem>.CreateAsync(source, pagination);
        }

        public async Task<PagedList<ConsumptionItem>> GetUserHistory(int inventoryId, string userId, Pagination pagination)
        {
            if (pagination == null)
            {
                throw new ArgumentNullException(nameof(pagination));
            }

            pagination.PreparePaging();
            var source = GetAll();
            source.Where(x => x.InventoryId == inventoryId);
            source.Where(x => x.UserId == userId);
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
            return await PagedList<ConsumptionItem>.CreateAsync(source, pagination);
        }

        public Task<ConsumptionItem> GetWithIncludesAsync(int itemId)
        {
            return _context.ConsumptionItems.GetIncludes().FirstOrDefaultAsync(x => _CurrentDepartment.Ids.Contains(x.DepartmentId) && x.Id == itemId);
        }
    }
}
