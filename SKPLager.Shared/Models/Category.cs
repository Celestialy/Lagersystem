using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SKPLager.Shared.Models
{
    /// <summary>
    /// Item Category
    /// </summary>
    public class Category : BaseModel<int>
    {
        /// <summary>
        /// Name of the category
        /// </summary>
        [Required]
        public string Name { get; set; }
    }
}
