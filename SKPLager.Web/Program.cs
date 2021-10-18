using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using SKPLager.Web.Factorys;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR.Client;
using SKPLager.Services.SignalR;
using SKPLager.Services.Exstensions;

namespace SKPLager.Web
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("#app");

            builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
            
            builder.Services.AddOidcAuthentication(options =>
            {
                // Configure your authentication provider options here.
                // For more information, see https://aka.ms/blazor-standalone-auth
                builder.Configuration.Bind("oidc", options.ProviderOptions);
                options.UserOptions.RoleClaim = "roles";
            }).AddAccountClaimsPrincipalFactory<ArrayClaimsPrincipalFactory<RemoteUserAccount>>();


            builder.Services.AddSingleton<InventoryHub>();
            builder.Services.AddSingleton(x => HubConnectionFactoryMaker.HubConnectionFactory(x, builder.Configuration["signalrURL"]));
            var host = builder.Build();
            var hubConnection = await host.Services.GetRequiredService<Task<HubConnection>>();
            if (hubConnection != null)
                builder.Services.AddSingleton(hubConnection);

            builder.Services.AddHttpServices("<api url>");

            await builder.Build().RunAsync();
        }
    }
}
