using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICP.Library.Models.PaymentCenterApi.Enums
{
    // 電支帳戶
    public enum PaymentSubType_ICash
    {
        /// <summary>
        /// 電支帳戶
        /// </summary>
        ICash = 1,

        Min = ICash,
        Max = ICash
    }

    // AccountLink
    public enum PaymentSubType_AccountLink
    {
        /// <summary>
        /// 第一銀行
        /// </summary>
        First = 1,
        /// <summary>
        /// 中國信託
        /// </summary>
        ChinaTrust = 2,
        /// <summary>
        /// 國泰世華
        /// </summary>
        Cathay = 3,

        Min = First,
        Max = Cathay
    }

    // ATM
    public enum PaymentSubType_ATM
    {
        /// <summary>
        /// 第一銀行
        /// </summary>
        First = 1,

        Min = First,
        Max = First
    }
}
