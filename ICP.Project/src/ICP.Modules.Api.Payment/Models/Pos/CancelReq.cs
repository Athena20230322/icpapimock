using ICP.Library.Models.AuthorizationApi;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace ICP.Modules.Api.Payment.Models.Pos
{
    public class CancelReq : BasePosReq
    {
        /// <summary>
        /// 愛金卡交易序號
        /// </summary>
        [Display(Name = "愛金卡交易序號")]
        [Required(ErrorMessage = "{0} 為必填")]
        [StringLength(20, ErrorMessage = "{0} 最大長度為20")]
        public string TransactionID { get; set; }

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
        /// 交易取消日期
        /// </summary>
        [Display(Name = "交易取消日期")]
        [Required(ErrorMessage = "{0} 為必填")]
        [StringLength(20, ErrorMessage = "{0} 最大長度為20")]
        [DataType(DataType.DateTime, ErrorMessage = "{0} 應為日期格式")]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd HH:mm:ss}")]
        public string MerchantTradeDate { get; set; }
    }
}