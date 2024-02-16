using Microsoft.VisualBasic;
using Microsoft.VisualBasic.CompilerServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace ICP.Infrastructure.Core.Web.Extensions
{
    public static class HttpRequestExtensions
    {
        private static long ip2long(string ip)
        {
            if (string.IsNullOrEmpty(ip)) return 0;

            char[] separator = new char[] { '.' };
            string[] items = ip.Split(separator);
            return long.Parse(items[0]) << 24
                    | long.Parse(items[1]) << 16
                    | long.Parse(items[2]) << 8
                    | long.Parse(items[3]);
        }

        public static long RealIP(this HttpRequestBase httpRequest)
        {
            return ip2long(httpRequest.RemoteRealIP());
        }

        public static long RealIP(this HttpRequest httpRequest)
        {
            return ip2long(httpRequest.RemoteRealIP());
        }

        public static long ProxyIP(this HttpRequestBase httpRequest)
        {
            return ip2long(httpRequest.RemoteProxyIP());
        }

        public static long ProxyIP(this HttpRequest httpRequest)
        {
            return ip2long(httpRequest.RemoteProxyIP());
        }

        /// <summary>
        /// Get client remote IP address.
        /// </summary>
        public static string RemoteRealIP(this HttpRequestBase httpRequest)
        {
            string szRemoteIP = string.Empty;

            if (httpRequest != null)
            {
                var oRequest = httpRequest;
                var szKeys = new string[]
                {
                        "HTTP_CLIENT_IP",
                        "HTTP_X_FORWARDED_FOR",
                        "HTTP_X_FORWARDED",
                        "HTTP_X_CLUSTER_CLIENT_IP",
                        "HTTP_FORWARDED_FOR",
                        "HTTP_FORWARDED",
                        "REMOTE_ADDR",
                        "HTTP_VIA"
                };

                foreach (string szKey in szKeys)
                {
                    if (oRequest.ServerVariables.AllKeys.Contains(szKey))
                    {
                        szRemoteIP = oRequest.ServerVariables[szKey];
                        break;
                    }
                }

                if (szRemoteIP == "::1") szRemoteIP = "127.0.0.1";
            }

            return szRemoteIP;
        }
        
        /// <summary>
        /// Get client remote IP address.
        /// </summary>
        public static string RemoteRealIP(this HttpRequest httpRequest)
        {
            string szRemoteIP = string.Empty;

            if (httpRequest != null)
            {
                var oRequest = httpRequest;
                var szKeys = new string[]
                {
                        "HTTP_CLIENT_IP",
                        "HTTP_X_FORWARDED_FOR",
                        "HTTP_X_FORWARDED",
                        "HTTP_X_CLUSTER_CLIENT_IP",
                        "HTTP_FORWARDED_FOR",
                        "HTTP_FORWARDED",
                        "REMOTE_ADDR",
                        "HTTP_VIA"
                };

                foreach (string szKey in szKeys)
                {
                    if (oRequest.ServerVariables.AllKeys.Contains(szKey))
                    {
                        szRemoteIP = oRequest.ServerVariables[szKey];
                        break;
                    }
                }

                if (szRemoteIP == "::1") szRemoteIP = "127.0.0.1";
            }

            return szRemoteIP;
        }

        public static string RemoteProxyIP(this HttpRequestBase httpRequest)
        {
            string szRemoteIP = string.Empty;

            if (httpRequest != null)
            {
                szRemoteIP = httpRequest.UserHostAddress;

                if (szRemoteIP == "::1") szRemoteIP = "127.0.0.1";
            }

            return szRemoteIP;
        }

        public static string RemoteProxyIP(this HttpRequest httpRequest)
        {
            string szRemoteIP = string.Empty;

            if (httpRequest != null)
            {
                szRemoteIP = httpRequest.UserHostAddress;

                if (szRemoteIP == "::1") szRemoteIP = "127.0.0.1";
            }

            return szRemoteIP;
        }

        ///// <summary>
        ///// Convert IP to Long
        ///// </summary>
        ///// <param name="httpRequest"></param>
        ///// <returns></returns>
        //public static long IPConvertToLong(this HttpRequestBase httpRequest)
        //{
        //    string ip = httpRequest.RemoteRealIP();
        //    return ipConvertToLong(ip);
        //}

        ///// <summary>
        ///// Convert IP to Long
        ///// </summary>
        ///// <param name="httpRequest"></param>
        ///// <returns></returns>
        //public static long IPConvertToLong(this HttpRequest httpRequest)
        //{
        //    string ip = httpRequest.RemoteRealIP();
        //    return ipConvertToLong(ip);
        //}

        private static long ipConvertToLong(string IPAddress)
        {
            int num = 0;
            decimal num1 = new decimal();
            if (Information.UBound(Strings.Split(IPAddress, ".", -1, CompareMethod.Binary), 1) != 3)
            {
                return 0;
            }
            int num2 = 1;
            do
            {
                int num3 = Strings.InStr(checked(num + 1), IPAddress, ".", CompareMethod.Text);
                if (num2 == 4)
                {
                    num3 = checked(Strings.Len(IPAddress) + 1);
                }
                int integer = Conversions.ToInteger(Conversion.Int(Strings.Mid(IPAddress, checked(num + 1), checked(checked(num3 - num) - 1))));
                if (integer > 255)
                {
                    return 0;
                }
                num = num3;
                num1 = new decimal((double)(integer % 256) * Math.Pow(256, (double)(checked(4 - num2))) + Convert.ToDouble(num1));
                num2 = checked(num2 + 1);
            }
            while (num2 <= 4);
            return (long)num1;
        }
    }
}
