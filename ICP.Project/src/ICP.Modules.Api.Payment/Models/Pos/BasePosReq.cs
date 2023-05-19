using ICP.Library.Models.AuthorizationApi;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace ICP.Modules.Api.Payment.Models.Pos
{
    public class BasePosReq : BaseAuthorizationApiRequest
    {
        /// <summary>
        /// 平台編號
        /// </summary>
        public long PlatformID { get; set; }

        /// <summary>
        /// 廠商編號
        /// </summary>
        [Display(Name = "廠商編號")]
        [Range(1, long.MaxValue, ErrorMessage = "{0} 為必填")]
        public long MerchantID { get; set; }
    }
}