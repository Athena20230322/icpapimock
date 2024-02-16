using System.Collections.Generic;

namespace ICP.Library.Repositories.MemberRepositories
{
    using SystemRepositories;
    using Infrastructure.Core.Utils;
    using System.Configuration;

    /// <summary>
    /// 設定檔 儲存庫
    /// Host Config: Host\App_Data\appSettings.config
    /// Key/IV: ICP.Infrastructure\ICP.Infrastructure.Host.KeyApi\App_Data\appSettings.config
    /// </summary>
    public class MemberConfigRepository: BaseConfigRepository
    {
        public string UserCodeHashKey
        {
            get
            {
                return GetKeyApiValue("UserCodeHashKey");
            }
        }

        public string UserCodeHashIV
        {
            get
            {
                return GetKeyApiValue("UserCodeHashIV");
            }
        }

        public string MemberPasswordKey
        {
            get
            {
                return GetKeyApiValue("MemberPasswordKey");
            }
        }

        public string MemberPasswordIV
        {
            get
            {
                return GetKeyApiValue("MemberPasswordIV");
            }
        }

        public string TradePasswordKey
        {
            get
            {
                return GetKeyApiValue("TradePasswordKey");
            }
        }

        public string TradePasswordIV
        {
            get
            {
                return GetKeyApiValue("TradePasswordIV");
            }
        }

        public string MemberOpenWalletKey
        {
            get
            {
                return GlobalConfigUtil.GetKeyApiValue("MemberOpenWalletKey");
            }
        }

        public string MemberOpenWalletIV
        {
            get
            {
                return GlobalConfigUtil.GetKeyApiValue("MemberOpenWalletIV");
            }
        }

        public string CustomOpenWalletAESKey
        {
            get
            {
                return GlobalConfigUtil.GetKeyApiValue("CustomOpenWalletAESKey");
            }
        }

        public string CustomOpenWalletAESIV
        {
            get
            {
                return GlobalConfigUtil.GetKeyApiValue("CustomOpenWalletAESIV");
            }
        }

        public string CustomOpenWalletHashKey
        {
            get
            {
                return GlobalConfigUtil.GetKeyApiValue("CustomOpenWalletHashKey");
            }
        }

        public string CustomOpenWalletHashIV
        {
            get
            {
                return GlobalConfigUtil.GetKeyApiValue("CustomOpenWalletHashIV");
            }
        }

        public bool ClientOpenWalletSwitch => GlobalConfigUtil.GetAppSetting("ClientOpenWalletSwitch") == "1";

        public string OPWebUIApiAESKey
        {
            get
            {
                return GlobalConfigUtil.GetKeyApiValue("OPWebUIApiAESKey");
            }
        }

        public string OPWebUIApiAESIV
        {
            get
            {
                return GlobalConfigUtil.GetKeyApiValue("OPWebUIApiAESIV");
            }
        }

        public string OPWebUIApiHashKey
        {
            get
            {
                return GlobalConfigUtil.GetKeyApiValue("OPWebUIApiHashKey");
            }
        }

        public string OPWebUIApiHashIV
        {
            get
            {
                return GlobalConfigUtil.GetKeyApiValue("OPWebUIApiHashIV");
            }
        }

        public string IDNOPath
        {
            get
            {
                return ConfigurationManager.AppSettings["IDNO.Path"];
            }
        }

        /// <summary>
        /// 正式環境
        /// 0:否 1:是
        /// </summary>
        public bool ProductMode => GetAppSetting("ProductionMode") == "1";

        /// <summary>
        /// 允許模擬測試
        /// 0:否 1:是
        /// </summary>
        public bool AllowMock => GetAppSetting("AllowMock") == "1";

        public string OverSeaPath
        {
            get
            {
                return ConfigurationManager.AppSettings["OverSeaPath"];
            }
        }

        public string AppRssAESIv
        {
            get
            {
                return GlobalConfigUtil.GetKeyApiValue("AppRssAESIv");
            }

        }

        public string AppRssAESKey
        {
            get
            {
                return GlobalConfigUtil.GetKeyApiValue("AppRssAESKey");
            }
        }

        public string AppRssFrontKey
        {
            get
            {
                return GlobalConfigUtil.GetKeyApiValue("AppRssFrontKey");
            }
        }

        public string AppRssRearKey
        {
            get
            {
                return GlobalConfigUtil.GetKeyApiValue("AppRssRearKey");
            }
        }

        /// <summary>
        /// 未成年上傳圖片路徑
        /// </summary>
        public string Path_TeenagersLegalDetail
        {
            get
            {
                return GetAppSetting("Path:TeenagersLegalDetail");
            }
        }

        #region 電子發票
        /// <summary>
        /// 電子發票Key
        /// </summary>
        public string InvoiceDataHashKey
        {
            get
            {
                return GlobalConfigUtil.GetKeyApiValue("InvoiceDataHashKey");
            }
        }
        /// <summary>
        /// 電子發票Iv
        /// </summary>
        public string InvoiceDataHashIV
        {
            get
            {
                return GlobalConfigUtil.GetKeyApiValue("InvoiceDataHashIV");
            }
        }

        /// <summary>
        /// 電子發票驗證碼加密Key
        /// </summary>
        public string InvoiceVerificationCodeHashKey
        {
            get
            {
                return GlobalConfigUtil.GetKeyApiValue("InvoiceVerificationCodeHashKey");
            }
        }
        
        /// <summary>
        /// 銀行存摺封面圖片路徑
        /// </summary>
        public string Path_BankAccount
        {
            get
            {
                return GetAppSetting("Path:BankAccount");
            }
        }

        /// <summary>
        /// 電子發票驗證碼加密Key Iv
        /// </summary>
        public string InvoiceVerificationCodeHashIv
        {
            get
            {
                return GlobalConfigUtil.GetKeyApiValue("InvoiceVerificationCodeHashIv");
            }
        }
        #endregion
        
        /// APP XML 設定路徑
        /// </summary>
        public string APPXMLPath
        {
            get
            {
                return GetAppSetting("APPXMLPath");
            }
        }

        #region ManageBank
        /// <summary>
        /// 管理銀行 取得 第一銀行 FXML 公鑰
        /// </summary>
        public string ManageBank_FirstBank_FXML_PubKey => GetAppSetting("ManageBank:FirstBank:FXML:PubKey");
        #endregion

        /// <summary>
        /// 內部後台IP
        /// </summary>
        public string AdminIP
        {
            get
            {
                return GetAppSetting("AdminIP");
            }
        }
    }
}