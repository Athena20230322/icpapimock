using ICP.Infrastructure.Core.Utils;
using System.Configuration;

namespace ICP.Modules.Api.AccountLink.Services
{
    /// <summary>
    /// 其它共用config設定
    /// </summary>
    public class CommonConfigService
    {
        /// <summary>
        /// 網站Domain
        /// </summary>
        public static string CurrentDomain
        {
            get
            {
                return ConfigurationManager.AppSettings["CurrentDomain"];
            }
        }

        /// <summary>
        /// 會員網站Domain
        /// </summary>
        public static string MemberDomain
        {
            get
            {
                return ConfigurationManager.AppSettings["MemberDomain"];
            }
        }

        /// <summary>
        /// AccountLink HashKey (內部站台傳遞資料用)
        /// </summary>
        public static string ACLinkHashKey
        {
            get
            {
                return GlobalConfigUtil.ACLink_HashKey;
            }
        }

        /// <summary>
        /// AccountLink HashIV (內部站台傳遞資料用)
        /// </summary>
        public static string ACLinkHashIV
        {
            get
            {
                return GlobalConfigUtil.ACLink_HashIV;
            }
        }

        /// <summary>
        /// 逾時秒數
        /// </summary>
        public static int TimeoutSec
        {
            get
            {
                return int.Parse(ConfigurationManager.AppSettings["TimeoutSec"]);
            }
        }

        /// <summary>
        /// 綁定結果前台路徑
        /// </summary>
        public static string BindWebResultUrl
        {
            get
            {
                return ConfigurationManager.AppSettings["BindWebResultUrl"];
            }
        }
    }
}
