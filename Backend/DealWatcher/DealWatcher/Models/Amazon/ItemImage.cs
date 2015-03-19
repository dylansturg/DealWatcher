using System;
using System.ComponentModel;
using System.Xml.Schema;
using System.Xml.Serialization;
using System.Collections.Generic;
using System.Globalization;

namespace DealWatcher.Models.Amazon
{
	
	public class ItemImage
	{
		
		// ELEMENTS
		[XmlElement("URL")]
		public ItemImageURL ItemImageURL { get; set; }
		
		[XmlElement("Height")]
		public ItemImageHeight ItemImageHeight { get; set; }
		
		[XmlElement("Width")]
		public ItemImageWidth ItemImageWidth { get; set; }
		
		// CONSTRUCTOR
		public ItemImage()
		{}
	}
}
