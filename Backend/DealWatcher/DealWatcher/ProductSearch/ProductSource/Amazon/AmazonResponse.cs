using DealWatcher.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DealWatcher.ProductSearch.ProductSource.Amazon
{
    public class AmazonResponse
    {
        private string _data;
        public AmazonResponse(String responseString)
        {
            _data = responseString;
        }

        public async Task<IEnumerable<AmazonProduct>> GetResponseResults()
        {
            return null;
        }
    }
}
