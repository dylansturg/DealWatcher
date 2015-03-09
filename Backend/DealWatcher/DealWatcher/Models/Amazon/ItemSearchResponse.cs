using System;
using System.ComponentModel;
using System.Xml.Schema;
using System.Xml.Serialization;
using System.Collections.Generic;
using System.Globalization;

namespace DealWatcher.Models.Amazon
{
    [XmlRoot(ElementName = "ItemSearchResponse", Namespace = "http://webservices.amazon.com/AWSECommerceService/2011-08-01")]
	public class ItemSearchResponse
	{
		
		// ELEMENTS		
		[XmlElement("Items")]
		public Items Items { get; set; }
		
		// CONSTRUCTOR
		public ItemSearchResponse()
		{}
	}
}
