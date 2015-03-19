using System.Xml.Serialization;

namespace DealWatcher.Models.Amazon
{
	
	public class TotalUsed
	{
		
		// ELEMENTS
		[XmlText]
		public int Value { get; set; }
		
		// CONSTRUCTOR
	}
}
