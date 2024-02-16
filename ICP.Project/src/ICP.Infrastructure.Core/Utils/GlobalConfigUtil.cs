using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICP.Infrastructure.Core.Utils
{
    public static class GlobalConfigUtil
    {
        public static string GetAppSetting(string key)
        {
            return ConfigurationManager.AppSettings[key];
        }

        public static string GetKeyApiValue(string key)
        {
            using (var api = new KeyApiServiceReference.KeyApiSoapClient())
            {
                return api.GetValue(key);
            }
        }

        public static string DbProxy_Url
        {
            get
            {
                return GetAppSetting("DbProxy_Url");
            }
        }

        public static string DbProxy_HashKey
        {
            get
            {
                return GetAppSetting("DbProxy_HashKey");
            }
        }

        public static string DbProxy_HashIv
        {
            get
            {
                return GetAppSetting("DbProxy_HashIv");
            }
        }

        public static bool DbProxy_Enable
        {
            get
            {
                return ((GetAppSetting("DbProxy_Enable") ?? string.Empty) == "1");
            }
        }

        public static string Host_Member_Domain
        {
            get
            {
                return GetAppSetting("Host_Member_Domain");
            }
        }       

        /// <summary>
        /// 系統內部串接加密HashKey
        /// </summary>
        public static string SYS_HashKey
        {
            get
            {
                return GetKeyApiValue("SYS_HashKey");
            }
        }

        public static string Host_Middleware_AccountLink_Domain
        {
            get
            {
                return GetAppSetting("Host_Middleware_AccountLink_Domain");
            }
        }

        /// <summary>
        /// 系統內部串接加密HashIV
        /// </summary>
        public static string SYS_HashIV
        {
            get
            {
                return GetKeyApiValue("SYS_HashIV");
            }
        }

        /// <summary>
        /// Payment站臺Domain
        /// </summary>
        public static string Host_Payment_Domain
        {
            get
            {
                return GetAppSetting("Host_Payment_Domain");
            }
        }

        /// <summary>
        /// PaymentCenter站臺Domain
        /// </summary>
        public static string Host_PaymentCenter_Domain
        {
            get
            {
                return GetAppSetting("Host_PaymentCenter_Domain");
            }
        }

        public static string Environment
        {
            get
            {
                return GetAppSetting("Environment");
            }
        }

        public static string ACLink_HashKey
        {
            get
            {
                return GetKeyApiValue("ACLink_HashKey");
            }
        }

        public static string ACLink_HashIV
        {
            get
            {
                return GetKeyApiValue("ACLink_HashIV");
            }
        }

        /// <summary>
        /// Open Wallet Server Domain
        /// </summary>
        public static string Host_OW_Domain
        {
            get
            {
                return GetAppSetting("Host_OW_Domain");
            }
        }

        public static string FingerPrintPasswordHashKey
        {
            get
            {
                return GetAppSetting("FingerPrintPasswordHashKey");
            }
        }

        public static string FingerPrintPasswordHashIV
        {
            get
            {
                return GetAppSetting("FingerPrintPasswordHashIV");
            }
        }
    }
}
