using SKPLager.API.Database;
using SKPLager.Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SKPLager.API.Services.Repos
{
    public class ItemRepo : Repo<Item, int>, IItemRepo
    {
        public ItemRepo(DBContext context, ICurrentUserDepartmentService currentDepartment) : base(context, currentDepartment)
        {
        }
    }
}
