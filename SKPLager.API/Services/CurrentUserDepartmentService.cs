using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SKPLager.API.Services
{
    public class CurrentUserDepartmentService : ICurrentUserDepartmentService
    {
        public List<int> Ids { get; private set; }

        public CurrentUserDepartmentService(IHttpContextAccessor httpContextAccessor)
        {            
            try
            {
                Ids = httpContextAccessor.HttpContext.User.Claims.Where(e => e.Type == "department").Select(x => int.Parse(x.Value.Split(':')[0])).ToList<int>();
            }
            catch (Exception)
            {

                Ids = new List<int>();
            }
        }

        public bool IsInDepartment(int departmentId)
        {
            return Ids.Any(x => x == departmentId);
        }
    }
}
