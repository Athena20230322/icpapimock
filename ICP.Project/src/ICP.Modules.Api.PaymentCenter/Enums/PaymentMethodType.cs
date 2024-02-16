using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICP.Modules.Api.PaymentCenter.Enums
{
    public enum ePaymentType
    {
        Null = 0,               // 無
        ICash = 1,              // 電支帳戶
        AccountLink = 2,        // 連結帳戶扣款
        ATM = 3,                // 銀行櫃員機
        Cash = 4,               // 現金(儲值)
        Invoice = 5,            // 發票(中獎發票)(儲值)
        Transfer_Icash = 6,     // 轉帳_電支帳戶
        Withdrawal_Icash = 7,   // 提領_電支帳戶
        Adjust_Icash = 8,       // 調帳_電支帳戶
        Allocate_Icash = 9,     // 撥款_電支帳戶
        FreezeCash_Icash = 10,  // 凍結款項_電支帳戶

        Min = ICash,
        Max = Invoice,
        TranscationMin = ICash,
        TranscationMax = AccountLink,
        TopupMin = AccountLink,
        TopupMax = Invoice
    }
}
