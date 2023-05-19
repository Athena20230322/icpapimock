using ICP.Library.Models.AuthorizationApi;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace ICP.Modules.Api.Payment.Models.Pos
{
    public class QueryMemberInfoReq : BasePosReq
    {
        /// <summary>
        /// 交易識別碼(條碼)
        /// </summary>
        [Display(Name = "交易識別碼(條碼)")]
        [Required(ErrorMessage = "{0} 為必填")]
        [StringLength(18, ErrorMessage = "{0} 最大長度為18")]
        public string Barcode { get; set; }
    }
}