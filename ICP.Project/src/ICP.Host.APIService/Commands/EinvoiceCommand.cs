using ICP.Host.APIService.Services;
using ICP.Library.Models.EinvoiceLibrary;
using ICP.Library.Models.EinvoiceLibrary.DTO;

namespace ICP.Host.APIService.Commands
{
    public class EinvoiceCommand
    {
        private readonly EinvoiceService _einvoiceService;

        public EinvoiceCommand(EinvoiceService einvoiceService)
        {
            _einvoiceService = einvoiceService;
        }

        #region 載具API

        public BankAccountResultDTO SetBankAccount(BankAccountDTO model)
        {

            return _einvoiceService.SetBankAccount(model);
        }


        public CarrierInvTitleResultDTO GetCarrierInvTitle(CarrierInvTitleDTO model)
        {
            return _einvoiceService.GetCarrierInvTitle(model);
        }


        public CarrierInvDetailResultDTO GetCarrierInvDetail(CarrierInvDetailDTO model)
        {
            return _einvoiceService.GetCarrierInvDetail(model);
        }


        public ChangeCarrierPwdResultDTO ChangeCarrierPwd(ChangeCarrierPwdDTO model)
        {
            return _einvoiceService.ChangeCarrierPwd(model);
        }


        public ForgotCarrierPwdResultDTO ForgotCarrierPwd(ForgotCarrierPwdDTO model)
        {
            return _einvoiceService.ForgotCarrierPwd(model);
        }


        public CarrierUnderTypeResultDTO GetCarrierUnderType(CarrierUnderTypeDTO model)
        {
            return _einvoiceService.GetCarrierUnderType(model);
        }


        public CarrierBarcodeResultDTO GetCarrierBarcode(CarrierBarcodeDTO model)
        {
            return _einvoiceService.GetCarrierBarcode(model);
        }


        public RegisterCarrierResultDTO RegisterCarrier(RegisterCarrierDTO model)
        {
            return _einvoiceService.RegisterCarrier(model);
        }


        public InvDetailResultDTO GetInvDetail(InvDetailDTO model)
        {
            return _einvoiceService.GetInvDetail(model);
        }


        public InvTitleResultDTO GetInvTitle(InvTitleDTO model)
        {
            return _einvoiceService.GetInvTitle(model);
        }

        #endregion
    }
}