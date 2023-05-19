using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace ICP.Modules.Api.Payment.Models.Pos
{
    public class TopUpRefundReq : TopUpCancelReq
    {
        /// <summary>
        /// POS交易序號
        /// </summary>
        [Display(Name = "POS交易序號")]
        [Required(ErrorMessage = "{0} 為必填")]
        [StringLength(6, ErrorMessage = "{0} 最大長度為6")]
        public string PosRefNo { get; set; }
    }
}