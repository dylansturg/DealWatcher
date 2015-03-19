using System.Xml.Serialization;

namespace DealWatcher.Models.Amazon
{
	
	public class Description
	{
		
		// ELEMENTS
		[XmlText]
		public string Value { get; set; }
		
		// CONSTRUCTOR
	}
}
