using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using SKPLager.API.Configurations;
using SKPLager.API.Hubs;
using SKPLager.API.Services;
using SKPLager.API.Services.Repos;
using SKPLager.Shared.Configurations;
using SKPLager.Shared.IHubs;
using SKPLager.Shared.Models;
using SKPLager.Shared.Models.DTOs;
using SKPLager.Shared.Models.Paging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SKPLager.API.Controllers
{
    [ApiController]
    public class ItemController : ControllerBase
    {
        private readonly IInventoryRepo inventoryRepo;
        private readonly IInventoryItemRepo itemRepo;
        private readonly ICurrentUserDepartmentService currentUserDepartment;
        private readonly IHubContext<InventoryHub, IInventoryPushHub> _hubContext;
        private readonly IMapper _mapper;
        public ItemController(IHubContext<InventoryHub, IInventoryPushHub> hubContext, ICurrentUserDepartmentService currentUserDepartment, IItemRepo itemRepo, IInventoryItemRepo inventoryItemRepo, IInventoryRepo inventoryRepo, IMapper mapper)
        {
            _hubContext = hubContext;
            this.currentUserDepartment = currentUserDepartment;
            this.itemRepo = inventoryItemRepo;
            this.inventoryRepo = inventoryRepo;
            _mapper = mapper;
        }

        [Authorize(Policy = Policies.IsAtleastInventoryManager)]
        [HttpPost(ApiRoutes.Inventory.Item.Create)]
        public async Task<IActionResult> CreateItem(int inventoryId, [FromBody] InventoryItem item)
        {
            if (!currentUserDepartment.IsInDepartment(item.DepartmentId))
            {
                return BadRequest("Not in department");
            }
            if (item.Item == null)
            {
                return BadRequest("No item");
            }

            await itemRepo.AddAsync(item);
            await itemRepo.SaveAsync();
            await _hubContext.Clients.Group(Group.InventoryPage(inventoryId)).CreatedItem(item);
            return Ok(item);
        }

        [Authorize(Policy = Policies.IsAtleastInventoryManager)]
        [HttpDelete(ApiRoutes.Inventory.Item.Delete)]
        public async Task<IActionResult> DeleteItem(int inventoryId, int itemId)
        {
            if (await checkParameter(inventoryId, itemId))
            {
                return BadRequest("Not in department");
            }
            itemRepo.Remove(await itemRepo.GetAsync(itemId));
            await itemRepo.SaveAsync();
            await _hubContext.Clients.Groups(Group.InventoryPage(inventoryId)).DeletedItem(itemId);
            return Ok();
        }

        [Authorize(Policy = Policies.IsAtleastInventoryManager)]
        [HttpPut(ApiRoutes.Inventory.Item.Update)]
        public async Task<IActionResult> UpdateItem(int inventoryId, int itemId, UpdateItemDTO item)
        {
            if (await checkParameter(inventoryId, itemId))
            {
                return BadRequest("Not in department");
            }
            if (item.Id != itemId)
                return BadRequest("Item Ids doesnt match");

            var ItemToUpdate = await itemRepo.GetAsync(itemId);
            _mapper.Map(item, ItemToUpdate);
            itemRepo.Update(ItemToUpdate);
            await itemRepo.SaveAsync();
            await _hubContext.Clients.Groups(Group.InventoryPage(inventoryId)).UpdatedItem(ItemToUpdate);
            return Ok(ItemToUpdate);
        }

        [Authorize(Policy = Policies.IsAtleastInventoryManager)]
        [HttpGet(ApiRoutes.Inventory.Item.GetAll)]
        public async Task<IActionResult> GetItems(int inventoryId, [FromQuery] Pagination pagination)
        {
            return Ok(await itemRepo.GetAsPagedList(inventoryId ,pagination));
        }

        [Authorize(Policy = Policies.IsAtleastInventoryManager)]
        [HttpGet(ApiRoutes.Inventory.Item.GetOne)]
        public async Task<IActionResult> GetItem(int inventoryId, int itemId)
        {
            if (await checkParameter(inventoryId, itemId))
            {
                return BadRequest("Not in department");
            }
            var item = await itemRepo.GetWithIncludesAsync(itemId);
            return Ok(item);
        }

        private async Task<bool> checkParameter(int inventoryId, int itemId) => !await inventoryRepo.AnyAsync(x => x.Id == inventoryId && currentUserDepartment.Ids.Contains(x.DepartmentId) && x.Items.Any(y => y.Id == itemId && currentUserDepartment.Ids.Contains(y.DepartmentId)));
    }
}
