using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICP.Modules.Api.AccountLink.Enums
{
    /// <summary>
    /// API類型
    /// </summary>
    public enum ApiType : int
    {
        /// <summary>
        /// 綁定
        /// </summary>
        ACLinkBind = 1,
        /// <summary>
        /// 解綁
        /// </summary>
        ACLinkCancel = 2,
        /// <summary>
        /// 交易扣款
        /// </summary>
        ACLinkPay = 3,
        /// <summary>
        /// 儲值
        /// </summary>
        ACLinkDeposit = 4,
        /// <summary>
        /// 交易退款
        /// </summary>
        ACLinkRefund = 5,
        /// <summary>
        /// 提領
        /// </summary>
        ACLinkWithdrawal = 6,
        /// <summary>
        /// 綁定狀態查詢
        /// </summary>
        ACLinkBindQuery = 7,
        /// <summary>
        /// 交易查詢
        /// </summary>
        ACLinkPayQuery = 8,
        /// <summary>
        /// 綁定申請
        /// </summary>
        ACLinkBindApply = 9,
        /// <summary>
        /// 綁定結果通知(後端)
        /// </summary>
        BindApiResult = 10,
        /// <summary>
        /// 綁定結束導回(前端)
        /// </summary>
        BindWebResult = 11,
        /// <summary>
        /// 銀行端解綁
        /// </summary>
        BankACLinkCancel = 12
    }
}
