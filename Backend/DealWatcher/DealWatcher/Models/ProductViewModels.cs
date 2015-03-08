using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DealWatcher.Models
{
    public class ProductCodeTypesViewModel
    {
        public int Id { get; set; }
        public string Type { get; set; }
        public int? ProprietarySellerId { get; set; } 
    }
}