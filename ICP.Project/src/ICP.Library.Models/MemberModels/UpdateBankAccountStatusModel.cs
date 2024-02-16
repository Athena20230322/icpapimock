using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICP.Library.Models.MemberModels
{
    public class UpdateBankAccountStatusModel
    {
        /// <summary>
        /// 會員編號
        /// </summary>
        public long MID { get; set; }

        /// <summary>
        /// 提領類別
        /// 0 = 提領轉入帳戶
        /// 1 = 連結扣款帳戶
        /// </summary>
        public byte Category { get; set; }

        /// <summary>
        /// 銀行代碼
        /// </summary>
        public string BankCode { get; set; }

        /// <summary>
        /// 銀行帳號
        /// </summary>
        public string BankAccount { get; set; }

        /// <summary>
        /// 銀行帳號狀態
        /// 0 = 待驗證
        /// 1 = 正常
        /// 2 = 驗證失敗
        /// </summary>
        public byte AccountStatus { get; set; }

        /// <summary>
        /// 帳號識別碼
        /// </summary>
        public string INDTAccount { get; set; }

        /// <summary>
        /// 同意升級為二類會員
        /// </summary>
        public bool AgreeLevelUp { get; set; } = true;
    }
}
