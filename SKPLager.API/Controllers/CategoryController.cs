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
using SKPLager.Shared.Models.Paging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SKPLager.API.Controllers
{
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryRepo categoryRepo;
        private readonly IInventoryRepo inventoryRepo;
        private readonly ICurrentUserDepartmentService currentUserDepartment;
        private readonly IHubContext<InventoryHub, IInventoryPushHub> _hubContext;

        public CategoryController(IHubContext<InventoryHub, IInventoryPushHub> hubContext, ICurrentUserDepartmentService currentUserDepartment, ICategoryRepo categoryRepo, IInventoryRepo inventoryRepo)
        {
            _hubContext = hubContext;
            this.currentUserDepartment = currentUserDepartment;
            this.categoryRepo = categoryRepo;
            this.inventoryRepo = inventoryRepo;
        }

        [Authorize(Policy = Policies.IsAtleastInventoryManager)]
        [HttpPost(ApiRoutes.Category.Create)]
        public async Task<IActionResult> CreateCategory([FromBody] Category category)
        {
            if (!currentUserDepartment.IsInDepartment(category.DepartmentId))
            {
                return BadRequest("Not in department");
            }

            await categoryRepo.AddAsync(category);
            await categoryRepo.SaveAsync();
            await _hubContext.Clients.Group(Group.Categories(category.DepartmentId)).CreatedCategory(category);
            return Ok();        
        }

        [Authorize(Policy = Policies.IsAtleastInventoryManager)]
        [HttpDelete(ApiRoutes.Category.Delete)]
        public async Task<IActionResult> DeleteCategory(int categoryId)
        {

            if (!categoryRepo.Any(x => x.Id == categoryId && currentUserDepartment.Ids.Contains(x.DepartmentId)))
            {
                return BadRequest("Not in department");
            }
            var category = await categoryRepo.GetAsync(categoryId);
            var departmentId = category.DepartmentId;
            categoryRepo.Remove(category);
            await categoryRepo.SaveAsync();
            await _hubContext.Clients.Groups(Group.Categories(departmentId)).DeletedCategory(categoryId);
            return Ok();
        }

        [Authorize(Policy = Policies.IsAtleastInventoryManager)]
        [HttpGet(ApiRoutes.Category.GetAll)]
        public async Task<IActionResult> GetCategories([FromQuery] Pagination pagination)
        {
            return Ok(await categoryRepo.GetAsPagedList(pagination));
        }

        [Authorize(Policy = Policies.IsAtleastInventoryManager)]
        [HttpGet(ApiRoutes.Category.GetOne)]
        public async Task<IActionResult> GetCategory(int categoryId)
        {
            if (!categoryRepo.Any(x => x.Id == categoryId && currentUserDepartment.Ids.Contains(x.DepartmentId)))
            {
                return BadRequest("Not in department");
            }
            var category = await categoryRepo.GetAsync(categoryId);
            return Ok(category);
        }

        [Authorize(Policy = Policies.IsAtleastInventoryManager)]
        [HttpGet(ApiRoutes.Inventory.Category.GetAll)]
        public async Task<IActionResult> GetCategorysUsedInInventory(int inventoryId)
        {
            if (!inventoryRepo.Any(x => x.Id == inventoryId && currentUserDepartment.Ids.Contains(x.DepartmentId)))
            {
                return BadRequest("Not in department");
            }
            var categories = await categoryRepo.GetCategoriesFromInventory(inventoryId);
            return Ok(categories);
        }
    }
}
