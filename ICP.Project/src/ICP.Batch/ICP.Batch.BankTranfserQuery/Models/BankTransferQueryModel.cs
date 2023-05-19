using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICP.Batch.BankTranfserQuery.Models
{
    public class BankTransferQueryModel
    {
        /// <summary>
        /// 轉帳資料編號
        /// </summary>
        public long TransferID { get; set; }

        /// <summary>
        /// FXML 訊息唯一key
        /// </summary>
        public string SvcRqId { get; set; }

        /// <summary>
        /// 會員編號
        /// </summary>
        public long MID { get; set; }

        /// <summary>
        /// 銀行代碼
        /// </summary>
        public string BankCode { get; set; }

        /// <summary>
        /// 銀行帳號
        /// </summary>
        public string BankAccount { get; set; }

        /// <summary>
        /// 同意升級為二類會員
        /// </summary>
        public bool AgreeLevelUp { get; set; }
    }
}
