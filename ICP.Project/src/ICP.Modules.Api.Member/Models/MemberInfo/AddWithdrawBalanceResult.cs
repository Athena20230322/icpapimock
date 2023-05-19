using ICP.Library.Models.AuthorizationApi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICP.Modules.Api.Member.Models.MemberInfo
{
    public class AddWithdrawBalanceResult : BaseAuthorizationApiResult
    {
        /// <summary>
        /// 電支帳戶餘額
        /// </summary>
        public int TopUpCash { get; set; }

        /// <summary>
        /// 提領金額
        /// </summary>
        public int Amount { get; set; }

        /// <summary>
        /// 手續費
        /// </summary>
        public int HandlingCharge { get; set; }

        /// <summary>
        /// 帳戶扣除金額 (提領金額+手續費)
        /// </summary>
        public int TotalAmount { get; set; }

        /// <summary>
        /// 提領轉入帳戶-銀行代碼
        /// </summary>
        public string BankCode { get; set; }

        /// <summary>
        /// 提領轉入帳戶-銀行帳號
        /// </summary>
        public string BankAccount { get; set; }

        /// <summary>
        /// 提領轉入帳戶-帳號末五碼
        /// </summary>
        public string AccountLast5No { get; set; }

        /// <summary>
        /// 銀行名稱
        /// </summary>
        public string BankName { get; set; }

        /// <summary>
        /// 提領訂單編號
        /// </summary>
        public string TradeNo { get; set; }

        /// <summary>
        /// 提領日期
        /// </summary>
        public string CreateDate { get; set; }

        /// <summary>
        /// 提領狀態
        /// 1 = 提領作業完成 (成功或失敗依RtnCode判斷)
        /// 2 = 提領作業處理中
        /// </summary>
        public int Status { get; set; }
    }
}
