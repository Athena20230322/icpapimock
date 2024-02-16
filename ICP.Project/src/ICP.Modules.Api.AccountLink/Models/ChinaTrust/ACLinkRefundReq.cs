using ICP.Infrastructure.Core.Models;
using ICP.Infrastructure.Core.Models.Consts;
using System.ComponentModel.DataAnnotations;

namespace ICP.Modules.Api.AccountLink.Models.ChinaTrust
{
    public class ACLinkRefundReq : ValidatableObject
    {
        /// <summary>
        /// 會員編號
        /// </summary>
        [Required(ErrorMessage = "{0} 為必填")]
        [Range(1, long.MaxValue, ErrorMessage = "{0} 格式不正確")]
        public long MID { get; set; }

        /// <summary>
        /// 身分證字號
        /// </summary>
        [Required(ErrorMessage = "{0} 為必填")]
        [RegularExpression(RegexConst.IDNO, ErrorMessage = "{0} 格式錯誤")]
        public string IDNO { get; set; }

        /// <summary>
        /// 帳號識別碼
        /// </summary>
        [Required(ErrorMessage = "{0} 為必填")]
        [StringLength(20, MinimumLength = 10, ErrorMessage = "{0} 格式不正確")]
        public string INDTAccount { get; set; }

        /// <summary>
        /// 訂單編號
        /// </summary>
        [Required(ErrorMessage = "{0} 為必填")]
        public string TradeNo { get; set; }

        /// <summary>
        /// 與銀行交易的原始交易序號
        /// </summary>
        [Required(ErrorMessage = "{0} 為必填")]
        public string BankTradeNo { get; set; }

        /// <summary>
        /// 來源(1:交易 3:儲值)
        /// </summary>
        [Required(ErrorMessage = "{0} 為必填")]
        public int TradeSource { get; set; }

        /// <summary>
        /// 退款金額
        /// </summary>
        [Required(ErrorMessage = "{0} 為必填")]
        public int RefundAmt { get; set; }

        /// <summary>
        /// 退款時間
        /// </summary>
        [Required(ErrorMessage = "{0} 為必填")]
        public string RefundTime { get; set; }

        /// <summary>
        /// 交易備註
        /// </summary>
        public string TradeNote { get; set; }
    }
}
