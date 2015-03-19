using System.Xml.Serialization;

namespace DealWatcher.Models.Amazon
{
	
	public class ListPriceAmount
	{
		
		// ELEMENTS
		[XmlText]
		public int Value { get; set; }
		
		// CONSTRUCTOR
	}
}
