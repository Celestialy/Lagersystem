using SKPLager.Shared.Models.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace SKPLager.Shared.Helpers
{
    public static class ClaimsPrincipalExstension
    {
        public static bool IsInventoryManagerOrAbove(this ClaimsPrincipal principal) => principal.IsInRole(Roles.Administrator) || principal.IsInRole(Roles.Instructor) || principal.IsInRole(Roles.InventoryManager);

        /// <summary>
        /// Gets a user object from current user
        /// </summary>
        /// <param name="principal"></param>
        /// <returns></returns>
        public static User ToUser(this ClaimsPrincipal principal)
        {
            User user = new User();
            user.Id = principal.FindFirst("ID").Value;
            user.Name = principal.FindFirst("name").Value;
            user.FirstName = principal.FindFirst("given_name").Value;
            user.LastName = principal.FindFirst("family_name").Value;
            user.Mail = principal.FindFirst("email").Value;
            user.CardId = principal.FindFirst("cardid").Value;
            user.Roles = principal.FindAll("roles").Select(x => new Role { Name = x.Value }).ToList();
            user.Departments = principal.FindAll("department").Select(x => new Department { Id = int.Parse(x.Value.Split(':')[0]), Name = x.Value.Split(':')[1] }).ToList<Department>();
            return user;
        }

        public static List<Department> ToDepartments(this ClaimsPrincipal principal)
        {
            return principal.FindAll("department").Select(x => new Department { Id = int.Parse(x.Value.Split(':')[0]), Name = x.Value.Split(':')[1] }).ToList<Department>();
        }
        /// <summary>
        /// Get user id from the claims
        /// </summary>
        /// <param name="principal">The Claims</param>
        /// <returns>Return id</returns>
        public static string GetUserId(this ClaimsPrincipal principal)
        {
            return principal.FindFirst(ClaimTypes.NameIdentifier).Value;
        }

    }
}
