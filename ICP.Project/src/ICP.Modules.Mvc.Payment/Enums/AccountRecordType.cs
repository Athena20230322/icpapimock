using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICP.Modules.Mvc.Payment.Enums
{
    /// <summary>
    /// 帳戶紀錄類別
    /// </summary>
    public enum AccountRecordType
    {
        /// <summary>
        /// 所有
        /// </summary>
        All = 0,           
        /// <summary>
        /// 交易
        /// </summary>
        Transaction = 1,    
        /// <summary>
        /// 儲值
        /// </summary>
        Topup = 2,          
        /// <summary>
        /// 轉出
        /// </summary>
        TransferOut = 31,
        /// <summary>
        /// 轉入
        /// </summary>
        TransferIn = 32,
        /// <summary>
        /// 提領
        /// </summary>
        Withdrawal = 4,     
        /// <summary>
        /// 退款
        /// </summary>
        Refund = 5         
    }
}
