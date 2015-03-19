using System.Xml.Serialization;

namespace DealWatcher.Models.Amazon
{
	
	public class OfferSummary
	{
		
		// ELEMENTS
		[XmlElement("LowestNewPrice")]
		public ListPrice LowestNewPrice { get; set; }
		
		[XmlElement("TotalNew")]
		public TotalNew TotalNew { get; set; }
		
		[XmlElement("TotalUsed")]
		public TotalUsed TotalUsed { get; set; }
		
		[XmlElement("LowestUsedPrice")]
		public ListPrice LowestUsedPrice { get; set; }
		
		// CONSTRUCTOR
	}
}
