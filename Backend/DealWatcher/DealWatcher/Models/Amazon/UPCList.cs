using System;
using System.ComponentModel;
using System.Xml.Schema;
using System.Xml.Serialization;
using System.Collections.Generic;
using System.Globalization;

namespace DealWatcher.Models.Amazon
{
	
	public class UPCList
	{
		
		// ELEMENTS
		[XmlElement("UPCListElement")]
		public List<UPCListElement> UPCListElement { get; set; }
		
		// CONSTRUCTOR
		public UPCList()
		{}
	}
}
