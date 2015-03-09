using System;
using System.ComponentModel;
using System.Xml.Schema;
using System.Xml.Serialization;
using System.Collections.Generic;
using System.Globalization;

namespace DealWatcher.Models.Amazon
{
	
	public class ListPrice
	{
		
		// ELEMENTS
		[XmlElement("Amount")]
		public ListPriceAmount ListPriceAmount { get; set; }
		
		[XmlElement("CurrencyCode")]
		public ListPriceCurrencyCode ListPriceCurrencyCode { get; set; }
		
		// CONSTRUCTOR
		public ListPrice()
		{}
	}
}
