using ICP.Infrastructure.Core.Models;
using System.ComponentModel.DataAnnotations;

namespace ICP.Modules.Api.Payment.Models.TopUp
{
    public class AutoTopUpSwitchReq : ValidatableObject
    {
        /// <summary>
        /// 會員編號
        /// </summary>
        [Required(ErrorMessage = "{0} 為必填")]
        [Range(1, long.MaxValue, ErrorMessage = "{0} 格式不正確")]
        public long MID { get; set; }

        /// <summary>
        /// 自動儲值開關 (0：關閉, 1：開啟)
        /// </summary>
        [Required(ErrorMessage = "{0} 為必填")]
        [Range(0, 1, ErrorMessage = "{0} 格式不正確")]
        public int TopUpSwitch { get; set; }
    }
}
