using ICP.Infrastructure.Core.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICP.Modules.Api.PaymentCenter.Models
{
    public class RefundReqModel : ValidatableObject
    {
        /// <summary>
        /// 廠商編號
        /// </summary>
        [Display(Name = "廠商編號")]
        [Required(ErrorMessage = "{0} 為必填")]
        [Range(1, long.MaxValue, ErrorMessage = "{0} 格式不正確")]
        public long MerchantID { get; set; }

        /// <summary>
        /// 平台商編號
        /// </summary>
        [Display(Name = "平台商編號")]
        [Range(0, long.MaxValue, ErrorMessage = "{0} 格式不正確")]
        public long PlatformID { get; set; }

        /// <summary>
        /// 訂單流水號
        /// </summary>
        [Display(Name = "訂單流水號")]
        [Required(ErrorMessage = "{0} 為必填")]
        [Range(1, long.MaxValue, ErrorMessage = "{0} 格式不正確")]
        public long PaymentCenterTradeID { get; set; }

        /// <summary>
        /// Payment訂單編號
        /// </summary>
        [Display(Name = "Payment訂單編號")]
        [Required(ErrorMessage = "{0} 為必填")]
        [StringLength(20)]
        public string TradeNo { get; set; }

        /// <summary>
        /// 廠商訂單編號
        /// </summary>
        [Display(Name = "廠商訂單編號")]
        [Required(ErrorMessage = "{0} 為必填")]
        [StringLength(64)]
        public string MerchantTradeNo { get; set; }

        /// <summary>
        /// 交易金額
        /// </summary>
        [Display(Name = "交易金額")]
        //[Required(ErrorMessage = "{0} 為必填")]
        [Range(0, int.MaxValue, ErrorMessage = "{0} 格式不正確")]
        public decimal? Amount { get; set; }

        /// <summary>
        /// 檢核碼
        /// </summary>
        [Display(Name = "檢核碼")]
        [Required(ErrorMessage = "{0} 為必填")]
        public string CheckMacValue { get; set; }
    }
}
