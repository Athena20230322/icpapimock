using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICP.Batch.BankTranfserSend.Models
{
    /// <summary>
    /// 待轉帳資訊發動轉帳指示
    /// </summary>
    public class BankTransferSendModel
    {
        /// <summary>
        /// 提領記錄編號
        /// </summary>
        public long TransferID { get; set; }

        /// <summary>
        /// 會員編號
        /// </summary>
        public long MID { get; set; }

        /// <summary>
        /// 交易編號
        /// </summary>
        public string TradeNo { get; set; }

        /// <summary>
        /// 銀行代碼
        /// </summary>
        public string BankCode { get; set; }

        /// <summary>
        /// 銀行名稱
        /// </summary>
        public string BankName { get; set; }

        /// <summary>
        /// 銀行帳號
        /// </summary>
        public string BankAccount { get; set; }

        /// <summary>
        /// 銀行帳戶戶名
        /// </summary>
        public string AccountName { get; set; }

        /// <summary>
        /// 統一編號或身分證字號
        /// </summary>
        public string TransAccount { get; set; }

        /// <summary>
        /// 轉帳總金額
        /// </summary>
        public int Amount { get; set; }

        /// <summary>
        /// 轉帳類別
        /// 1:單次 2:排程  3: 撥款排程
        /// </summary>
        public byte TransferAuth { get; set; }
    }
}
