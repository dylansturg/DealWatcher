using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace DealWatcher.ProductSearch.ProductSource.Amazon
{
    protected class RequestsHelper
    {
        private const String REQUEST_URI = "/onca/xml";
        private const String REQUEST_METHOD = "GET";

        private String endpoint = "webservices.amazon.com"; // must be lowercase
        private String awsAccessKeyId = "AKIAJ23Q6IDLNLX3IKFQ";
        private String awsSecretKey = "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";
        private String associatesId = "httpdylangith-20";

        private byte[] SecretKeyBytes;

        private static RequestsHelper _instance = null;
        public static RequestsHelper Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new RequestsHelper();
                }
                return _instance;
            }
        }

        private RequestsHelper()
        {
            SecretKeyBytes = System.Text.Encoding.UTF8.GetBytes(awsSecretKey);
        }

        public String SignRequest(Dictionary<String, String> requestParams)
        {
            requestParams.Add("AWSAccessKeyId", awsAccessKeyId);
            requestParams.Add("Timestamp", timestamp());
            requestParams.Add("AssociateTag", associatesId);

            SortedDictionary<String, String> sortedParamMap = new SortedDictionary<String, String>(
                    requestParams);
            String canonicalQS = canonicalize(sortedParamMap);
            String toSign = REQUEST_METHOD + "\n" + endpoint + "\n" + REQUEST_URI
                    + "\n" + canonicalQS;

            String hmac = HMACSign(toSign);
            String sig = percentEncodeRfc3986(hmac);
            String url = "http://" + endpoint + REQUEST_URI + "?" + canonicalQS
                    + "&Signature=" + sig;

            return url;
        }

        private String HMACSign(String stringToSign)
        {
            String signature = null;
            byte[] data = System.Text.Encoding.UTF8.GetBytes(stringToSign);

            using (var hmac = new HMACSHA256(SecretKeyBytes))
            {
                byte[] signedBytes = hmac.ComputeHash(data);
                signature = Convert.ToBase64String(signedBytes);
            }
            return signature;
        }

        private String timestamp()
        {
            String timestamp = null;
            DateTime now = DateTime.UtcNow;
            timestamp = now.ToString("yyyy-MM-dd'T'HH:mm:ss'Z'");
            return timestamp;
        }

        private String canonicalize(SortedDictionary<String, String> sortedParamMap)
        {
            if (sortedParamMap.Count == 0)
            {
                return "";
            }

            StringBuilder buffer = new StringBuilder();
            foreach (var requestParam in sortedParamMap)
            {
                buffer.Append(percentEncodeRfc3986(requestParam.Key));
                buffer.Append("=");
                buffer.Append(percentEncodeRfc3986(requestParam.Value));
                buffer.Append("&");
            }
            buffer.Remove(buffer.Length - 1, 1);

            return buffer.ToString();
        }

        private String percentEncodeRfc3986(String s)
        {
            String result = Uri.EscapeUriString(s);
            return result.ToUpper();
        }
    }
}
