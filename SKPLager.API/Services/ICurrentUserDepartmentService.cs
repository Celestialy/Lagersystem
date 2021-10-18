using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SKPLager.API.Services
{
    public interface ICurrentUserDepartmentService
    {
        /// <summary>
        /// Ids of all users departments
        /// </summary>
        public List<int> Ids { get;}

        public bool IsInDepartment(int departmentId);
    }
}
