using SKPLager.Shared.Models;
using SKPLager.Shared.Models.Paging;
using SKPLager.Shared.Wrappers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace SKPLager.Services.API
{
    internal class ItemService : IItemService
    {
        private HttpClient client;

        public ItemService(HttpClient client)
        {
            this.client = client;
        }

        public async Task Create(int inventoryId, InventoryItem inventory)
        {
            try
            {
                await client.PostAsJsonAsync<InventoryItem>(ApiRoutes.Inventory.Item.Create(inventoryId), inventory);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        public async Task Delete(int inventoryId, int itemId)
        {
            try
            {
                await client.DeleteAsync(ApiRoutes.Inventory.Item.Delete(inventoryId, itemId));
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        public async Task<PagedList<InventoryItem>> GetAll(int inventoryId, Pagination pagination)
        {
            try
            {
                return await client.GetFromJsonAsync<PagedList<InventoryItem>>(ApiRoutes.Inventory.Item.GetAll(inventoryId) + pagination.AsQuery());
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return null;
            }
        }

        public async Task<InventoryItem> GetOne(int inventoryId, int itemId)
        {
            try
            {
                return await client.GetFromJsonAsync<InventoryItem>(ApiRoutes.Inventory.Item.GetOne(inventoryId, itemId));
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return null;
            }
        }

        public async Task Update(int inventoryId, int itemId, InventoryItem item)
        {
            try
            {
                await client.PutAsJsonAsync<InventoryItem>(ApiRoutes.Inventory.Item.Update(inventoryId, itemId), item);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

    }
}
