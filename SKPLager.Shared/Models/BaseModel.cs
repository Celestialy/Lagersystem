using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace SKPLager.Shared.Models
{
    [Index(nameof(DepartmentId))]
    public class BaseModel<IDType>
    {
        [Key]
        public IDType Id { get; set; }
        /// <summary>
        /// Department Image is in
        /// </summary>
        
        [Required]
        public int DepartmentId { get; set; }

        [JsonIgnore]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        [JsonIgnore]
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;




    }
}
