using ICP.Library.Repositories.EinvoiceLibrary;
using ICP.Library.Services.Einvoice;

namespace ICP.Library.Services.EinvoiceLibrary
{
    /// <summary>
    /// 電子發票API對外站台服務
    /// </summary>
    public class EinvoiceService
    {
        #region 建構值

        private EinvoiceSoapClient _einvoiceSoapClient;
        

        public EinvoiceService(EinvoiceSoapClient einvoiceSoapClient)
        {
            _einvoiceSoapClient = einvoiceSoapClient;
        }

        #endregion

        #region GOV API

        /// <summary>
        /// 手機條碼綁定金融帳戶或電子支付帳戶
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public BankAccountResultDTO SetBankAccount(BankAccountDTO model)
        {

            return _einvoiceSoapClient.SetBankAccount(model);
        }

        /// <summary>
        /// 載具發票表頭查詢
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public CarrierInvTitleResultDTO GetCarrierInvTitle(CarrierInvTitleDTO model)
        {
            return _einvoiceSoapClient.GetCarrierInvTitle(model);
        }

        /// <summary>
        /// 載具發票明細查詢
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public CarrierInvDetailResultDTO GetCarrierInvDetail(CarrierInvDetailDTO model)
        {
            return _einvoiceSoapClient.GetCarrierInvDetail(model);
        }

        /// <summary>
        /// 變更手機條碼驗證碼
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public ChangeCarrierPwdResultDTO ChangeCarrierPwd(ChangeCarrierPwdDTO model)
        {
            return _einvoiceSoapClient.ChangeCarrierPwd(model);
        }

        /// <summary>
        /// 忘記手機條碼驗證碼
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public ForgotCarrierPwdResultDTO ForgotCarrierPwd(ForgotCarrierPwdDTO model)
        {
            return _einvoiceSoapClient.ForgotCarrierPwd(model);
        }

        /// <summary>
        /// 手機條碼歸戶載具查詢
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public CarrierUnderTypeResultDTO GetCarrierUnderType(CarrierUnderTypeDTO model)
        {
            return _einvoiceSoapClient.GetCarrierUnderType(model);
        }

        /// <summary>
        /// 查詢手機條碼
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public CarrierBarcodeResultDTO GetCarrierBarcode(CarrierBarcodeDTO model)
        {
            return _einvoiceSoapClient.GetCarrierBarcode(model);
        }

        /// <summary>
        /// 手機條碼及驗證碼註冊
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public RegisterCarrierResultDTO RegisterCarrier(RegisterCarrierDTO model)
        {
            return _einvoiceSoapClient.RegisterCarrier(model);
        }

        /// <summary>
        /// 查詢發票明細
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public InvDetailResultDTO GetInvDetail(InvDetailDTO model)
        {
            return _einvoiceSoapClient.GetInvDetail(model);
        }

        /// <summary>
        /// 查詢發票表頭
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public InvTitleResultDTO GetInvTitle(InvTitleDTO model)
        {
            return _einvoiceSoapClient.GetInvTitle(model);
        }


        #endregion

       

    }
}