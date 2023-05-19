using ICP.Infrastructure.Core.Models;
using ICP.Modules.Api.PaymentCenter.Enums;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;


namespace ICP.Modules.Api.PaymentCenter.Models
{
    public class TradeReqModel : ValidatableObject
    {
        /// <summary>
        /// 會員編號
        /// </summary>
        [Display(Name = "會員編號")]
        [Required(ErrorMessage = "{0} 為必填")]
        [Range(1, long.MaxValue, ErrorMessage = "{0} 格式不正確")]
        public long MID { get; set; }

        /// <summary>
        /// 廠商編號
        /// </summary>
        [Display(Name = "廠商編號")]
        [Required(ErrorMessage = "{0} 為必填")]
        [Range(0, long.MaxValue, ErrorMessage = "{0} 格式不正確")]
        public long MerchantID { get; set; }

        /// <summary>
        /// 平台商編號
        /// </summary>
        [Display(Name = "平台商編號")]
        [Range(0, long.MaxValue, ErrorMessage = "{0} 格式不正確")]
        public long PlatformID { get; set; }

        /// <summary>
        /// 交易模式
        /// </summary>
        [Display(Name = "交易模式")]
        [Required(ErrorMessage = "{0} 為必填")]
        [Range((int)eTradeMode.Min, (int)eTradeMode.Max, ErrorMessage = "{0} 格式不正確")]
        public int TradeModeID { get; set; }

        /// <summary>
        /// 付款方式代碼
        /// </summary>
        [Display(Name = "付款方式")]
        [Required(ErrorMessage = "{0} 為必填")]
        [Range((int)ePaymentType.Min, (int)ePaymentType.Max, ErrorMessage = "{0} 格式不正確")]
        public int PaymentTypeID { get; set; }

        /// <summary>
        /// 收單行名稱代碼
        /// </summary>
        [Display(Name = "收單行名稱代碼")]
        [Required(ErrorMessage = "{0} 為必填")]
        [Range(0, 10, ErrorMessage = "{0} 格式不正確")]
        public int PaymentSubTypeID { get; set; }

        /// <summary>
        /// 訂單流水號
        /// </summary>
        public long TradeID { get; set; }

        /// <summary>
        /// 訂單編號
        /// </summary>
        [Display(Name = "訂單編號")]
        [Required(ErrorMessage = "{0} 為必填")]
        [StringLength(20)]
        public string TradeNo { get; set; }

        /// <summary>
        /// 廠商交易編號
        /// </summary>
        [Display(Name = "廠商交易編號")]
        [Required(ErrorMessage = "{0} 為必填")]
        [StringLength(64)]
        public string MerchantTradeNo { get; set; }

        /// <summary>
        /// 交易金額
        /// </summary>
        [Display(Name = "交易金額")]
        [Required(ErrorMessage = "{0} 為必填")]
        [Range(0, int.MaxValue, ErrorMessage = "{0} 格式不正確")]
        public decimal Amount { get; set; }

        /// <summary>
        /// 銀行帳號識別碼
        /// </summary>
        [Display(Name = "銀行帳號識別碼")]
        [StringLength(16)]
        public string AccountID { get; set; }

        /// <summary>
        /// 檢核碼
        /// </summary>
        [Display(Name = "檢核碼")]
        [Required(ErrorMessage = "{0} 為必填")]
        public string CheckMacValue { get; set; }
    }
}
