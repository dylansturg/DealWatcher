using DealWatcher.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace DealWatcher.ProductSearch.ProductSource.Amazon
{
    private enum Operation
    {
        ItemLookup,
        ItemSearch,
    }

    public class AmazonRequest
    {
        private const int MAX_ITEM_PAGES = 10;
        protected Operation RequestType { get; set; }
        protected int Page { get; set; }

        protected String ProductCode { get; set; }
        protected String ProductCodeType { get; set; }
        protected String ProductName { get; set; }

        public AmazonRequest(Product search)
        {
            Page = 0;
        }

        public AmazonRequest(ProductSearchBindingModel search)
        {
            Page = 0;
        }

        public async Task<IEnumerable<AmazonResponse>> ExecuteAsync()
        {
            var results = new List<AmazonResponse>();
            var apiTasks = new List<Task>();
            using (var client = new HttpClient())
            {
                for (int i = Page; i < MAX_ITEM_PAGES; i++)
                {
                    var request = GetRequestParameters();
                    var uri = RequestsHelper.Instance.SignRequest(request);
                    var downloadTask = Task.Factory.StartNew(async () =>
                    {
                        var responseString = await client.GetStringAsync(uri);
                        results.Add(new AmazonResponse(responseString));
                    });
                    
                    Page++;
                }
            }
            await Task.WhenAll(apiTasks);
            return results;
        }

        private Dictionary<String, String> GetRequestParameters()
        {
            var requestParams = new Dictionary<String, String>();
            AppendSearchParams(requestParams);
            AppendDefaultParams(requestParams);
            return requestParams;
        }

        private void AppendSearchParams(Dictionary<String, String> request)
        {
            switch (RequestType)
            {
                case Operation.ItemLookup:
                    ItemLookupParams(request);
                    break;
                case Operation.ItemSearch:
                    ItemSearchParams(request);
                    break;
            }
        }

        private void ItemLookupParams(Dictionary<String, String> request)
        {
            request.Add("Operation", "ItemLookup");
            request.Add("ItemId", ProductCode);
            request.Add("IdType", ProductCodeType);
        }

        private void ItemSearchParams(Dictionary<String, String> request)
        {
            request.Add("Operation", "ItemSearch");
            request.Add("Keywords", ProductName);
        }

        private void AppendDefaultParams(Dictionary<String, String> request)
        {
            request.Add("SearchIndex", "All");
            request.Add("Availability", "Available");
            request.Add("ResponseGroup", "ItemAttributes,Images,OfferSummary");
            request.Add("ItemPage", Page.ToString());
        }
    }
}
