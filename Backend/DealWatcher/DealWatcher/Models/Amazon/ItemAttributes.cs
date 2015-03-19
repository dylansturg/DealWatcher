using System.Xml.Serialization;

namespace DealWatcher.Models.Amazon
{
	
	public class ItemAttributes
	{
		
		// ELEMENTS
		[XmlElement("EANList")]
		public EANList EANList { get; set; }
		
		[XmlElement("ListPrice")]
		public ListPrice ListPrice { get; set; }
		
		[XmlElement("Title")]
		public Title Title { get; set; }

		[XmlElement("UPCList")]
		public UPCList UPCList { get; set; }
		
		// CONSTRUCTOR
	}
}
