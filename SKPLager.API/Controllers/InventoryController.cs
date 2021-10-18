using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SKPLager.API.Services;
using SKPLager.API.Services.Repos;
using SKPLager.Shared.Models;
using SKPLager.Shared.Models.Enums;
using SKPLager.Shared.Models.Paging;
using SKPLager.Shared.Wrappers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text.Json.Serialization;
using System.Text.Json;
using SKPLager.Shared.Converters;
using SKPLager.Shared.Configurations;
using SKPLager.API.Configurations;
using SKPLager.API.Hubs;
using Microsoft.AspNetCore.SignalR;
using SKPLager.Shared.IHubs;
using SKPLager.API.Helpers;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SKPLager.API.Controllers
{
    [ApiController]
    [Authorize(Policy = Policies.IsAtleastInstructor)]
    public class InventoryController : ControllerBase
    {

        private readonly IInventoryRepo inventoryRepo;
        private readonly ICurrentUserDepartmentService currentUserDepartment;
        private readonly IHubContext<InventoryHub, IInventoryPushHub> _hubContext;
        public InventoryController(IInventoryRepo inventoryRepo, ICurrentUserDepartmentService currentUserDepartment, IHubContext<InventoryHub, IInventoryPushHub> hubContext)
        {
            this.inventoryRepo = inventoryRepo;
            this.currentUserDepartment = currentUserDepartment;
            _hubContext = hubContext;
        }
        [Authorize(Policy = Policies.IsAtleastInstructor)]
        [HttpPost(ApiRoutes.Inventory.Create)]
        public async Task<IActionResult> CreateInventory([FromBody] Inventory inventory)
        {
            if (!currentUserDepartment.IsInDepartment(inventory.DepartmentId))
            {
                return BadRequest("Not in department");
            }

            await inventoryRepo.AddAsync(inventory);
            await inventoryRepo.SaveAsync();
            await _hubContext.Clients.Group(Group.InventoriesPage(inventory.DepartmentId)).CreatedInventory(inventory);
            return Ok(inventory);
        }

        [Authorize(Policy = Policies.IsAtleastInstructor)]
        [HttpDelete(ApiRoutes.Inventory.Delete)]
        public async Task<IActionResult> DeleteInventory(int InventoryId)
        {
            if (!inventoryRepo.Any(x => x.Id == InventoryId && currentUserDepartment.Ids.Contains(x.DepartmentId)))
            {
                return BadRequest("Not in department");
            }
            var inventory = await inventoryRepo.GetAsync(InventoryId);
            var departmentId = inventory.DepartmentId;
            inventoryRepo.Remove(inventory);
            await inventoryRepo.SaveAsync();
            await _hubContext.Clients.Groups(Group.InventoriesPage(departmentId), Group.InventoryPage(InventoryId)).DeletedInventory(InventoryId);
            return Ok();
        }

        [Authorize(Policy = Policies.IsAtleastInventoryManager)]
        [HttpGet(ApiRoutes.Inventory.GetAll)]
        public async Task<IActionResult> GetInventories([FromQuery] Pagination pagination)
        {
            return Ok(await inventoryRepo.GetAsPagedList(pagination));
        }

        [Authorize(Policy = Policies.IsAtleastInventoryManager)]
        [HttpGet(ApiRoutes.Inventory.GetOne)]
        public async Task<IActionResult> GetInventory(int inventoryId)
        {
            if (!inventoryRepo.Any(x => x.Id == inventoryId && currentUserDepartment.Ids.Contains(x.DepartmentId)))
            {
                return BadRequest("Not in department");
            }
            return Ok(await inventoryRepo.GetAsync(inventoryId));
        }
    }
}
