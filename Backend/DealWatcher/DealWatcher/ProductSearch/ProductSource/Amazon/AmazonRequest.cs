using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using DealWatcher.Models;

namespace DealWatcher.ProductSearch.ProductSource.Amazon
{
    public class AmazonRequest
    {
        protected enum Operation
        {
            ItemLookup,
            ItemSearch,
        }

        private const string AmazonCodeType = "ASIN";
        private const int MaxItemPages = 6;
        protected Operation RequestType { get; set; }
        protected int Page = 1;

        protected String ProductCode { get; set; }
        protected String ProductCodeType { get; set; }
        protected String ProductName { get; set; }

        public AmazonRequest(Product search)
        {
            var savedProductCode = search.ProductCodes.FirstOrDefault(code => code.ProductCodeType.Type.Equals(AmazonCodeType, StringComparison.CurrentCultureIgnoreCase));
            if (savedProductCode == null)
            {
                savedProductCode = search.ProductCodes.FirstOrDefault();
                if (savedProductCode == null)
                {
                    SetupNameSearch(search.DisplayName);
                    return;
                }
            }

            SetupItemLookup(savedProductCode.Code, savedProductCode.ProductCodeType.Type);
        }

        public AmazonRequest(ProductSearchViewModel search)
        {
            if (!String.IsNullOrEmpty(search.ProductCode) && !String.IsNullOrEmpty(search.ProductCodeType))
            {
                SetupItemLookup(search.ProductCode, search.ProductCodeType);
            }
            else
            {
                SetupNameSearch(search.ProductName);
            }
        }

        private void SetupNameSearch(String searchTerms)
        {
            RequestType = Operation.ItemSearch;
            ProductName = searchTerms;
        }

        private void SetupItemLookup(String productCode, String codeType)
        {
            RequestType = Operation.ItemLookup;
            ProductCode = productCode;
            ProductCodeType = codeType;
        }

        public async Task<IEnumerable<AmazonResponse>> ExecuteAsync()
        {
            var results = new ConcurrentBag<AmazonResponse>();
            
            using (var client = new HttpClient())
            {
                var apiTasks = new List<Task>();
                for (var i = Page; i < MaxItemPages; i++)
                {
                    var request = GetRequestParameters();
                    var uri = RequestsHelper.Instance.SignRequest(request);

                    apiTasks.Add(TaskEx.Run(async () =>
                    {
                        var apiResponse = await client.GetStringAsync(uri);
                        var response = new AmazonResponse();
                        await response.ParseResultsAsync(apiResponse);
                        results.Add(response);
                    }));

                    Page++;
                }
                await Task.WhenAll(apiTasks);
            }
            
            return results.ToList();
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
            request.Add("SearchIndex", "Blended");
            request.Add("Availability", "Available");
            request.Add("ResponseGroup", "ItemAttributes,Images,OfferSummary");
            request.Add("ItemPage", Page.ToString());
        }
    }
}
