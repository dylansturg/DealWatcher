using System.Xml.Serialization;

namespace DealWatcher.Models.Amazon
{
	
	public class ItemImageWidth
	{
		// ATTRIBUTES
		[XmlAttribute("Units")]
		public string Units { get; set; }
		
		// ELEMENTS
		[XmlText]
		public int Value { get; set; }
		
		// CONSTRUCTOR
	}
}
