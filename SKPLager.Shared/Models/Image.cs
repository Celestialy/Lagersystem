using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SKPLager.Shared.Models
{
    [Index(nameof(IsDeleted))]
    public class Image : BaseModel <int>
    {
        /// <summary>
        /// Name of the Image
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Image URL
        /// </summary>
        public Uri Uri { get; set; }
        /// <summary>
        /// If the image is deleted
        /// </summary>
        public bool IsDeleted { get; set; }
        
    }
}
