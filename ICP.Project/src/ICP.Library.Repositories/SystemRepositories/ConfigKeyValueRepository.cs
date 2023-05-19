using System.Collections.Generic;

namespace ICP.Library.Repositories.SystemRepositories
{
    using Infrastructure.Abstractions.DbUtil;
    using Models.SystemModels;
    using System;

    /// <summary>
    /// DB設定檔 儲存庫
    /// [ICP_Member]
    /// 新增 ausp_Member_Admin_System_AddConfigKeyValue_I
    /// 更新 ausp_Member_Admin_System_UpdateConfigKeyValue_U
    /// </summary>
    public class ConfigKeyValueRepository
    {
        #region 不公開
        //單執行序使用 Dictionary, 多執行序請用 ConcurrentDictionary
        private Dictionary<string, string> _dict;

        private readonly IDbConnectionPool _dbConnectionPool = null;
        #endregion

        #region 公開
        public ConfigKeyValueRepository(IDbConnectionPool dbConnectionPool)
        {
            _dict = new Dictionary<string, string>();
            _dbConnectionPool = dbConnectionPool;
        }

        public ConfigKeyValue GetConfig(string Key)
        {
            var db = _dbConnectionPool.Create(DatabaseName.ICP_Member);

            string sql = "EXEC ausp_Member_System_GetConfigKeyValue_S";

            var args = new
            {
                Key
            };

            sql += db.GenerateParameter(args);

            return db.QuerySingleOrDefault<ConfigKeyValue>(sql, args);
        }

        public string GetConfigKeyValue(string Key)
        {
            if (_dict.ContainsKey(Key)) return _dict[Key];

            var model = GetConfig(Key);

            if (model == null) return null;

            _dict.Add(Key, model.Value);

            return model.Value;
        }
        #endregion

        #region OP ClientApi
        /// <summary>
        /// OP ClientApi 廠商代碼
        /// </summary>
        public string op_client_id
        {
            get
            {
                return GetConfigKeyValue("op:clientApi:id");
            }
        }

        /// <summary>
        /// OP ClientApi 廠商密碼
        /// </summary>
        public string op_client_mima
        {
            get
            {
                return GetConfigKeyValue("op:clientApi:mima");
            }
        }

        /// <summary>
        /// OP ClientApi 網域
        /// </summary>
        public string op_client_domain
        {
            get
            {
                return GetConfigKeyValue("op:clientApi:domain");
            }
        }

        /// <summary>
        /// OP Client Zip PW
        /// </summary>
        public string op_client_zippw
        {
            get
            {
                return GetConfigKeyValue("op:client:zippw");
            }
        }
        #endregion

        #region OP CustomApi
        /// <summary>
        /// OP CustomApi 網域
        /// </summary>
        public string op_custom_domain
        {
            get
            {
                return GetConfigKeyValue("op:customApi:domain");
            }
        }
        #endregion

        #region OP FTP
        /// <summary>
        /// OP FTP 廠商代碼
        /// </summary>
        public string op_ftp_account
        {
            get
            {
                return GetConfigKeyValue("op:ftp:account");
            }
        }

        /// <summary>
        /// OP FTP 廠商密碼
        /// </summary>
        public string op_ftp_mima
        {
            get
            {
                return GetConfigKeyValue("op:ftp:mima");
            }
        }

        /// <summary>
        /// OP FTP 位置
        /// </summary>
        public string op_ftp_address
        {
            get
            {
                return GetConfigKeyValue("op:ftp:address");
            }
        }
        #endregion

        #region MailSend
        /// <summary>
        /// SMTP 預設寄件者
        /// </summary>
        public string SMTP_DefaultMailFrom
        {
            get
            {
                return  GetConfigKeyValue("SMTP:DefaultMailFrom");
            }
        }

        /// <summary>
        /// SMTP帳號
        /// </summary>
        public string SMTP_User
        {
            get
            {
                return GetConfigKeyValue("SMTP:User");
            }
        }
        /// <summary>
        /// SMTP密碼
        /// </summary>
        public string SMTP_Password
        {
            get
            {
                return GetConfigKeyValue("SMTP:Password");
            }
        }
        /// <summary>
        /// SMTP_IP
        /// </summary>
        public string SMTP_IP
        {
            get
            {
                return GetConfigKeyValue("SMTP:IP");
            }
        }
        /// <summary>
        /// SMTP錯誤信地址
        /// </summary>
        public string SMTP_ErrMailAddress
        {
            get
            {
                return GetConfigKeyValue("SMTP:ErrMailAddress");
            }
        }

        /// <summary>
        /// SMTP錯誤信寄信者
        /// </summary>
        public string SMTP_ErrorMailFrom
        {
            get
            {
                return GetConfigKeyValue("SMTP:ErrorMailFrom");
            }
        }

        /// <summary>
        /// SMTP_Port
        /// </summary>
        public int SMTP_Port
        {

            get
            {
                return Int32.Parse(GetConfigKeyValue("SMTP:Port"));
            }
        }
        #endregion

        #region MistakeSMS
        /// <summary>
        /// MistakeSMS廠商帳號
        /// </summary>
        public string MTSMS_username
        {
            get
            {
                return GetConfigKeyValue("MTSMS:username");
            }
        }

