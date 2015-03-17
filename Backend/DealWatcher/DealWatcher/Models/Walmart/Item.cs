using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DealWatcher.Models.Walmart
{
    public class Attributes
    {
        public string color { get; set; }
    }

    public class Item
    {
        public int itemId { get; set; }
        public int parentItemId { get; set; }
        public string name { get; set; }
        public double msrp { get; set; }
        public double salePrice { get; set; }
        public string upc { get; set; }
        public string categoryPath { get; set; }
        public string shortDescription { get; set; }
        public string longDescription { get; set; }
        public string brandName { get; set; }
        public string thumbnailImage { get; set; }
        public string mediumImage { get; set; }
        public string largeImage { get; set; }
        public string productTrackingUrl { get; set; }
        public bool ninetySevenCentShipping { get; set; }
        public double standardShipRate { get; set; }
        public double twoThreeDayShippingRate { get; set; }
        public double overnightShippingRate { get; set; }
        public string color { get; set; }
        public bool marketplace { get; set; }
        public bool shipToStore { get; set; }
        public bool freeShipToStore { get; set; }
        public string modelNumber { get; set; }
        public string productUrl { get; set; }
        public string customerRating { get; set; }
        public int numReviews { get; set; }
        public List<int> variants { get; set; }
        public string customerRatingImage { get; set; }
        public string categoryNode { get; set; }
        public bool bundle { get; set; }
        public bool preOrder { get; set; }
        public string stock { get; set; }
        public Attributes attributes { get; set; }
        public string addToCartUrl { get; set; }
        public string affiliateAddToCartUrl { get; set; }
        public bool freeShippingOver50Dollars { get; set; }
        public bool availableOnline { get; set; }
    }
}