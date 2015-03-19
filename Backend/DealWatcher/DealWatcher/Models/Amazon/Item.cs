using System;
using System.ComponentModel;
using System.Xml.Schema;
using System.Xml.Serialization;
using System.Collections.Generic;
using System.Globalization;

namespace DealWatcher.Models.Amazon
{
	
	public class Item
	{
		
		// ELEMENTS
		[XmlElement("ASIN")]
		public ASIN ASIN { get; set; }
		
		[XmlElement("DetailPageURL")]
		public DetailPageURL DetailPageURL { get; set; }
		
		[XmlElement("SmallImage")]
		public ItemImage ItemSmallImage { get; set; }
		
		[XmlElement("MediumImage")]
		public ItemImage ItemMediumImage { get; set; }
		
		[XmlElement("LargeImage")]
		public ItemImage ItemLargeImage { get; set; }
		
		[XmlElement("ItemAttributes")]
		public ItemAttributes ItemAttributes { get; set; }
		
		[XmlElement("OfferSummary")]
		public OfferSummary OfferSummary { get; set; }
		
		// CONSTRUCTOR
		public Item()
		{}
	}
}
