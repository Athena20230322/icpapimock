using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICP.Batch.ExecSendMistake.Services
{
    public class ConfigService
    {
        /// <summary>
        ///三竹發送URL 
        /// </summary>
        public static string MistakeSmsSubmitUrl
        {
            get
            {
                string retVal = ConfigurationManager.AppSettings["MistakeSmsSubmitUrl"];
                if (retVal == null)
                {
                    return "";
                }
                return retVal;
            }
        }
        /// <summary>
        /// 三竹所提供username
        /// </summary>
        public static string SysUsername
        {
            get
            {
                string retVal = ConfigurationManager.AppSettings["SysUsername"];
                if (retVal == null)
                {
                    return "";
                }
                return retVal;
            }
        }
        /// <summary>
        /// 三竹所提供Password
        /// </summary>
        public static string SysPassword
        {
            get
            {
                string retVal = ConfigurationManager.AppSettings["SysPassword"];
                if (retVal == null)
                {
                    return "";
                }
                return retVal;
            }
        }

        /// <summary>
        /// 三竹接收回傳URL
        /// </summary>
        public static string MistakeSmsResponseUrl
        {
            get
            {
                string retVal = ConfigurationManager.AppSettings["ConfigurationManager"];
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
