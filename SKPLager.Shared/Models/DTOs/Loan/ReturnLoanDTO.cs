using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SKPLager.Shared.Models.DTOs
{
    public class ReturnLoanDTO
    {

        [Key]
        public int Id { get; set; }
        /// <summary>
        /// The user who borrowed this item
        /// </summary>
        [Required]
        public string UserId { get; set; }

        /// <summary>
        /// If the item is returned
        /// </summary>
        public bool IsReturned { get; set; }
    }
}
