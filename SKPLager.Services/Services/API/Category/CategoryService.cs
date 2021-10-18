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
    internal class CategoryService : ICategoryService
    {
        private HttpClient client;

        public CategoryService(HttpClient client)
        {
            this.client = client;
        }

        public async Task Create(Category category)
        {
            try
            {
                await client.PostAsJsonAsync<Category>(ApiRoutes.Category.Create, category);
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
                await client.DeleteAsync(ApiRoutes.Category.Delete(id));
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        public async Task<PagedList<Category>> GetAll(Pagination pagination)
        {
            try
            {
                return await client.GetFromJsonAsync<PagedList<Category>>(ApiRoutes.Category.GetAll + pagination.AsQuery());
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return null;
            }
        }

        public async Task<List<Category>> GetAllFromInventory(int inventoryId)
        {
            try
            {
                return await client.GetFromJsonAsync<List<Category>>(ApiRoutes.Inventory.Category.GetAll(inventoryId));
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return null;
            }
        }

        public async Task<Category> GetOne(int id)
        {
            try
            {
                return await client.GetFromJsonAsync<Category>(ApiRoutes.Category.GetOne(id));
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return null;
            }
        }
    }
}
