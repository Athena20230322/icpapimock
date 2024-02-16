
namespace ICP.Modules.Api.PaymentCenter.Models
{
    public class AddRefundTradeModel : QryRefundTradeModel
    {
        /// <summary>
        /// 退款單流水號
        /// </summary>
        public long SeqNo { get; set; }
    }
}
