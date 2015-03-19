using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace DealWatcher.ProductSearch.ProductSource.Amazon
{
    class RequestsHelper
    {
        private const String RequestUri = "/onca/xml";
        private const String RequestMethod = "GET";

        private const String Endpoint = "webservices.amazon.com"; // must be lowercase
        private const String AwsAccessKeyId = "AKIAJ23Q6IDLNLX3IKFQ";
        private const String AwsSecretKey = "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";
        private const String AssociatesId = "httpdylangith-20";

        private readonly byte[] _secretKeyBytes;

        private static RequestsHelper _instance;
        public static RequestsHelper Instance
        {
            get { return _instance ?? (_instance = new RequestsHelper()); }
        }

        public RequestsHelper()
        {
            _secretKeyBytes = Encoding.UTF8.GetBytes(AwsSecretKey);
        }

        public String SignRequest(Dictionary<String, String> requestParams)
        {
            requestParams.Add("AWSAccessKeyId", AwsAccessKeyId);
            requestParams.Add("Timestamp", Timestamp());
            requestParams.Add("AssociateTag", AssociatesId);

            var sortedParamMap = new SortedDictionary<String, String>(
                    requestParams, new ByteSorter());
            var canonicalQs = Canonicalize(sortedParamMap);
            var toSign = RequestMethod + "\n" + Endpoint + "\n" + RequestUri
                    + "\n" + canonicalQs;

            var hmac = HmacSign(toSign);
            var sig = PercentEncodeRfc3986(hmac);
            var url = "http://" + Endpoint + RequestUri + "?" + canonicalQs
                    + "&Signature=" + sig;

            return url;
        }

        private String HmacSign(String stringToSign)
        {
            String signature;
            var data = Encoding.UTF8.GetBytes(stringToSign);

            using (var hmac = new HMACSHA256(_secretKeyBytes))
            {
                var signedBytes = hmac.ComputeHash(data);
                signature = Convert.ToBase64String(signedBytes);
            }
            return signature;
        }

        private static String Timestamp()
        {
            var now = DateTime.UtcNow;
            now = now.Subtract(TimeSpan.FromMilliseconds(now.Millisecond));
            var timestamp = now.ToString("yyyy-MM-dd'T'HH:mm:ss'Z'");
            return timestamp;
        }

        private String Canonicalize(SortedDictionary<String, String> sortedParamMap)
        {
            if (sortedParamMap.Count == 0)
            {
                return "";
            }

            var buffer = new StringBuilder();
            foreach (var requestParam in sortedParamMap)
            {
                buffer.Append(PercentEncodeRfc3986(requestParam.Key));
                buffer.Append("=");
                buffer.Append(PercentEncodeRfc3986(requestParam.Value));
                buffer.Append("&");
            }
            buffer.Remove(buffer.Length - 1, 1);

            return buffer.ToString();
        }

        private String PercentEncodeRfc3986(String s)
        {
            var result = Uri.EscapeDataString(s);
            return result;
        }

        protected class ByteSorter : IComparer<String>
        {
            public int Compare(string x, string y)
            {
                return String.CompareOrdinal(x, y);
            }
        }
    }
}
