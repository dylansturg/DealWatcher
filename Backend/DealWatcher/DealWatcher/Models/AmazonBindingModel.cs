using System;
using System.Collections.Generic;

namespace DealWatcher.Models
{
    public class AmazonBindingModel
    {
    }

    public class AmazonProduct
    {
        public String ASIN;
        public String DetailsUrl;
        public IEnumerable<string> ImageUrls = new HashSet<string>();
        public double Price = -1;
        public String Title;
        public HashSet<String> UPCs = new HashSet<string>();
        public HashSet<String> EANs = new HashSet<string>();
    }
}
