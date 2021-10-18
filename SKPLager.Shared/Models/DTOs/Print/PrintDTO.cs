using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SKPLager.Shared.Models.DTOs
{
    public class PrintDTO
    {
        [Required]
        public string Barcode { get; set; }
    }
}
