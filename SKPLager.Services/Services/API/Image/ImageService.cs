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
    internal class ImageService : IImageService
    {
        private HttpClient client;

        public ImageService(HttpClient client)
        {
            this.client = client;
        }

        public async Task Create(Image image)
        {
            try
            {
                await client.PostAsJsonAsync<Image>(ApiRoutes.Image.Create, image);
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
                await client.DeleteAsync(ApiRoutes.Image.Delete(id));
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        public async Task<PagedList<Image>> GetAll(Pagination pagination)
        {
            try
            {
                return await client.GetFromJsonAsync<PagedList<Image>>(ApiRoutes.Image.GetAll + pagination.AsQuery());
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return null;
            }
        }

        public async Task<Image> GetOne(int id)
        {
            try
            {
                return await client.GetFromJsonAsync<Image>(ApiRoutes.Image.GetOne(id));
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return null;
            }
        }
    }
}
