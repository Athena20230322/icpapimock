using System.Web.Mvc;
using System.Web.Services;
using CommonServiceLocator;
using ICP.Host.APIService.Commands;
using ICP.Library.Models.EinvoiceLibrary;
using ICP.Library.Models.EinvoiceLibrary.DTO;

namespace ICP.Host.APIService.Api
{
    /// <summary>
    ///Einvoice 的摘要描述
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // 若要允許使用 ASP.NET AJAX 從指令碼呼叫此 Web 服務，請取消註解下列一行。
    // [System.Web.Script.Services.ScriptService]
    public class Einvoice : WebService
    {
        private readonly EinvoiceCommand _einvoiceCommand;

        public Einvoice()
        {
            _einvoiceCommand = ServiceLocator.Current.GetInstance<EinvoiceCommand>();
        }

        [WebMethod]
        public BankAccountResultDTO SetBankAccount(BankAccountDTO model)
        {

            return _einvoiceCommand.SetBankAccount(model);
        }

        [WebMethod]
        public CarrierInvTitleResultDTO GetCarrierInvTitle(CarrierInvTitleDTO model)
        {

            return _einvoiceCommand.GetCarrierInvTitle(model);
        }

        [WebMethod]
        public CarrierInvDetailResultDTO GetCarrierInvDetail(CarrierInvDetailDTO model)
        {
            return _einvoiceCommand.GetCarrierInvDetail(model);
        }

        [WebMethod]
        public ChangeCarrierPwdResultDTO ChangeCarrierPwd(ChangeCarrierPwdDTO model)
        {
            return _einvoiceCommand.ChangeCarrierPwd(model);
        }

        [WebMethod]
        public ForgotCarrierPwdResultDTO ForgotCarrierPwd(ForgotCarrierPwdDTO model)
        {
            return _einvoiceCommand.ForgotCarrierPwd(model);
        }

        [WebMethod]
        public CarrierUnderTypeResultDTO GetCarrierUnderType(CarrierUnderTypeDTO model)
        {
            return _einvoiceCommand.GetCarrierUnderType(model);
        }

        [WebMethod]
        public CarrierBarcodeResultDTO GetCarrierBarcode(CarrierBarcodeDTO model)
        {
            return _einvoiceCommand.GetCarrierBarcode(model);
        }

        [WebMethod]
        public RegisterCarrierResultDTO RegisterCarrier(RegisterCarrierDTO model)
        {
            return _einvoiceCommand.RegisterCarrier(model);
        }

        [WebMethod]
        public InvDetailResultDTO GetInvDetail(InvDetailDTO model)
        {
            return _einvoiceCommand.GetInvDetail(model);
        }

        [WebMethod]
        public InvTitleResultDTO GetInvTitle(InvTitleDTO model)
        {
            return _einvoiceCommand.GetInvTitle(model);
        }

    }
}
