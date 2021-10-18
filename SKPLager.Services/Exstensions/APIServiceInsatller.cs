using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Microsoft.Extensions.DependencyInjection;
using SKPLager.Services.API;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SKPLager.Services.Exstensions
{
    public static class APIServiceInsatller
    {

        /// <summary>
        /// Adds all our api services
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddHttpServices(this IServiceCollection services, string url)
        {
            
            services.AddHttpClient("unauth", x => x.BaseAddress = new Uri(url));
            services.AddHttpClient("auth", x => x.BaseAddress = new Uri(url)).AddHttpMessageHandler(sp =>
            {
                var handler = sp.GetService<AuthorizationMessageHandler>()
                       .ConfigureHandler(
                           authorizedUrls: new[] { url });
                return handler;
            });
            services.AddHttpClient<IInventoryService, InventoryService>("auth");
            services.AddHttpClient<IImageService, ImageService>("auth");
            services.AddHttpClient<ICategoryService, CategoryService>("auth");
            services.AddHttpClient<IItemService, ItemService>("auth");
            return services;
        }
    }
}
