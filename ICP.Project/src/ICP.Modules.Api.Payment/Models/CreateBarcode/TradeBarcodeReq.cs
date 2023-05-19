using ICP.Library.Models.AuthorizationApi;
using System.ComponentModel.DataAnnotations;

namespace ICP.Modules.Api.Payment.Models.CreateBarcode
{
    public class TradeBarcodeReq : BaseAuthorizationApiRequest
    {
        ///// <summary>
        ///// 時間戳 格式：2019/01/01 00:00:00
        ///// </summary>
        //[Display(Name = "時間戳")]
        //[Required(ErrorMessage = "{0} 為必填")]
        //public string TimeStamp { get; set; }

        /// <summary>
        /// 付款方式 1：icashpay電支帳戶 2：AccountLink
        /// </summary>
        [Display(Name = "付款方式")]
        [Range(1, 2, ErrorMessage = "{0} 的值有誤")]
        public int PaymentType { get; set; }

        /// <summary>
        /// 付款方式識別碼
        /// </summary>
        [Display(Name = "付款方式識別碼")]
        [Required(ErrorMessage = "{0} 為必填")]
        public string PayID { get; set; }
    }
}