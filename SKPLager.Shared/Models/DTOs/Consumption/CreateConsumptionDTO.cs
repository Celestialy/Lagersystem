using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SKPLager.Shared.Models.DTOs
{
    public class CreateConsumptionDTO
    {
        /// <summary>
        /// The user who borrowed this item
        /// </summary>
        [Required]
        public string UserId { get; set; }
        /// <summary>
        /// The amount we currently have of the item
        /// </summary>
        public int Amount { get; set; } = 1;

        [Required]
        public int DepartmentId { get; set; }
        /// <summary>
        /// The items Id
        /// </summary>
        [Required]
        public int ItemId { get; set; }
        /// <summary>
        /// The inventory where the item was borrowed froms id
        /// </summary>
        [Required]
        public int InventoryId { get; set; }
    }
}
