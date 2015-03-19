using System.Xml.Serialization;

namespace DealWatcher.Models.Amazon
{
	
	public class Title
	{
		
		// ELEMENTS
		[XmlText]
		public string Value { get; set; }
		
		// CONSTRUCTOR
	}
}
