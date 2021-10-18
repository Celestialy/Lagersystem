using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SKPLager.Web.Pages
{

    public partial class Index
    {
        [Inject]
        NavigationManager Navigation { get; set; }

        protected override void OnInitialized()
        {
            Navigation.NavigateTo("/inventory/TestUdlån");
        }
    }
}