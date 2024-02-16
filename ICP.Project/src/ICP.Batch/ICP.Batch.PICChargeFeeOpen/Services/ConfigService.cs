using System.Configuration;

namespace ICP.Batch.PICChargeFeeOpen.Services
{
    public class ConfigService
    {
        private static string GetLocalSetting(string key)
        {
            return ConfigurationManager.AppSettings[key];
        }

        /// <summary>
        ///  PIC Web Service URL 路徑
        /// </summary>
        public static string PicWSUrl
        {
            get
            {
                if (string.IsNullOrWhiteSpace(_PicWSUrl))
                {
                    _PicWSUrl = GetLocalSetting("PicWSUrl");
                }
                return _PicWSUrl;
            }
        }
        private static string _PicWSUrl;

        /// <summary>
        ///  營業人編號
        /// </summary>
        public static string Identifier
        {
            get
            {
                if (string.IsNullOrWhiteSpace(_Identifier))
                {
                    _Identifier = GetLocalSetting("Identifier");
                }
                return _Identifier;
            }
        }
        private static string _Identifier;

        /// <summary>
        ///  帳號
        /// </summary>
        public static string Account
        {
            get
            {
                if (string.IsNullOrWhiteSpace(_Account))
                {
                    _Account = GetLocalSetting("Account");
                }
                return _Account;
            }
        }
        private static string _Account;

        /// <summary>
        ///  密碼
        /// </summary>
        public static string Password
        {
            get
            {
                if (string.IsNullOrWhiteSpace(_Password))
                {
                    _Password = GetLocalSetting("Password");
                }
                return _Password;
            }
        }
        private static string _Password;

        /// <summary>
        ///  錯誤信發送人員
        /// </summary>
        public static string ErrMailTo
        {
            get
            {
                if (string.IsNullOrWhiteSpace(_ErrMailTo))
                {
                    _ErrMailTo = GetLocalSetting("ErrMailTo");
                }
                return _ErrMailTo;
            }
        }
        private static string _ErrMailTo;

    }
}
