using System.Collections.Generic;
using System.Xml.Serialization;

namespace DealWatcher.Models.Amazon
{
	
	public class EANList
	{
		
		// ELEMENTS
		[XmlElement("EANListElement")]
		public List<EANListElement> EANListElement { get; set; }
		
		// CONSTRUCTOR
	}
}
