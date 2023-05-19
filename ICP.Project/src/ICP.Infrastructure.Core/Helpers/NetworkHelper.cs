using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Security;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace ICP.Infrastructure.Core.Helpers
{
    public class NetworkHelper
    {
        /// <summary>
        /// 預設逾時秒數
        /// </summary>
        public int DefaultTimeout { get; set; } = 15;

        /// <summary>
        /// 預設為POST模式
        /// </summary>
        public string DefaultMethod { get; set; } = "POST";

        public string DoRequest(string url, string data, string contentType,
                                int timeoutSeconds, CookieContainer cookieContainer, IDictionary<string, string> headers, int codepage = 65001,string Method="")
        {
            if (string.IsNullOrWhiteSpace(Method))
            {
                Method = DefaultMethod;
            }

            //如果是https請求
            if (url.StartsWith("https", StringComparison.OrdinalIgnoreCase))
            {
                // 忽略憑證
                ServicePointManager.ServerCertificateValidationCallback =
                    new RemoteCertificateValidationCallback((s, cert, chain, errors) => true);
            }
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls | (SecurityProtocolType)768 | (SecurityProtocolType)3072;

            //### 建立HttpWebRequest物件
            HttpWebRequest httpWebRequest = WebRequest.Create(url) as HttpWebRequest;
            httpWebRequest.ProtocolVersion = HttpVersion.Version10;

            //### 指定送出去的方式
            httpWebRequest.Method = Method;
            httpWebRequest.UserAgent = "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.0)";
            httpWebRequest.Accept = "text/html";
            httpWebRequest.Referer = "https://www.google.com.tw";

            //### 設定content type, it is required, otherwise it will not work.
            httpWebRequest.ContentType = contentType;

            //設定Timeout時間(單位毫秒)
            httpWebRequest.Timeout = ((timeoutSeconds > 0) ? timeoutSeconds : DefaultTimeout) * 1000;

            if (cookieContainer != null)
            {
                httpWebRequest.CookieContainer = cookieContainer;
            }

            if (headers != null)
            {
                foreach (var item in headers.Where(x => !string.IsNullOrWhiteSpace(x.Key)))
                {
                    httpWebRequest.Headers.Add(item.Key, item.Value);
                }
            }

            //如Method為POST則將request stream寫入post data
            if (Method == DefaultMethod)
            {
                //### 取得request stream 並且寫入post data
                using (StreamWriter sw = new StreamWriter(httpWebRequest.GetRequestStream()))
                {
                    //### 設定要送出的參數; separated by "&"
                    sw.Write(data ?? string.Empty);
                    sw.Flush();
                    sw.Close();
                }
            }

            //### 取得server的reponse結果
            string receiveData = null;
            using (var httpWebResponse = httpWebRequest.GetResponse())
            {
                using (var sr = new StreamReader(httpWebResponse.GetResponseStream(), Encoding.GetEncoding(codepage)))
                {
                    receiveData = sr.ReadToEnd();
                    sr.Close();
                }
            }

            return receiveData;
        }

        public string DoRequestWithUrlEncode(string url, IDictionary<string, string> data, int codepage = 65001)
        {
            string strData = string.Empty;

            if (data != null)
            {
                var listData = data.Where(x => !string.IsNullOrWhiteSpace(x.Key))
                                   .Select(x => string.Format("{0}={1}", x.Key, HttpUtility.UrlEncode(x.Value)));
                strData = string.Join("&", listData);
            }

            return DoRequest(url, strData, "application/x-www-form-urlencoded", 0, null, null, codepage);
        }

        public string DoRequestWithUrlEncode(string url, IDictionary<string, string> data, int timeoutSeconds, CookieContainer cookieContainer, IDictionary<string, string> headers)
        {
            string strData = string.Empty;

            if (data != null)
            {
                var listData = data.Where(x => !string.IsNullOrWhiteSpace(x.Key))
                                   .Select(x => string.Format("{0}={1}", x.Key, HttpUtility.UrlEncode(x.Value)));
                strData = string.Join("&", listData);
            }

            return DoRequest(url, strData, "application/x-www-form-urlencoded", timeoutSeconds, cookieContainer, headers);
        }

        public string DoRequestWithUrlEncode(string url, System.Collections.Specialized.NameValueCollection data, int timeoutSeconds, CookieContainer cookieContainer, IDictionary<string, string> headers)
        {
            string strData = string.Empty;

            if (data != null)
            {
                var keyValues = new List<string>();
                foreach (string key in data.AllKeys)
                {
                    if (string.IsNullOrWhiteSpace(key)) continue;
                    var value = data[key];
                    keyValues.Add(string.Format("{0}={1}", key, HttpUtility.UrlEncode(value)));
                }
                strData = string.Join("&", keyValues);
            }

            return DoRequest(url, strData, "application/x-www-form-urlencoded" ,timeoutSeconds, cookieContainer, headers);
        }

        public string DoRequestWithJson(string url, string json)
        {
            return DoRequest(url, json, "application/json", 0, null, null);
        }

        public string DoRequestWithJson(string url, string json, int timeoutSeconds, CookieContainer cookieContainer, IDictionary<string, string> headers)
        {
            return DoRequest(url, json, "application/json", timeoutSeconds, cookieContainer, headers);
        }

        public string DoRequestWithGet(string url, CookieContainer cookieContainer, IDictionary<string, string> headers)
        {
            return DoRequest(url , null, "application/x-www-form-urlencoded" ,0 , cookieContainer, headers, 65001,"GET");
        }
    }
}
