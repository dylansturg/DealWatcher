using System.Xml.Serialization;

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
	}
}
