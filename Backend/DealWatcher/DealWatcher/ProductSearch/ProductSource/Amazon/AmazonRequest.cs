using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using DealWatcher.Models;

namespace DealWatcher.ProductSearch.ProductSource.Amazon
{
    public class AmazonRequest
    {
        public enum Operation
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
                SetupNameSearch(search.Keywords);
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
            var results = new AmazonResponse[MaxItemPages - Page];
            
            using (var client = new HttpClient(new AmazonRetryHandler()))
            {
                var apiTasks = new List<Task>();
                for (var i = Page; i < MaxItemPages; i++)
                {
                    var request = GetRequestParameters();
                    var uri = RequestsHelper.Instance.SignRequest(request);

                    var index = i;
                    apiTasks.Add(TaskEx.Run(async () =>
                    {
                        try
                        {
                            var apiResponse = await client.GetStringAsync(uri);
                            var response = new AmazonResponse(RequestType);
                            await response.ParseResultsAsync(apiResponse);
                            results[index - 1] = response;
                        }
                        catch (Exception httpException)
                        {
                            Console.WriteLine(httpException);
                        }
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
            AppendDefaultParams(requestParams);
            AppendSearchParams(requestParams);
            return requestParams;
        }

        private void AppendSearchParams(IDictionary<String, String> request)
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

        private void ItemLookupParams(IDictionary<String, String> request)
        {
            request.Add("Operation", "ItemLookup");
            request.Add("ItemId", ProductCode);
            request.Add("IdType", ProductCodeType);
        }

        private void ItemSearchParams(IDictionary<string, string> request)
        {
            request.Add("Operation", "ItemSearch");
            request.Add("Keywords", ProductName);
            request.Add("SearchIndex", "All");
        }

        private void AppendDefaultParams(IDictionary<String, String> request)
        {
            request.Add("Availability", "Available");
            request.Add("ResponseGroup", "ItemAttributes,Images,OfferSummary");
            request.Add("ItemPage", Page.ToString());
        }
    }

    class AmazonRetryHandler : HttpClientHandler
    {
        private const int BackoffMultiplier = 1000;
        public int Retries { get; set; }
        public int Backoff { get; set; }

        
        public AmazonRetryHandler(int retries = 2, int backoff = 1)
        {
            Retries = retries;
            Backoff = backoff;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            HttpResponseMessage response = null;
            for (var i = 0; i < Retries; i++)
            {
                response = await base.SendAsync(request, cancellationToken);
                if (response.IsSuccessStatusCode)
                {
                    return response;
                }

                Thread.Sleep(TimeSpan.FromMilliseconds(Backoff*BackoffMultiplier*Math.Pow(2, i)));
            }
            return response;
        }
    }
}
