using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Xml;

namespace MTGWatcher.Models
{
    public class RequestHelper
    {
        public static string mkmRequest(string url)
        {
            String method = "GET";

            HttpWebRequest request = WebRequest.CreateHttp(url) as HttpWebRequest;
            OAuthHeader header = new OAuthHeader();
            request.Headers.Add(HttpRequestHeader.Authorization, header.getAuthorizationHeader(method, url));
            request.Method = method;

            HttpWebResponse response = request.GetResponse() as HttpWebResponse;
            string responseText;
            using (var reader = new System.IO.StreamReader(response.GetResponseStream(), Encoding.UTF8))
            {
                 responseText = reader.ReadToEnd();
            }
            return responseText;
            // proceed further
        }

    }


    /// <summary>
    /// Class encapsulates tokens and secret to create OAuth signatures and return Authorization headers for web requests.
    /// </summary>
    class OAuthHeader
    {
        /// <summary>App Token</summary>
        protected String appToken = "3g1xTLCLwzmsCd7j";
        /// <summary>App Secret</summary>
        protected String appSecret = "3pAKr8ECy8IitxqTq8AOm50Xqa8UU7Iy";
        /// <summary>Access Token (Class should also implement an AccessToken property to set the value)</summary>
        protected String accessToken = "";
        /// <summary>Access Token Secret (Class should also implement an AccessToken property to set the value)</summary>
        protected String accessSecret = "";
        /// <summary>OAuth Signature Method</summary>
        protected String signatureMethod = "HMAC-SHA1";
        /// <summary>OAuth Version</summary>
        protected String version = "1.0";
        /// <summary>All Header params compiled into a Dictionary</summary>
        protected IDictionary<String, String> headerParams;

        /// <summary>
        /// Constructor
        /// </summary>
        public OAuthHeader()
        {
            //String nonce = Guid.NewGuid().ToString("n");
            String nonce = "972WIs";
            //String timestamp = (DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds.ToString();
            String timestamp = "1485348905";
            /// Initialize all class members
            this.headerParams = new Dictionary<String, String>();
            this.headerParams.Add("oauth_consumer_key", this.appToken);
            this.headerParams.Add("oauth_token", this.accessToken);
            this.headerParams.Add("oauth_nonce", nonce);
            this.headerParams.Add("oauth_timestamp", timestamp);
            this.headerParams.Add("oauth_signature_method", this.signatureMethod);
            this.headerParams.Add("oauth_version", this.version);
        }

        /// <summary>
        /// Pass request method and URI parameters to get the Authorization header value
        /// </summary>
        /// <param name="method">Request Method</param>
        /// <param name="url">Request URI</param>
        /// <returns>Authorization header value</returns>
        public String getAuthorizationHeader(String method, String url)
        {
            /// Add the realm parameter to the header params
            this.headerParams.Add("realm", url);

            /// Start composing the base string from the method and request URI
            String baseString = method.ToUpper()
                              + "&"
                              + Uri.EscapeDataString(url)
                              + "&";

            /// Gather, encode, and sort the base string parameters
            SortedDictionary<String, String> encodedParams = new SortedDictionary<String, String>();
            foreach (KeyValuePair<String, String> parameter in this.headerParams)
            {
                if (false == parameter.Key.Equals("realm"))
                {
                    encodedParams.Add(Uri.EscapeDataString(parameter.Key), Uri.EscapeDataString(parameter.Value));
                }
            }

            /// Expand the base string by the encoded parameter=value pairs            
            List<String> paramStrings = new List<String>();
            foreach (KeyValuePair<String, String> parameter in encodedParams)
            {
                paramStrings.Add(parameter.Key + "=" + parameter.Value);
            }
            String paramString = Uri.EscapeDataString(String.Join<String>("&", paramStrings));
            baseString += paramString;

            /// Create the OAuth signature
            String signatureKey = Uri.EscapeDataString(this.appSecret) + "&" + Uri.EscapeDataString(this.accessSecret);
            HMAC hasher = HMACSHA1.Create();
            hasher.Key = Encoding.UTF8.GetBytes(signatureKey);
            Byte[] rawSignature = hasher.ComputeHash(Encoding.UTF8.GetBytes(baseString));
            String oAuthSignature = Convert.ToBase64String(rawSignature);

            /// Include the OAuth signature parameter in the header parameters array
            this.headerParams.Add("oauth_signature", oAuthSignature);

            /// Construct the header string
            List<String> headerParamStrings = new List<String>();
            foreach (KeyValuePair<String, String> parameter in this.headerParams)
            {
                headerParamStrings.Add(parameter.Key + "=\"" + parameter.Value + "\"");
            }
            String authHeader = "OAuth " + String.Join<String>(", ", headerParamStrings);

            return authHeader;
        }
    }
}