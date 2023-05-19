using System.Collections.Specialized;

namespace ICP.Host.Middleware.JCIC.Repositories
{
    public class ConfigRepository
    {
        private NameValueCollection AppSettings
        {
            get
            {
                return System.Configuration.ConfigurationManager.AppSettings;
            }
        }

        /// <summary>
        /// JCIC開關
        /// 0 = 回傳成功
        /// 1 = 執行
        /// 2 = 回傳失敗
        /// </summary>
        public string Switch
        {
            get
            {
                string retVal = AppSettings["Switch"];
                if (retVal == null)
                {
                    return "";
                }

                return retVal.ToString();
            }
        }

        /// <summary>
        /// 聯徵API 登入帳號
        /// </summary>
        public string JCICAccount
        {
            get
            {
                string retVal = AppSettings["JCICAccount"];
                if (retVal == null)
                {
                    return "";
                }

                return retVal.ToString();
            }
        }

        /// <summary>
        /// 聯徵API 登入密碼
        /// </summary>
        public string JCICPassword
        {
            get
            {
                string retVal = AppSettings["JCICPassword"];
                if (retVal == null)
                {
                    return "";
                }

                return retVal.ToString();
            }
        }
    }
}