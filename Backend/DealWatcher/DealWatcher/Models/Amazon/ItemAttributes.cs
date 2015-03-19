using System;
using System.ComponentModel;
using System.Xml.Schema;
using System.Xml.Serialization;
using System.Collections.Generic;
using System.Globalization;

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
		public ItemAttributes()
		{}
	}
}
