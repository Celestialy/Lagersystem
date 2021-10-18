using SKPLager.Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SKPLager.API.Services.Repos
{
    public interface IItemRepo : IRepo<Item, int>
    {
    }
}
