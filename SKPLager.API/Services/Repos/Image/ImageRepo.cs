using Microsoft.EntityFrameworkCore;
using SKPLager.API.Database;
using SKPLager.API.Helpers;
using SKPLager.Shared.Models;
using SKPLager.Shared.Models.Paging;
using SKPLager.Shared.Wrappers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SKPLager.API.Services.Repos
{
    public class ImageRepo : Repo<Image, int>, IImageRepo
    {
        public ImageRepo(DBContext context, ICurrentUserDepartmentService currentDepartment) : base(context, currentDepartment)
        {
        }

        public override IQueryable<Image> GetAll()
        {
            return _context.Images.Where(x => _CurrentDepartment.Ids.Contains(x.DepartmentId) && !x.IsDeleted);
        }

        public async Task<PagedList<Image>> GetAsPagedList(Pagination pagination)
        {
            if (pagination == null)
            {
                throw new ArgumentNullException(nameof(pagination));
            }
            pagination.PreparePaging();
            var source = GetAll();
            if (!string.IsNullOrWhiteSpace(pagination.SearchQuery))
            {
                source = source.Where(x => x.Name.Contains(pagination.SearchQuery));
            }
            if (!string.IsNullOrWhiteSpace(pagination.OrderBy))
            {
                source = source.TryOrderBy<Image>(pagination.OrderBy);
            }
            if (pagination.DepartmentId != null)
            {
                if (_CurrentDepartment.IsInDepartment(pagination.DepartmentId.Value))
                {
                    source = source.Where(x => x.DepartmentId == pagination.DepartmentId.Value);
                }
                else
                {
                    throw new ArgumentOutOfRangeException(nameof(pagination.DepartmentId));
                }
            }
            return await PagedList<Image>.CreateAsync(source, pagination);
        }

        public override async Task<Image> GetAsync(int OId)
        {
            try
            {
                return await _context.Images.FirstOrDefaultAsync(x => x.Id == OId && _CurrentDepartment.Ids.Contains(x.DepartmentId));
            }
            catch (Exception)
            {
                return null;
            }
        }
        
        public override void Remove(Image entity)
        {
            entity.IsDeleted = true;
        }
    }
}