        /// <summary>
        /// MistakeSMS廠商密碼
        /// </summary>
        public string MTSMS_password
        {
            get
            {
                return GetConfigKeyValue("MTSMS:password");
            }
        }

        /// <summary>
        /// MistakeSMS送出站台連結
        /// </summary>
        public string MTSMS_submiturl
        {
            get
            {
                return GetConfigKeyValue("MTSMS:submiturl");
            }
        }
        /// <summary>
        /// MistakeSMS_CallBack連結
        /// </summary>
        public string MTSMS_responseurl
        {
            get
            {
                return GetConfigKeyValue("MTSMS:responseurl");
            }
        }
        #endregion

        #region AppRssPush
        /// <summary>
        /// AppRssPush發送位置
        /// </summary>
        public string AppRss_Push_Url => GetConfigKeyValue("AppRss:Push:Url");


        #endregion

        #region Environment

        /// <summary>
        /// 運行環境代碼
        /// </summary>
        public string Environment_Code => GetConfigKeyValue("Environment:Code");

        #endregion

        #region Einvoice
        /// <summary>
        /// 電子發票帳號
        /// </summary>
        public string Einvoice_AppID => GetConfigKeyValue("Einvoice:AppID");
        /// <summary>
        /// 電子發票Key
        /// </summary>
        public string Einvoice_ApiKey => GetConfigKeyValue("Einvoice:ApiKey");

        /// <summary>
        /// 電子發票歸戶代碼
        /// </summary>
        public string Einvoice_CardType => GetConfigKeyValue("Einvoice:Type:CardType");

        #region EinvoiceUrl
        /// <summary>
        /// URL 代理 電子發票載具
        /// </summary>
        public string Einvoice_Url_InvoiceApiProxy => GetConfigKeyValue("Einvoice:Url:InvoiceApiProxy");
        /// <summary>
        /// URL 變更 手機條碼驗證碼
        /// </summary>
        public string Einvoice_Url_ChangeCarrierPwd => GetConfigKeyValue("Einvoice:Url:ChangeCarrierPwd");
        /// <summary>
        /// URL 忘記手機條碼驗證碼
        /// </summary>
        public string Einvoice_Url_ForgotCarrierPwd => GetConfigKeyValue("Einvoice:Url:ForgotCarrierPwd");
        /// <summary>
        /// URL　查詢 手機條碼
        /// </summary>
        public string Einvoice_Url_GetCarrierBarcode => GetConfigKeyValue("Einvoice:Url:GetCarrierBarcode");
        /// <summary>
        /// URL 查詢 載具發票明細
        /// </summary>
        public string Einvoice_Url_GetCarrierInvDetail => GetConfigKeyValue("Einvoice:Url:GetCarrierInvDetail");
        /// <summary>
        /// URL 查詢 載具發票表頭
        /// </summary>
        public string Einvoice_Url_GetCarrierInvTitle => GetConfigKeyValue("Einvoice:Url:GetCarrierInvTitle");
        /// <summary>
        /// URL 查詢 手機條碼歸戶載具
        /// </summary>
        public string Einvoice_Url_GetCarrierUnderType => GetConfigKeyValue("Einvoice:Url:GetCarrierUnderType");
        /// <summary>
        /// URL 查詢 發票明細
        /// </summary>
        public string Einvoice_Url_GetInvDetail => GetConfigKeyValue("Einvoice:Url:GetInvDetail");
        /// <summary>
        /// URL 查詢 發票表頭
        /// </summary>
        public string Einvoice_Url_GetInvTitle => GetConfigKeyValue("Einvoice:Url:GetInvTitle");
        /// <summary>
        /// URL 查詢 發票表頭
        /// </summary>
        public string Einvoice_Url_RegisterCarrier => GetConfigKeyValue("Einvoice:Url:RegisterCarrier");
        /// <summary>
        /// URL 手機條碼綁定金融帳戶或電子支付帳戶
        /// </summary>
        public string Einvoice_Url_SetBankAccount => GetConfigKeyValue("Einvoice:Url:SetBankAccount");

        /// <summary>
        /// URL 接收歸戶訊息回傳(小平台)
        /// </summary>
        public string Einvoice_Url_BindCallBack => GetConfigKeyValue("Einvoice:Url:BindCallBack");

        /// <summary>
        /// URL 歸戶聯結網址
        /// </summary>
        public string Einvoice_Url_APMEMBERVAN => GetConfigKeyValue("Einvoice:Url:APMEMBERVAN");
        #endregion

        /// <summary>
        /// Type 載具類別編號
        /// </summary>
        public string Einvoice_Type_CardType=>GetConfigKeyValue("Einvoice:Type:CardType");

        /// <summary>
        /// ICP 統一編號
        /// </summary>
        public string Einvoice_ICPIdentifier=>GetConfigKeyValue("Einvoice:ICPIdentifier");
        #endregion

        #region ManageBank
        /// <summary>
        /// 管理銀行 取得 第一銀行 FXML URL
        /// </summary>
        public string ManageBank_FirstBank_FXML_Url => GetConfigKeyValue("ManageBank:FirstBank:FXML:Url");
        #endregion
    }
}
