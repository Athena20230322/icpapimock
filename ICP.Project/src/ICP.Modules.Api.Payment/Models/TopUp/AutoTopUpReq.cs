using ICP.Infrastructure.Core.Models;
using System.ComponentModel.DataAnnotations;

namespace ICP.Modules.Api.Payment.Models.TopUp
{
    public class AutoTopUpReq : ValidatableObject
    {
        /// <summary>
        /// 會員編號
        /// </summary>
        [Required(ErrorMessage = "{0} 為必填")]
        [Range(1, long.MaxValue, ErrorMessage = "{0} 格式不正確")]
        public long MID { get; set; }

        /// <summary>
        /// 交易金額
        /// </summary>
        [Required(ErrorMessage = "{0} 為必填")]
        [Range(1, int.MaxValue, ErrorMessage = "{0} 格式不正確")]
        public int Amount { get; set; }
    }
}
