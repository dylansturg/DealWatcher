using System.Collections.Generic;
using System.Xml.Serialization;

namespace DealWatcher.Models.Amazon
{
	
	public class Items
	{
		
		// ELEMENTS		
		[XmlElement("Item")]
		public List<Item> Item { get; set; }
		
		// CONSTRUCTOR
	}
}
