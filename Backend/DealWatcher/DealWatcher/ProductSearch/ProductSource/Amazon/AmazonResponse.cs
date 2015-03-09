using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DealWatcher.Models;

namespace DealWatcher.ProductSearch.ProductSource.Amazon
{
    public class AmazonResponse
    {
        private string _data;
        public AmazonResponse(String responseString)
        {
            _data = responseString;
        }

        public async Task<IEnumerable<AmazonProduct>> GetResponseResultsAsync()
        {
            return null;
        }
    }
}
