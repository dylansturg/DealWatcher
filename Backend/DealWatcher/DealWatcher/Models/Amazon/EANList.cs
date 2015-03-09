using System;
using System.ComponentModel;
using System.Xml.Schema;
using System.Xml.Serialization;
using System.Collections.Generic;
using System.Globalization;

namespace DealWatcher.Models.Amazon
{
	
	public class EANList
	{
		
		// ELEMENTS
		[XmlElement("EANListElement")]
		public List<EANListElement> EANListElement { get; set; }
		
		// CONSTRUCTOR
		public EANList()
		{}
	}
}
