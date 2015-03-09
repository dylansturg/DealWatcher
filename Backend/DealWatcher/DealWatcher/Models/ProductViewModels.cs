using System;
using System.Collections;
using System.Collections.Generic;
namespace DealWatcher.Models
{
    public class ProductCodeTypesViewModel
    {
        public int Id { get; set; }
        public string Type { get; set; }
        public int? ProprietarySellerId { get; set; } 
    }

    public class ProductSearchViewModel
    {
        public string ProductName { get; set; }
        public string Keywords { get; set; }
        public string ProductCode { get; set; }
        public int? ProductCodeTypeId { get; set; }
        public string ProductCodeType { get; set; }
    }

    public class ProductViewModel
    {
        public int Id { get; set; }
        public string DisplayName { get; set; }


        public IEnumerable<ProductImageViewModel> ProductImages { get; set; }
        public IEnumerable<ProductPriceViewModel> ProductPrices { get; set; }
        public IEnumerable<ProductCodeViewModel> ProductCodes { get; set; }
    }

    public class ProductImageViewModel
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public string Url { get; set; }
    }

    public class ProductPriceViewModel
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public int SellerId { get; set; }
        public DateTime Gathered { get; set; }
        public string LocationUrl { get; set; }
        public decimal Price { get; set; }
        public bool Current { get; set; }
    }

    public class ProductCodeViewModel
    {
        public int TypeId { get; set; }
        public string Type { get; set; }
        public int ProductId { get; set; }
        public string Code { get; set; }
    }
}