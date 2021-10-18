using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SKPLager.Web.Factorys
{
    public class HubConnectionFactoryMaker
    {
        

        public static async Task<HubConnection> HubConnectionFactory(IServiceProvider serviceProvider,  string url)
        {
            using var scope = serviceProvider.CreateScope();
            if (!(await scope.ServiceProvider.GetRequiredService<IAccessTokenProvider>().RequestAccessToken()).TryGetToken(out var token))
                return null;
            var connection = new HubConnectionBuilder().WithAutomaticReconnect().WithUrl(url, o =>
            {
                o.AccessTokenProvider = () => Task.FromResult(token.Value.ToString());
            }).Build();

            await connection.StartAsync();
            return connection;
        }
    }
}
