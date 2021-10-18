using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SKPLager.API.Services.Repos;
using SKPLager.API.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SKPLager.API.Services.Storage;

namespace SKPLager.API.Installers
{
    public class ServicesInstaller : IInstaller
    {
        public void InstallServices(IServiceCollection services, IConfiguration configuration)
        {
            services.AddHttpContextAccessor();

            #region Repos
            services.AddScoped<IImageRepo, ImageRepo>();
            services.AddScoped<IInventoryRepo, InventoryRepo>();
            services.AddScoped<IItemRepo, ItemRepo>();
            services.AddScoped<IInventoryItemRepo, InventoryItemRepo>();
            services.AddScoped<ICategoryRepo, CategoryRepo>();
            services.AddScoped<ILoanItemRepo, LoanItemRepo>();
            services.AddScoped<IConsumptionItemRepo, ConsumptionItemRepo>();
            #endregion

            services.AddScoped<ICurrentUserDepartmentService, CurrentUserDepartmentService>();

            services.AddScoped<IStorageService, StorageSerivce>();
        }
    }
}
