using System;
using System.ComponentModel;
using System.Xml.Schema;
using System.Xml.Serialization;
using System.Collections.Generic;
using System.Globalization;
using System.Security.Cryptography.X509Certificates;

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
		public ItemSearchResponse()
		{}
	}

    [XmlRoot(ElementName = "ItemLookupResponse", Namespace = "http://webservices.amazon.com/AWSECommerceService/2011-08-01")]
    public class ItemLookupResponse : IAmazonResult
    {
        [XmlElement("Items")]
        public Items Items { get; set; }

        public ItemLookupResponse()
        {}
    }
}
