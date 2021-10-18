using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace SKPLager.Shared.Models
{
    public class LoanItem : BaseModel<int>
    {
        /// <summary>
        /// The user who borrowed this item
        /// </summary>
        public string UserId { get; set; }
        /// <summary>
        /// If the item is returned
        /// </summary>
        public bool IsReturned { get; set; }
        /// <summary>
        /// The items Id
        /// </summary>
        [Required]
        public int ItemId { get; set; }
        /// <summary>
        /// The borrowed item
        /// </summary>
        public Item Item { get; set; }
        /// <summary>
        /// The inventory where the item was borrowed froms id
        /// </summary>
        [Required]
        public int InventoryId { get; set; }
        /// <summary>
        /// The inventory where the item was borrowed from
        /// </summary>
        [JsonIgnore]
        public Inventory Inventory { get; set; }
    }
}
