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
    public class LoanController : ControllerBase
    {
        private readonly IInventoryRepo inventoryRepo;
        private readonly ILoanItemRepo loanRepo;
        private readonly ICurrentUserDepartmentService currentUserDepartment;
        private readonly IHubContext<InventoryHub, IInventoryPushHub> _hubContext;
        private readonly IMapper _mapper;

        public LoanController(IHubContext<InventoryHub, IInventoryPushHub> hubContext, ICurrentUserDepartmentService currentUserDepartment, ILoanItemRepo loanRepo, IMapper mapper, IInventoryRepo inventoryRepo)
        {
            _hubContext = hubContext;
            this.currentUserDepartment = currentUserDepartment;
            this.loanRepo = loanRepo;
            _mapper = mapper;
            this.inventoryRepo = inventoryRepo;
        }

        [Authorize(Policy = Policies.IsAtleastInventoryManager)]
        [HttpPost(ApiRoutes.Inventory.Loan.Create)]
        public async Task<IActionResult> CreateLoan(int inventoryId ,[FromBody] CreateLoanDTO loan)
        {
            if (!currentUserDepartment.IsInDepartment(loan.DepartmentId))
            {
                return BadRequest("Not in department");
            }
            if (loan.ItemId == 0)
            {
                return BadRequest("No item");
            }
            var item = _mapper.Map<LoanItem>(loan);
            await loanRepo.AddAsync(item);
            await loanRepo.SaveAsync();
            await _hubContext.Clients.Group(Group.InventoryPage(inventoryId)).CreatedLoan(item);
            return Ok(item);
        }

        [Authorize(Policy = Policies.IsAtleastInstructor)]
        [HttpDelete(ApiRoutes.Inventory.Loan.Delete)]
        public async Task<IActionResult> DeleteLoan(int inventoryId, int loanId)
        {
            if (await checkParameter(inventoryId, loanId))
            {
                return BadRequest("Not in department");
            }
            loanRepo.Remove(await loanRepo.GetAsync(loanId));
            await loanRepo.SaveAsync();
            await _hubContext.Clients.Groups(Group.InventoryPage(inventoryId)).DeletedLoan(loanId);
            return Ok(loanId);
        }

        [Authorize(Policy = Policies.IsAtleastInventoryManager)]
        [HttpPut(ApiRoutes.Inventory.Loan.Return)]
        public async Task<IActionResult> ReturnLoan(int inventoryId, int loanId, ReturnLoanDTO loan)
        {
            if (await checkParameter(inventoryId, loanId))
            {
                return BadRequest("Not in department");
            }
            if (loan.Id != loanId)
                return BadRequest("Item Ids doesnt match");

            var LoanToReturn= await loanRepo.GetAsync(loanId);
            if (LoanToReturn.UserId != loan.UserId)
            {
                return BadRequest("Cant edit user");
            }
            if (!LoanToReturn.IsReturned && loan.IsReturned)
            {
                await loanRepo.ReturnItem(LoanToReturn.ItemId);
            }
            _mapper.Map(loan, LoanToReturn);
            loanRepo.Update(LoanToReturn);
            await loanRepo.SaveAsync();
            await _hubContext.Clients.Groups(Group.InventoryPage(inventoryId)).UpdatedLoan(LoanToReturn);
            return Ok(LoanToReturn);
        }

        [Authorize(Policy = Policies.IsAtleastInventoryManager)]
        [HttpGet(ApiRoutes.Inventory.Loan.GetAll)]
        public async Task<IActionResult> GetLoans(int inventoryId, [FromQuery] Pagination pagination)
        {
            return Ok(await loanRepo.GetAsPagedList(inventoryId, pagination));
        }

        [Authorize(Policy = Policies.AllAccess)]
        [HttpGet(ApiRoutes.Inventory.Loan.User.GetHistory)]
        public async Task<IActionResult> GetUserLoanHistory(int inventoryId, [FromQuery] Pagination pagination)
        {            
            return Ok(await loanRepo.GetUserHistory(inventoryId, User.GetUserId(), pagination));
        }
        [Authorize(Policy = Policies.IsAtleastInventoryManager)]
        [HttpGet(ApiRoutes.Inventory.Loan.User.GetUserHistory)]
        public async Task<IActionResult> GetUserLoanHistory(int inventoryId, string userId, [FromQuery] Pagination pagination)
        {
            return Ok(await loanRepo.GetUserHistory(inventoryId, userId, pagination));
        }

        [Authorize(Policy = Policies.IsAtleastInventoryManager)]
        [HttpGet(ApiRoutes.Inventory.Loan.GetOne)]
        public async Task<IActionResult> GetLoan(int inventoryId, int loanId)
        {
            if (await checkParameter(inventoryId, loanId))
            {
                return BadRequest("Not in department");
            }
            var item = await loanRepo.GetWithIncludesAsync(loanId);
            return Ok(item);
        }

        private async Task<bool> checkParameter(int inventoryId, int loanId) => !await inventoryRepo.AnyAsync(x => x.Id == inventoryId && currentUserDepartment.Ids.Contains(x.DepartmentId) && x.Loans.Any(y => y.Id == loanId));
    }
}
