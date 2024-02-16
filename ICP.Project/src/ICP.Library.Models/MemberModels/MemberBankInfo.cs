using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICP.Library.Models.MemberModels
{
    public class MemberBankInfo
    {
        /// <summary>
        /// 流水號
        /// </summary>
        public long AccountID { get; set; }

        /// <summary>
        /// 銀行代碼
        /// </summary>
        public string BankCode { get; set; }

        /// <summary>
        /// 提領類別
        /// 0 = 提領轉入帳戶
        /// 1 = 連結扣款帳戶
        /// </summary>
        public byte Category { get; set; }

        /// <summary>
        /// 銀行分行代碼
        /// </summary>
        public string BankBranchCode { get; set; }

        /// <summary>
        /// 銀行帳號
        /// </summary>
        public string BankAccount { get; set; }

        /// <summary>
        /// 帳號識別碼
        /// </summary>
        public string INDTAccount { get; set; }

        /// <summary>
        /// 建立時間
        /// </summary>
        public DateTime CreateDate { get; set; }

        /// <summary>
        /// 銀行帳號狀態
        /// 0 = 待驗證
        /// 1 = 正常
        /// 2 = 驗證失敗
        /// </summary>
        public byte AccountStatus { get; set; }

        /// <summary>
        /// 預設銀行帳號
        /// 0 = No
        /// 1 = Yes
        /// </summary>
        public byte isDefault { get; set; }

        /// <summary>
        /// 銀行名稱
        /// </summary>
        public string BankName { get; set; }

        /// <summary>
        /// 銀行名稱(簡寫四碼)
        /// </summary>
        public string BankShortName { get; set; }
    }
}
