using MatBlazor;
using Microsoft.AspNetCore.Components.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SKPLager.Web.Shared
{
    public partial class MainLayout
    {
        public static bool testDrawer { get; set; }

        MatTheme MatTheme = new MatTheme()
        {
            Primary = "#448AFF",
            Secondary = "#448AFF"
        };

        private async Task BeginSignOut(MouseEventArgs args)
        {
            await SignOutManager.SetSignOutState();
            Navigation.NavigateTo("authentication/logout");
        }
    }
}