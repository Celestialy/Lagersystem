using SKPLager.Shared.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace SKPLager.Shared.Models.Paging
{
    public class Pagination
    {
        //request
        /// <summary>
        /// Set number of items on page
        /// </summary>
        public int pageSize { get; set; } = 10;
        /// <summary>
        /// Sets Current page
        /// </summary>
        public int currentPage { get; set; } = 1;
        /// <summary>
        /// Sets what the list should be sorted after
        /// </summary>
        public string OrderBy { get; set; }
        /// <summary>
        /// Set the category you are sorting items after
        /// </summary>
        public int? CategoryId { get; set; }
        /// <summary>
        /// What fields you want to have shown
        /// </summary>
        [JsonIgnore]
        public string[] Fields { get; set; }
        /// <summary>
        /// Sets the search word for searching after items
        /// </summary>
        public string SearchQuery { get; set; }
        /// <summary>
        /// Sets what departments you want infomation from
        /// </summary>
        [JsonIgnore]
        public string[] Departments { get; set; }
        /// <summary>
        /// The Id of the Department you want infomation from
        /// </summary>
        public int? DepartmentId { get; set; }
        /// <summary>
        /// Sets what roles you want infomation from
        /// </summary>
        [JsonIgnore]
        public string[] Roles { get; set; }
        //response
        /// <summary>
        /// Gets the total number of items
        /// </summary>
        public int TotalCount { get; set; }
        /// <summary>
        /// Gets the total number of pages
        /// </summary>
        public int TotalPages { get; set; }

        /// <summary>
        /// Converts the model to query
        /// </summary>
        public string AsQuery()
        {
            string query = "?";
            if (pageSize > 0)
            {
                query += "pageSize=" + pageSize + "&";
            }
            if (currentPage > 0)
            {
                query += "currentPage=" + currentPage + "&";
            }
            if (!string.IsNullOrEmpty(OrderBy))
            {
                query += "OrderBy=" + OrderBy + "&";
            }
            if (!string.IsNullOrEmpty(SearchQuery))
            {
                query += "SearchQuery=" + SearchQuery + "&";
            }
            if (DepartmentId.HasValue)
            {
                query += "DepartmentId=" + DepartmentId.Value + "&";
            }
            if (CategoryId.HasValue)
            {
                query += "CategoryId=" + CategoryId.Value;
            }
            return query;
        }

        /// <summary>
        /// Converts the model to query
        /// </summary>
        public string AsQuery(bool toAuthApi)
        {
            if (!toAuthApi)
            {
                return AsQuery();
            }
            string query = "?";
            if (pageSize > 0)
            {
                query += "PageSize=" + pageSize + "&";
            }
            if (currentPage > 0)
            {
                query += "PageNumber=" + currentPage + "&";
            }
            if (!string.IsNullOrEmpty(OrderBy))
            {
                query += "OrderBy=" + OrderBy + "&";
            }
            if (!Fields.IsNullOrEmpty())
            {
                query += "Fields=" + string.Join(",", Fields) + "&";
            }
            if (!string.IsNullOrEmpty(SearchQuery))
            {
                query += "SearchQuery=" + SearchQuery + "&";
            }
            if (!Departments.IsNullOrEmpty())
            {
                query += "Departments=" + string.Join(",", Departments) + "&";
            }
            if (!Roles.IsNullOrEmpty())
            {
                query += "Roles=" + string.Join(",", Roles);
            }
            return query;
        }

        public void PreparePaging()
        {
            if (!string.IsNullOrWhiteSpace(SearchQuery))
            {
                SearchQuery = SearchQuery.ToLower().Trim();
            }
            currentPage = currentPage != 0 ? currentPage : 1;
            pageSize = pageSize < 100 ? pageSize : 100;
            pageSize = pageSize >= 0 ? pageSize : 10;
        }
        public void SetPageSize(int size)
        {
            this.pageSize = size;
        }
    }
}
