using ICP.Infrastructure.Core.Models;
using System.ComponentModel.DataAnnotations;

namespace ICP.Modules.Mvc.Payment.Models.QueryTrade
{
    public class TradeReq : ValidatableObject
    {
        /// <summary>
        /// 會員編號
        /// </summary>
        public long MID { get; set; }

        /// <summary>
        /// 訂單流水號
        /// </summary>
        [Required(ErrorMessage = "{0} 欄位不能為空")]
        [Range(0, int.MaxValue, ErrorMessage = "{0} 欄位格式不正確")]
        public long TradeID { get; set; }
    }
}