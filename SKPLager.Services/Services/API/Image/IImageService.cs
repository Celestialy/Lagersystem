using SKPLager.Shared.Models;
using SKPLager.Shared.Models.Paging;
using SKPLager.Shared.Wrappers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SKPLager.Services.API
{
    public interface IImageService
    {
        /// <summary>
        /// Gets all images in paged
        /// </summary>
        /// <returns>Return All entity in paged</returns>
        Task<PagedList<Image>> GetAll(Pagination pagination);

        /// <summary>
        /// Get one image
        /// </summary>
        /// <returns>Return entity</returns>
        Task<Image> GetOne(int id);

        /// <summary>
        /// Create image
        /// </summary>
        /// <param name="image"></param>
        /// <returns></returns>
        Task Create(Image image);

        /// <summary>
        /// Delete image
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task Delete(int id);
    }
}
