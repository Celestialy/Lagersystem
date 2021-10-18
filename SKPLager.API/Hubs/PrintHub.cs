using Microsoft.AspNetCore.SignalR;
using SKPLager.Shared.IHubs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SKPLager.API.Hubs
{
    public class PrintHub : Hub<IPrintHub>
    {
        public async Task AddToDepartment(int departmentId)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, departmentId.ToString());
        }
    }
}
