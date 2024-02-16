using ICP.Library.Models.AuthorizationApi;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace ICP.Modules.Api.Payment.Models.Pos
{
    public class RefundReq : BasePosReq
    {
        /// <summary>
        /// 愛金卡交易序號
        /// </summary>
        [Display(Name = "愛金卡交易序號")]
        [Required(ErrorMessage = "{0} 為必填")]
        [StringLength(20, ErrorMessage = "{0} 最大長度為20")]
        public string TransactionID { get; set; }

        /// <summary>
        /// 退貨金額
        /// </summary>
        [Display(Name = "退貨金額")]
        [Range(1, 9999999999.99, ErrorMessage = "{0} 為必填")]
        public decimal Amount { get; set; }

        /// <summary>
        /// 商店編號
        /// </summary>
        [Display(Name = "商店編號")]
        [StringLength(20, ErrorMessage = "{0} 最大長度為20")]
        public string StoreID { get; set; }

        /// <summary>
        /// 商店名稱
        /// </summary>
        [Display(Name = "商店名稱")]
        [StringLength(30, ErrorMessage = "{0} 最大長度為30")]
        public string StoreName { get; set; }

        /// <summary>
        /// POS交易序號
        /// </summary>
        [Display(Name = "POS交易序號")]
        [Required(ErrorMessage = "{0} 為必填")]
        [StringLength(6, ErrorMessage = "{0} 最大長度為6")]
        public string PosRefNo { get; set; }

        /// <summary>
        /// POS編號
        /// </summary>
        [Display(Name = "POS編號")]
        [StringLength(2, ErrorMessage = "{0} 最大長度為2")]
        public string MerchantTID { get; set; }

        /// <summary>
        /// 交易編號
        /// </summary>
        [Display(Name = "交易編號")]
        [Required(ErrorMessage = "{0} 為必填")]
        [StringLength(50, ErrorMessage = "{0} 最大長度為50")]
        public string MerchantTradeNo { get; set; }

        /// <summary>
        /// 交易退貨日期
        /// </summary>
        [Display(Name = "交易退貨日期")]
        [Required(ErrorMessage = "{0} 為必填")]
        [StringLength(20, ErrorMessage = "{0} 最大長度為20")]
        [DataType(DataType.DateTime, ErrorMessage = "{0} 應為日期格式")]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd HH:mm:ss}")]
        public string MerchantTradeDate { get; set; }
    }
}