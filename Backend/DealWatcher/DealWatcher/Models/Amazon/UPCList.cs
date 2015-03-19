using System.Collections.Generic;
using System.Xml.Serialization;

namespace DealWatcher.Models.Amazon
{
	
	public class UPCList
	{
		
		// ELEMENTS
		[XmlElement("UPCListElement")]
		public List<UPCListElement> UPCListElement { get; set; }
		
		// CONSTRUCTOR
	}
}
