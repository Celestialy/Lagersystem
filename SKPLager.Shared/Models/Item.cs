using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SKPLager.Shared.Models
{
    public class Item : BaseModel<int>
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
        /// The image of the item
        /// </summary>
        public Image Image { get; set; }
        /// <summary>
        /// The id of the items category
        /// </summary>
        public int? CategoryId { get; set; }
        /// <summary>
        /// The Category of the item
        /// </summary>
        public Category Category { get; set; }
    }
}
