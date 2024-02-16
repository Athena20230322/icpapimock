using ICP.Library.Models.AuthorizationApi;
using System.ComponentModel.DataAnnotations;

namespace ICP.Modules.Api.Payment.Models.ChargeOnline
{
    public class ChargeOnlineRequest: BaseAuthorizationApiRequest
    {
        /// <summary>
        /// 廠商編號
        /// </summary>
        [Display(Name = "廠商編號")]
        [Range(1, long.MaxValue, ErrorMessage = "{0} 為必填")]
        public long MerchantID { get; set; }

        /// <summary>
        /// 平台編號
        /// </summary>
        public long PlatformID { get; set; }

        /// <summary>
        /// 交易編號
        /// </summary>
        [Display(Name = "交易編號")]
        [Required(ErrorMessage = "{0} 為必填")]
        [StringLength(64,ErrorMessage ="{0} 最多64個字元")]
        public string MerchantTradeNo { get; set; }

        /// <summary>
        /// APP簽章Data
        /// </summary>
        [Display(Name = " APP簽章Data")]
        [Required(ErrorMessage = "{0} 為必填")]
        public string AppData { get; set; }
    }
}
