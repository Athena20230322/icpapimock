namespace ICP.Modules.Api.Payment.Models.ChargeBack
{
    public class ChargeBackSendToPaymentCenterReq : CancelSendToPaymentCenterReq
    {
        /// <summary>
        /// 退款金額
        /// </summary>
        public decimal Amount { get; set; }
    }
}