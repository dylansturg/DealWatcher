using System.Xml.Serialization;

namespace DealWatcher.Models.Amazon
{
	
	public class EANListElement
	{
		
		// ELEMENTS
		[XmlText]
		public string Value { get; set; }
		
		// CONSTRUCTOR
	}
}
