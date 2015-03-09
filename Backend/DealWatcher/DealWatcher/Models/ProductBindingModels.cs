using DealWatcher.Filters;

namespace DealWatcher.Models
{
    public class ProductSearchBindingModel
    {
        public string Keywords { get; set; }

        [RequiredWhenOtherPresentAttribute("ProductCodeTypeId")]
        public string ProductCode { get; set; }
        [RequiredWhenOtherPresent("ProductCode")]
        public int? ProductCodeTypeId { get; set; }
    }
}