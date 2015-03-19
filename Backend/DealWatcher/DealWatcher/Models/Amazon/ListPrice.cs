using System.Xml.Serialization;

namespace DealWatcher.Models.Amazon
{
	
	public class ListPrice
	{
		
		// ELEMENTS
		[XmlElement("Amount")]
		public ListPriceAmount ListPriceAmount { get; set; }
		
		[XmlElement("CurrencyCode")]
		public ListPriceCurrencyCode ListPriceCurrencyCode { get; set; }
		
		// CONSTRUCTOR
	}
}
