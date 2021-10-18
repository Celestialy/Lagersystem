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
    internal class InventoryService : IInventoryService
    {
        private HttpClient client;
        public InventoryService(HttpClient client)
        {
            this.client = client;
        }

        public async Task Create(Inventory inventory)
        {
            try
            {
                await client.PostAsJsonAsync<Inventory>(ApiRoutes.Inventory.Create, inventory);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        public async Task Delete(int id)
        {
            try
            {
                await client.DeleteAsync(ApiRoutes.Inventory.Delete(id));
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        public async Task<PagedList<Inventory>> GetAll(Pagination pagination)
        {
            try
            {
                return await client.GetFromJsonAsync<PagedList<Inventory>>(ApiRoutes.Inventory.GetAll + pagination.AsQuery());
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return null;
            }
        }

        public async Task<Inventory> GetOne(int id)
        {
            try
            {
                return await client.GetFromJsonAsync<Inventory>(ApiRoutes.Inventory.GetOne(id));
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return null;
            }
        }
    }
}
