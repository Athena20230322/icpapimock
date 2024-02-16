using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICP.Modules.Api.PaymentCenter.Enums
{
    public enum eTradeMode
    {
        Null = 0,           // 無
        Transaction = 1,    // 交易
        Topup = 2,          // 儲值
        Transfer = 3,       // 轉帳
        Withdrawal = 4,     // 提領
        Allocate = 5,       // 撥款
        Adjust = 6,         // 調帳
        FreezeCash = 7,     // 凍結款項

        Reversal = 98,      // 取消
        Refund = 99,        // 退款

        Min = Transaction,
        Max = FreezeCash
    }
}
