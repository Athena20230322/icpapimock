using ICP.Library.Models.AuthorizationApi;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICP.Modules.Api.Payment.Models
{
    public class TopUpATMReq : BaseAuthorizationApiRequest
    {
        /// <summary>
        /// 銀行代碼
        /// </summary>
        [Required(ErrorMessage = "{0}為必填欄位")]
        public string BankCode { get; set; }

        /// <summary>
        /// 儲值金額
        /// </summary>
        //[Required(ErrorMessage = "{0}為必填欄位")]
        [Range(100, 10000, ErrorMessage = "{0}可儲值範圍為{1}~{2}")]
        public decimal Amount { get; set; }

        /// <summary>
        /// 交易模式 → 1:交易 2:儲值 3:轉帳 4:提領
        /// </summary>
        public int TradeModeID { get; } = 2;

        /// <summary>
        /// 付款方式代碼
        /// </summary>
        public int PaymentTypeID { get; } = 3;      // ATM: 3

    }
}
