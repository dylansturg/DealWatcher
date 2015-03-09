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
}