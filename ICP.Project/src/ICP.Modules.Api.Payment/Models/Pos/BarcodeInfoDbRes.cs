using ICP.Infrastructure.Core.Models;

namespace ICP.Modules.Api.Payment.Models.Pos
{
    public class BarcodeInfoDbRes : BaseResult
    {
        /// <summary>
        /// 會員編號
        /// </summary>
        public long MID { get; set; }

        /// <summary>
        /// 金流類型 條碼類型為IC時 1: 愛金卡帳戶 2: 銀行快付  條碼類型為88時 1:現金儲值 2:中奬發票儲值
        /// </summary>
        public int PaymentType { get; set; }

        /// <summary>
        /// 付款方式識別碼
        /// </summary>
        public string PayID { get; set; }

        /// <summary>
        /// OP會員編號
        /// </summary>
        public string OpMemberID { get; set; }

        /// <summary>
        /// 愛金卡帳戶
        /// </summary>
        public string IcashAccount { get; set; }

        /// <summary>
        /// 條碼類型 IC：付款條碼　88：儲值條碼
        /// </summary>
        public string BarcodeType { get; set; }

        /// <summary>
        /// 支付識別碼, BarCodeType為IC時才會回傳
        /// </summary>
        public string IcashAccountPayID { get; set; }

        /// <summary>
        /// 支付種類(銀行代碼), BarCodeType為IC時才會回傳
        /// </summary>
        public string BankNo { get; set; }

        /// <summary>
        /// 支付名稱(銀行名稱), BarCodeType為IC時才會回傳
        /// </summary>
        public string BankName { get; set; }
    }
}