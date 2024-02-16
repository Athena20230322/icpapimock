using System.Configuration;

namespace ICP.Batch.PICChargeFeeResult.Services
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

        /// <summary>
        ///  發票日期(起) (日期格式為yyyyMMdd，限制查詢一天資料量)
        /// </summary>
        public static string InvoiceDateFrom
        {
            get
            {
                if (string.IsNullOrWhiteSpace(_InvoiceDateFrom))
                {
                    _InvoiceDateFrom = GetLocalSetting("InvoiceDateFrom");
                }
                return _InvoiceDateFrom;
            }
        }
        private static string _InvoiceDateFrom;

        /// <summary>
        ///  發票日期(迄) (日期格式為yyyyMMdd，限制查詢一天資料量)
        /// </summary>
        public static string InvoiceDateTo
        {
            get
            {
                if (string.IsNullOrWhiteSpace(_InvoiceDateTo))
                {
                    _InvoiceDateTo = GetLocalSetting("InvoiceDateTo");
                }
                return _InvoiceDateTo;
            }
        }
        private static string _InvoiceDateTo;

        /// <summary>
        ///  買方類型 (0: 全部、 1: 只傳B2B 、2: 只傳B2C ，未使用系統預設為：0.全部)
        /// </summary>
        public static string BuyerType
        {
            get
            {
                if (string.IsNullOrWhiteSpace(_BuyerType))
                {
                    _BuyerType = GetLocalSetting("BuyerType");
                }
                return _BuyerType;
            }
        }
        private static string _BuyerType;

    }
}
