using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DealWatcher.Models;
using DealWatcher.Models.Amazon;
using System.Xml.Serialization;
using System.IO;
using System.Text;
using System.Linq;

namespace DealWatcher.ProductSearch.ProductSource.Amazon
{
    public class AmazonResponse : IApiResponse
    {
        public IEnumerable<IApiProduct> ParsedProducts { get; private set; }
        private AmazonRequest.Operation RequestType { get; set; }

        public AmazonResponse(AmazonRequest.Operation requestType)
        {
            RequestType = requestType;
        }

        private XmlSerializer CreateSerializer()
        {
            switch (RequestType)
            {
                case AmazonRequest.Operation.ItemLookup:
                    return new XmlSerializer(typeof(ItemLookupResponse));
                case AmazonRequest.Operation.ItemSearch:
                    return new XmlSerializer(typeof(ItemSearchResponse));
                default:
                    // Probably shouldn't let this happen, who knows what that's going to do
                    return new XmlSerializer(typeof(void));
            }
        }

        public async Task ParseResultsAsync(String xmlSource)
        {
            List<Item> items = null;
            var serializer = CreateSerializer();
            using (var xmlStream = StringStream(xmlSource))
            {
                try
                {
                    var parsed = serializer.Deserialize(xmlStream) as IAmazonResult;
                    items = parsed != null ? (parsed.Items != null ? parsed.Items.Item : null) : null;
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
            }

            if (items == null)
            {
                ParsedProducts = new List<AmazonProduct>();
                return;
            }

            Exception parseFailure = null;
            var converted = items.Select(item =>
            {
                try
                {
                    return new AmazonProduct
                    {
                        ASIN = item.ASIN != null ? item.ASIN.Value : "",
                        DetailsUrl = item.DetailPageURL != null ? item.DetailPageURL.Value : "",
                        EANs = item.ItemAttributes.EANList != null ? new HashSet<string>(item.ItemAttributes.EANList.EANListElement.Select(el => el.Value)) : new HashSet<string>(),
                        UPCs = item.ItemAttributes.UPCList != null ? new HashSet<string>(item.ItemAttributes.UPCList.UPCListElement.Select(el => el.Value)) : new HashSet<string>(),
                        Title = item.ItemAttributes.Title.Value,
                        ImageUrls = SelectItemImage(item),
                        Price = SelectItemPrice(item),
                    };
                }
                catch (Exception e)
                {
                    parseFailure = e;
                    return null;
                }
            });
            
            var amazonProds = converted.Where(prod => prod != null).Where(prod => prod.Price > 0 && !String.IsNullOrEmpty(prod.ASIN));

            ParsedProducts = amazonProds;
        }

        private static double SelectItemPrice(Item item)
        {
            if (item.OfferSummary != null && item.OfferSummary.LowestNewPrice != null && item.OfferSummary.LowestNewPrice.ListPriceAmount != null)
            {
                return ParsePriceAmount(item.OfferSummary.LowestNewPrice.ListPriceAmount.Value.ToString());
            }
            
            if (item.ItemAttributes != null && item.ItemAttributes.ListPrice != null && item.ItemAttributes.ListPrice.ListPriceAmount != null)
            {
                return ParsePriceAmount(item.ItemAttributes.ListPrice.ListPriceAmount.Value.ToString());
            }

            return -1;
        }

        private static double ParsePriceAmount(string lowPrice)
        {
            if (lowPrice.Length <= 2)
            {
                return Double.Parse(String.Format("0.{0}", lowPrice));
            }

            var decimals = lowPrice.Substring(lowPrice.Length - 2, 2);
            var digits = lowPrice.Substring(0, lowPrice.Length - 2);
            return Double.Parse(String.Format("{0}.{1}", digits, decimals));
        }

        private static IEnumerable<string> SelectItemImage(Item item)
        {
            var result = new HashSet<String>();
            if (item.ItemLargeImage != null && item.ItemLargeImage.ItemImageURL != null)
            {
                result.Add(item.ItemLargeImage.ItemImageURL.Value);
            }
            else if (item.ItemMediumImage != null && item.ItemMediumImage.ItemImageURL != null)
            {
                result.Add(item.ItemMediumImage.ItemImageURL.Value);
            }
            else if (item.ItemSmallImage != null && item.ItemSmallImage.ItemImageURL != null)
            {
                result.Add(item.ItemSmallImage.ItemImageURL.Value);
            }
            return result;
        }

        private static Stream StringStream(String s)
        {
            return new MemoryStream(Encoding.UTF8.GetBytes(s ?? ""));
        }
    }
}
