using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace SKPLager.Shared.Models
{
    /// <summary>
    /// Consumption Item
    /// </summary>
    public class ConsumptionItem : BaseModel<int>
    {
        /// <summary>
        /// User who used the item
        /// </summary>
        public string UserId { get; set; }
        /// <summary>
        /// The amount we currently have of the item
        /// </summary>
        public int Amount { get; set; }
        /// <summary>
        /// Used item id
        /// </summary>
        [Required]
        public int ItemId { get; set; }
        /// <summary>
        /// Used item
        /// </summary>
        public Item Item { get; set; }
        /// <summary>
        /// Inventory id that the item was used from
        /// </summary>
        [Required]
        public int InventoryId { get; set; }
        /// <summary>
        /// Inventory that the item was used from
        /// </summary>
        [JsonIgnore]
        public Inventory Inventory { get; set; }
    }
}
