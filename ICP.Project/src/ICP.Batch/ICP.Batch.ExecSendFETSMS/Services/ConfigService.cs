using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICP.Batch.ExecSendFETSMS.Services
{
    public class ConfigService
    {
        /// <summary>
        /// 送致遠傳的URL
        /// </summary>
        public static string FETSmsSubmitUrl
        {
            get
            {
                string retVal = ConfigurationManager.AppSettings["FETSmsSubmitUrl"];

                if (retVal == null)
                {
                    return "";
                }

                return retVal;
            }
        }

        /// <summary>
        /// 遠傳提供的ID
        /// </summary>
        public static string SysId
        {
            get
            {
                string retVal = ConfigurationManager.AppSettings["SysId"];

                if (retVal == null)
                {
                    return "";
                }

                return retVal;
            }
        }

        /// <summary>
        /// 遠傳提供的Address
        /// </summary>
        public static string SrcAddress
        {
            get
            {
                string retVal = ConfigurationManager.AppSettings["SrcAddress"];

                if (retVal == null)
                {
                    return "";
                }

                return retVal;
            }
        }

        /// <summary>
        /// 遠傳-訊息有效時間
        /// </summary>
        public static string ExpiryMinutes
        {
            get
            {
                string retVal = ConfigurationManager.AppSettings["ExpiryMinutes"];

                if (retVal == null)
                {
                    return "";
                }

                return retVal;
            }
        }

        /// <summary>
        /// 遠傳-是否為長簡訊
        /// </summary>
        public static string LongSmsFlag
        {
            get
            {
                string retVal = ConfigurationManager.AppSettings["LongSmsFlag"];

                if (retVal == null)
                {
                    return "";
                }

                return retVal;
            }
        }

        /// <summary>
        /// 遠傳-是否Falsh SMS發送
        /// </summary>
        public static string FlashFlag
        {
            get
            {
                string retVal = ConfigurationManager.AppSettings["FlashFlag"];

                if (retVal == null)
                {
                    return "";
                }

                return retVal;
            }
        }

        //<add key ="FirstFailFlag" value="false"/>
        /// <summary>
        /// 遠傳-是否需要傳回相對應的DR
        /// </summary>
        public static string DrFlag
        {
            get
            {
                string retVal = ConfigurationManager.AppSettings["DrFlag"];

                if (retVal == null)
                {
                    return "";
                }

                return retVal;
            }
        }

        /// <summary>
        /// 遠傳-是否需要傳回Fist Fail狀態通知
        /// </summary>
        public static string FirstFailFlag
        {
            get
            {
                string retVal = ConfigurationManager.AppSettings["FirstFailFlag"];

                if (retVal == null)
                {
                    return "";
                }

                return retVal;
            }
        }

        /// <summary>
        /// WebRequest Timeout設定值
        /// </summary>
        public static int WebRequestTimeout
        {
            get
            {
                string retVal = ConfigurationManager.AppSettings["WebRequestTimeout"];
                if (retVal == null)
                {
                    return 0;
                }
                return Convert.ToInt32(retVal);
            }
        }
    }
}
