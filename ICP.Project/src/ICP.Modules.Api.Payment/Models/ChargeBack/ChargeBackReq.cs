using ICP.Library.Models.AuthorizationApi;
using System;
using System.ComponentModel.DataAnnotations;

namespace ICP.Modules.Api.Payment.Models.ChargeBack
{
    public class ChargeBackReq : BaseAuthorizationApiRequest
    {
        /// <summary>
        /// 廠商編號
        /// </summary>
        [Display(Name = "廠商編號")]
        [Range(0, long.MaxValue, ErrorMessage = "{0} 為必填")]
        public long MerchantID { get; set; }

        /// <summary>
        /// 平台編號
        /// </summary>
        public long PlatformID { get; set; }

        /// <summary>
        /// 愛金卡交易序號
        /// </summary>
        public string TransactionID { get; set; }

        /// <summary>
        /// 退貨金額
        /// </summary>
        public decimal Amount { get; set; }

        /// <summary>
        /// 商店ID
        /// </summary>
        public string StoreID { get; set; }

        /// <summary>
        /// 商店名稱
        /// </summary>
        public string StoreName { get; set; }

        /// <summary>
        /// 交易編號
        /// </summary>
        [Display(Name = "交易編號")]
        [Required(ErrorMessage = "{0} 為必填")]
        public string MerchantTradeNo { get; set; }

        /// <summary>
        /// 交易退貨日期
        /// </summary>
        [Display(Name = "交易退貨日期")]
        [Required(ErrorMessage = "{0} 為必填")]
        public DateTime? MerchantTradeDate { get; set; }

        /// <summary>
        /// 收銀機交易序號
        /// </summary>
        public string PosRefNo { get; set; }

        /// <summary>
        /// 商家的終端機編號或POS機編號
        /// </summary>
        public string MerchantTID { get; set; }

        /// <summary>
        /// 檢核碼
        /// </summary>
        public string CheckMacValue { get; set; }

        /// <summary>
        /// PaymentCenter的訂單流水號
        /// </summary>
        public long PaymentCenterTradeID { get; set; }

        /// <summary>
        /// 標記此交易為取消交易(沖正)，不需帶退貨金額
        /// </summary>
        public int ForCancel { get; set; }

        /// <summary>
        /// 退還紅利折抵金額
        /// </summary>
        public decimal BonusAmt { get; set; }

        /// <summary>
        /// 退還紅利折抵點數
        /// </summary>
        public decimal DebitPoint { get; set; }
    }
}
