﻿namespace DealWatcher.Models
{
    public class ProductSearchBindingModel
    {
        public string ProductName { get; set; }
        public string Keywords { get; set; }
        public string ProductCode { get; set; }
        public int? ProductCodeTypeId { get; set; }
    }
}