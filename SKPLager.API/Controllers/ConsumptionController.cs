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
using SKPLager.Shared.Helpers;
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
    public class ConsumptionController : ControllerBase
    {
        private readonly IInventoryRepo inventoryRepo;
        private readonly IConsumptionItemRepo consumptionRepo;
        private readonly ICurrentUserDepartmentService currentUserDepartment;
        private readonly IHubContext<InventoryHub, IInventoryPushHub> _hubContext;
        private readonly IMapper _mapper;

        public ConsumptionController(IHubContext<InventoryHub, IInventoryPushHub> hubContext, ICurrentUserDepartmentService currentUserDepartment, IMapper mapper, IInventoryRepo inventoryRepo, IConsumptionItemRepo consumptionRepo)
        {
            _hubContext = hubContext;
            this.currentUserDepartment = currentUserDepartment;
            _mapper = mapper;
            this.inventoryRepo = inventoryRepo;
            this.consumptionRepo = consumptionRepo;
        }

        [Authorize(Policy = Policies.IsAtleastInventoryManager)]
        [HttpPost(ApiRoutes.Inventory.Consumption.Create)]
        public async Task<IActionResult> CreateConsumption(int inventoryId, [FromBody] CreateConsumptionDTO create)
        {
            if (!currentUserDepartment.IsInDepartment(create.DepartmentId))
            {
                return BadRequest("Not in department");
            }
            if (create.ItemId == 0)
            {
                return BadRequest("No item");
            }
            var item = _mapper.Map<ConsumptionItem>(create);
            await consumptionRepo.AddAsync(item);
            await consumptionRepo.SaveAsync();
            await _hubContext.Clients.Group(Group.InventoryPage(inventoryId)).CreatedConsumption(item);
            return Ok(item);
        }

        [Authorize(Policy = Policies.IsAtleastInventoryManager)]
        [HttpDelete(ApiRoutes.Inventory.Consumption.Delete)]
        public async Task<IActionResult> DeleteConsumption(int inventoryId, int consumptionId)
        {
            if (await checkParameter(inventoryId, consumptionId))
            {
                return BadRequest("Not in department");
            }
            consumptionRepo.Remove(await consumptionRepo.GetAsync(consumptionId));
            await consumptionRepo.SaveAsync();
            await _hubContext.Clients.Groups(Group.InventoryPage(inventoryId)).DeletedLoan(consumptionId);
            return Ok(consumptionId);
        }


        [Authorize(Policy = Policies.IsAtleastInventoryManager)]
        [HttpGet(ApiRoutes.Inventory.Consumption.GetAll)]
        public async Task<IActionResult> GetConsumptions(int inventoryId, [FromQuery] Pagination pagination)
        {
            return Ok(await consumptionRepo.GetAsPagedList(inventoryId, pagination));
        }

        [Authorize(Policy = Policies.AllAccess)]
        [HttpGet(ApiRoutes.Inventory.Consumption.User.GetHistory)]
        public async Task<IActionResult> GetUserConsumptionHistory(int inventoryId, [FromQuery] Pagination pagination)
        {
            return Ok(await consumptionRepo.GetUserHistory(inventoryId, User.GetUserId(), pagination));
        }
        [Authorize(Policy = Policies.IsAtleastInventoryManager)]
        [HttpGet(ApiRoutes.Inventory.Consumption.User.GetUserHistory)]
        public async Task<IActionResult> GetUserConsumptionHistory(int inventoryId, string userId, [FromQuery] Pagination pagination)
        {
            return Ok(await consumptionRepo.GetUserHistory(inventoryId, userId, pagination));
        }

        [Authorize(Policy = Policies.IsAtleastInventoryManager)]
        [HttpGet(ApiRoutes.Inventory.Consumption.GetOne)]
        public async Task<IActionResult> GetConsumption(int inventoryId, int loanId)
        {
            if (await checkParameter(inventoryId, loanId))
            {
                return BadRequest("Not in department");
            }
            var item = await consumptionRepo.GetWithIncludesAsync(loanId);
            return Ok(item);
        }

        private async Task<bool> checkParameter(int inventoryId, int consumptionId) => !await inventoryRepo.AnyAsync(x => x.Id == inventoryId && currentUserDepartment.Ids.Contains(x.DepartmentId) && x.Consumptions.Any(y => y.Id == consumptionId));
    }
}
