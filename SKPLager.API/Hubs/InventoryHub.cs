using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using SKPLager.API.Configurations;
using SKPLager.API.Helpers;
using SKPLager.API.Services;
using SKPLager.API.Services.Repos;
using SKPLager.Shared.Configurations;
using SKPLager.Shared.Helpers;
using SKPLager.Shared.IHubs;
using SKPLager.Shared.Models;
using SKPLager.Shared.Models.Paging;
using SKPLager.Shared.Wrappers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SKPLager.API.Hubs
{
    public class InventoryHub : Hub<IInventoryPushHub>, IInventoryInvokeMethods
    {
        private readonly IInventoryRepo inventoryRepo;
        private readonly IInventoryItemRepo itemRepo;
        private readonly IImageRepo imageRepo;
        private readonly ICategoryRepo categoryRepo;
        private readonly IConsumptionItemRepo consumptionItemRepo;
        private readonly ILoanItemRepo loanItemRepo;
        private readonly ICurrentUserDepartmentService currentUserDepartment;

        public InventoryHub(IInventoryRepo inventoryRepo, IInventoryItemRepo itemRepo, IImageRepo imageRepo, ICategoryRepo categoryRepo, IConsumptionItemRepo consumptionItemRepo, ILoanItemRepo loanItemRepo, ICurrentUserDepartmentService currentUserDepartment)
        {
            this.inventoryRepo = inventoryRepo;
            this.itemRepo = itemRepo;
            this.imageRepo = imageRepo;
            this.categoryRepo = categoryRepo;
            this.consumptionItemRepo = consumptionItemRepo;
            this.loanItemRepo = loanItemRepo;
            this.currentUserDepartment = currentUserDepartment;
        }

        public override Task OnConnectedAsync()
        {
            var user = Context;
            return base.OnConnectedAsync();
        }

        #region Groups
        [Authorize(Policy = Policies.AllAccess)]
        public async Task AddToGroup(string groupName)
        {
            if (!groupName.StartsWith("Public"))
            {
                if (Context.User.IsInventoryManagerOrAbove())
                {
                    await Groups.AddToGroupAsync(Context.ConnectionId, groupName);
                }
            }
            else
            {
                await Groups.AddToGroupAsync(Context.ConnectionId, groupName);
            }
        }


        [Authorize(Policy = Policies.AllAccess)]
        public async Task RemoveFromGroup(string groupName)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, groupName);
        }
        //Todo make phone linking safer
        public async Task LinkWithPhone(string code)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, code);
        }

        public async Task DisconnectFromPhone(string code)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, code);
        }
        #endregion
        #region Inventory
        [Authorize(Policy = Policies.IsAtleastInventoryManager)]
        public async Task GetInventories(Pagination pagination)
        {
            await Clients.Caller.PagedInventories(await inventoryRepo.GetAsPagedList(pagination));
        }
        #endregion
        #region Image
        [Authorize(Policy = Policies.IsAtleastInventoryManager)]
        public async Task GetImages(Pagination pagination)
        {
            await Clients.Caller.PagedImages(await imageRepo.GetAsPagedList(pagination));
        }
        #endregion
        #region Category
        [Authorize(Policy = Policies.IsAtleastInventoryManager)]
        public async Task GetCategories(Pagination pagination)
        {
            await Clients.Caller.PagedCategories(await categoryRepo.GetAsPagedList(pagination));
        }
        #endregion
        #region Item
        [Authorize(Policy = Policies.IsAtleastInventoryManager)]
        public async Task GetItems(int inventoryId,Pagination pagination)
        {
            await Clients.Caller.PagedItems(await itemRepo.GetAsPagedList(inventoryId, pagination));
        }

        public async Task GetItemFromBarcode(string linkCode, string barcode)
        {
            await Clients.Group(linkCode).ScannedItem(await itemRepo.GetItemFromBarcode(barcode));
        }
        public async Task ScannedBarcode(string linkCode, string barcode)
        {
            await Clients.Group(linkCode).ScannedItem(await itemRepo.GetItemFromBarcode(barcode));
        }
        #endregion
        #region Loan
        [Authorize(Policy = Policies.IsAtleastInventoryManager)]
        public async Task GetLoans(int inventoryId, Pagination pagination)
        {
            await Clients.Caller.PagedLoans(await loanItemRepo.GetAsPagedList(inventoryId, pagination));
        }
        [Authorize(Policy = Policies.AllAccess)]
        public async Task GetLoanHistory(int inventoryId, Pagination pagination)
        {
            await Clients.Caller.PagedLoans(await loanItemRepo.GetUserHistory(inventoryId, Context.UserIdentifier, pagination));
        }
        [Authorize(Policy = Policies.IsAtleastInventoryManager)]
        public async Task GetUserLoanHistory(int inventoryId, string userId, Pagination pagination)
        {
            await Clients.Caller.PagedLoans(await loanItemRepo.GetUserHistory(inventoryId, userId, pagination));
        }

        #endregion
        #region Consumption
        [Authorize(Policy = Policies.IsAtleastInventoryManager)]
        public async Task GetConsumptions(int inventoryId, Pagination pagination)
        {
            await Clients.Caller.PagedConsumptions(await consumptionItemRepo.GetAsPagedList(inventoryId, pagination));
        }
        [Authorize(Policy = Policies.AllAccess)]
        public async Task GetConsumptionHistory(int inventoryId, Pagination pagination)
        {
            await Clients.Caller.PagedConsumptions(await consumptionItemRepo.GetUserHistory(inventoryId, Context.UserIdentifier, pagination));
        }
        [Authorize(Policy = Policies.IsAtleastInventoryManager)]
        public async Task GetUserConsumptionHistory(int inventoryId, string userId, Pagination pagination)
        {
            await Clients.Caller.PagedConsumptions(await consumptionItemRepo.GetUserHistory(inventoryId, userId, pagination));
        }
        #endregion
    }
}
