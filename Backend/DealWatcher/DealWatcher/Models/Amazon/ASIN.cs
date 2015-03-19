using System.Xml.Serialization;

namespace DealWatcher.Models.Amazon
{
	
	public class ASIN
	{
		
		// ELEMENTS
		[XmlText]
		public string Value { get; set; }
		
		// CONSTRUCTOR
	}
}
