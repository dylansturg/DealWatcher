using System.Xml.Serialization;

namespace DealWatcher.Models.Amazon
{
    public interface IAmazonResult
    {
        Items Items { get; set; }
    }

    [XmlRoot(ElementName = "ItemSearchResponse", Namespace = "http://webservices.amazon.com/AWSECommerceService/2011-08-01")]
	public class ItemSearchResponse : IAmazonResult
	{
		// ELEMENTS		
		[XmlElement("Items")]
		public Items Items { get; set; }
		
		// CONSTRUCTOR
	}

    [XmlRoot(ElementName = "ItemLookupResponse", Namespace = "http://webservices.amazon.com/AWSECommerceService/2011-08-01")]
    public class ItemLookupResponse : IAmazonResult
    {
        [XmlElement("Items")]
        public Items Items { get; set; }
    }
}
