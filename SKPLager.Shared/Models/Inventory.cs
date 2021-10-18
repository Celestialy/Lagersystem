using SKPLager.Shared.Models.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SKPLager.Shared.Models
{
    public class Inventory : BaseModel<int>
    {
        /// <summary>
        /// Name of inventory
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Inventory type
        /// </summary>
        public InventoryType Type { get; set; }
        /// <summary>
        /// Items in the Inventory
        /// </summary>
        public IEnumerable<InventoryItem> Items { get; set; }
        /// <summary>
        /// List of items used
        /// </summary>
        public IEnumerable<ConsumptionItem> Consumptions { get; set; }
        /// <summary>
        /// List of items borrowed
        /// </summary>
        public IEnumerable<LoanItem> Loans { get; set; }
    }
}
