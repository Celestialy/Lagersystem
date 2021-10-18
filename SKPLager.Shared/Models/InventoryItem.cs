using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace SKPLager.Shared.Models
{
    public class InventoryItem : BaseModel<int>
    {
        /// <summary>
        /// The amount we currently have of the item
        /// </summary>
        public int Amount { get; set; }
        /// <summary>
        /// Total Amount of the item
        /// </summary>
        public int TotalAmount { get; set; }
        /// <summary>
        /// The base item id
        /// </summary>
        public int ItemId { get; set; }
        /// <summary>
        /// The Base item
        /// </summary>
        [Required]
        public Item Item { get; set; }
        /// <summary>
        /// Inventory id the item is in
        /// </summary>
        [Required]
        public int InventoryId { get; set; }
        /// <summary>
        /// Inventory the item is in
        /// </summary>
        [JsonIgnore]
        public Inventory Inventory { get; set; }

    }
}
