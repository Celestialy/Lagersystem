using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SKPLager.Shared.Models.DTOs
{
    public class UpdateItemDTO
    {
        [Key]
        public int Id { get; set; }
        /// <summary>
        /// The amount we currently have of the item
        /// </summary>
        public int Amount { get; set; }
        /// <summary>
        /// Total Amount of the item
        /// </summary>
        public int TotalAmount { get; set; }
        /// <summary>
        /// The Base item
        /// </summary>
        [Required]
        public ForUpdateItemDTO Item { get; set; }
    }

    public class ForUpdateItemDTO
    {
        /// <summary>
        /// The brand of the item
        /// </summary>
        public string Brand { get; set; }
        /// <summary>
        /// The items model
        /// </summary>
        public string Model { get; set; }
        /// <summary>
        /// The barcode of item
        /// </summary>
        public string Barcode { get; set; }
        /// <summary>
        /// The id of the items image
        /// </summary>
        public int? ImageId { get; set; }
        /// <summary>
        /// The id of the items category
        /// </summary>
        public int? CategoryId { get; set; }
    }
}
