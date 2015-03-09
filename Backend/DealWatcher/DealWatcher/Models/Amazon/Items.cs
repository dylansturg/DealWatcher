using System;
using System.ComponentModel;
using System.Xml.Schema;
using System.Xml.Serialization;
using System.Collections.Generic;
using System.Globalization;

namespace DealWatcher.Models.Amazon
{
	
	public class Items
	{
		
		// ELEMENTS		
		[XmlElement("Item")]
		public List<Item> Item { get; set; }
		
		// CONSTRUCTOR
		public Items()
		{}
	}
}
