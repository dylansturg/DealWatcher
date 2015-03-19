using System;
using System.ComponentModel;
using System.Xml.Schema;
using System.Xml.Serialization;
using System.Collections.Generic;
using System.Globalization;

namespace DealWatcher.Models.Amazon
{
	
	public class ItemImageWidth
	{
		// ATTRIBUTES
		[XmlAttribute("Units")]
		public string Units { get; set; }
		
		// ELEMENTS
		[XmlText]
		public int Value { get; set; }
		
		// CONSTRUCTOR
		public ItemImageWidth()
		{}
	}
}
