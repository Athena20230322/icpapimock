using System.Configuration;
using ICP.Library.Repositories.SystemRepositories;

namespace ICP.Host.APIService.Services
{
    public class ConfigService
    {
        private readonly ConfigKeyValueRepository _configKeyValueRepository;
        public ConfigService(
            ConfigKeyValueRepository configKeyValueRepository
        )
        {
            _configKeyValueRepository = configKeyValueRepository;
        }




        /// <summary>
        /// 電子發票帳號
        /// </summary>
        public string EinvoiceAppID => _configKeyValueRepository.Einvoice_AppID;

        /// <summary>
        /// 電子發票Key
        /// </summary>
        public string EinvoiceApiKey =>_configKeyValueRepository.Einvoice_ApiKey;

        /// <summary>
        ///是否開啟由載具表頭反驗手機條碼機制
        /// </summary>
        public static string CarrierInvMockVerification
        {
            get
            {
                var retVal = ConfigurationManager.AppSettings["CarrierInvMockVerification"];
                return retVal ?? "";
            }
        }

        #region 電子發票Url
        public string Einvoice_Url_InvoiceApiProxy => _configKeyValueRepository.Einvoice_Url_InvoiceApiProxy;
        public string Einvoice_Url_ChangeCarrierPwd => _configKeyValueRepository.Einvoice_Url_ChangeCarrierPwd;
        public string Einvoice_Url_ForgotCarrierPwd => _configKeyValueRepository.Einvoice_Url_ForgotCarrierPwd;
        public string Einvoice_Url_GetCarrierBarcode => _configKeyValueRepository.Einvoice_Url_GetCarrierBarcode;
        public string Einvoice_Url_GetCarrierInvDetail => _configKeyValueRepository.Einvoice_Url_GetCarrierInvDetail;
        public string Einvoice_Url_GetCarrierInvTitle => _configKeyValueRepository.Einvoice_Url_GetCarrierInvTitle;
        public string Einvoice_Url_GetCarrierUnderType => _configKeyValueRepository.Einvoice_Url_GetCarrierUnderType;
        public string Einvoice_Url_GetInvDetail => _configKeyValueRepository.Einvoice_Url_GetInvDetail;
        public string Einvoice_Url_GetInvTitle => _configKeyValueRepository.Einvoice_Url_GetInvTitle;
        public string Einvoice_Url_RegisterCarrier => _configKeyValueRepository.Einvoice_Url_RegisterCarrier;
        public string Einvoice_Url_SetBankAccount => _configKeyValueRepository.Einvoice_Url_SetBankAccount;
        #endregion
       
 
    }
}