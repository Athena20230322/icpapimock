using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICP.Library.Models.MemberModels
{
    public class MemberBankAccount
    {
        /// <summary>
        /// 會員銀行帳號流水號
        /// </summary>
        public long BankID { get; set; }

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
        /// 銀行分行代碼
        /// </summary>
        public string BankBranchCode { get; set; }

        /// <summary>
        /// 銀行帳號
        /// </summary>
        public string BankAccount { get; set; }

        /// <summary>
        /// 銀行回傳成功綁定帳號識別碼
        /// </summary>
        public string INDTAccount { get; set; }

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
        /// 驗證類別
        /// 0 = 一元驗證
        /// 2 = 提領成功
        /// 3 = ACL綁定
        /// </summary>
        public byte AuthCategory { get; set; }

        /// <summary>
        /// 驗證類型 & 運算
        /// 1 = 帳號驗證
        /// 2 = 文件審核
        /// </summary>
        public byte AuthType { get; set; }

        /// <summary>
        /// 文件審核狀態
        /// 0 = 預設值(待驗證)
        /// 1 = 驗證成功
        /// 2 = 驗證不通過
        /// </summary>
        public byte PaperAuthStatus { get; set; }

        public string FilePath1 { get; set; }

        public string FilePath2 { get; set; }

        /// <summary>
        /// 電支後台批次匯入時,操作者帳號
        /// </summary>
        public string CreateUser { get; set; }

        /// <summary>
        /// 來源
        /// 0 = 使用者輸入
        /// 1 = 批次匯入
        /// </summary>
        public byte Source { get; set; }
    }
}
