
using SKPLager.Shared.Models.Paging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using System.Text.Json.Serialization;
using SKPLager.Shared.Converters;
using Microsoft.EntityFrameworkCore;

namespace SKPLager.Shared.Wrappers
{
    [JsonConverter(typeof(PagedListJsonConverterFactory))]
    public class PagedList<T> : List<T>
    {       
        public bool HasPrevious => (this.Paging.currentPage > 1);
        public bool HasNext => (this.Paging.currentPage < this.Paging.TotalPages);
        public Pagination Paging { get; set; }
        T[] Items
        {
            get
            {
                return this.ToArray();
            }
            set
            {
                if (value != null)
                {
                    this.AddRange(value);
                }
            }
        }
        public PagedList()
        {

        }
        public PagedList(List<T> items, Pagination pagination)
        {
            Paging = pagination;
            AddRange(items);
        }

        public static PagedList<T> Create(IQueryable<T> source, Pagination pagination)
        {
            pagination.TotalCount = source.Count();
            pagination.TotalPages = (int)Math.Ceiling(pagination.TotalCount / (double)pagination.pageSize);
            pagination.currentPage = pagination.currentPage <= pagination.TotalPages ? pagination.currentPage : pagination.TotalPages;
            var items = pagination.pageSize > 0 ? source.Skip((pagination.currentPage - 1) * pagination.pageSize).Take(pagination.pageSize).ToList() : source.ToList();
            return new PagedList<T>(items, pagination);
        }

        public static async Task<PagedList<T>> CreateAsync(IQueryable<T> source, Pagination pagination)
        {
            pagination.TotalCount = source.Count();
            pagination.TotalPages = (int)Math.Ceiling(pagination.TotalCount / (double)pagination.pageSize);
            pagination.currentPage = pagination.currentPage <= pagination.TotalPages ? pagination.currentPage : pagination.TotalPages != 0? pagination.TotalPages : 1;
            var items = pagination.pageSize > 0 ? await source.Skip((pagination.currentPage - 1) * pagination.pageSize).Take(pagination.pageSize).ToListAsync() : await source.ToListAsync();
            return new PagedList<T>(items, pagination);
        }

    }
}
