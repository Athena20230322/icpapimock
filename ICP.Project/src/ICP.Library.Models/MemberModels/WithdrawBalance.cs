using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICP.Library.Models.MemberModels
{
    public class WithdrawBalance
    {
        /// <summary>
        /// 會員編號
        /// </summary>
        public long MID { get; set; }

        /// <summary>
        /// 特店編號
        /// </summary>
        public long MerchantID { get; set; }

        /// <summary>
        /// 銀行代碼
        /// </summary>
        public string BankCode { get; set; }

        /// <summary>
        /// 銀行帳號
        /// </summary>
        public string BankAccount { get; set; }

        /// <summary>
        /// 結算類型
        /// 1 = 固定提領
        /// 2 = 保留餘額
        /// </summary>
        public byte AMTransferType { get; set; }

        /// <summary>
        /// 轉出金額
        /// </summary>
        public int Amount { get; set; }

        /// <summary>
        /// 轉帳類別
        /// 1 = 單次
        /// 2 = 排程
        /// </summary>
        public byte TransferType { get; set; }

        /// <summary>
        /// 同意升級二類會員
        /// false = 不同意
        /// true = 同意
        /// </summary>
        public bool AgreeLevelUp { get; set; }
    }
}
