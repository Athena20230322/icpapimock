using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICP.Modules.Api.Member.Models.ACLink
{
    public class ACLinkModel
    {
        /// <summary>
        /// 銀行回傳成功綁定帳號識別碼
        /// </summary>
        public string INDTAccount { get; set; }

        /// <summary>
        /// 銀行代碼
        /// </summary>
        public string BankCode { get; set; }

        /// <summary>
        /// 經遮蔽處理的綁定實體帳號
        /// </summary>
        public string LINKAccount { get; set; }

        /// <summary>
        /// 會員綁定狀態
        /// </summary>
        public int Status { get; set; }

        /// <summary>
        /// 是否為預設(1 = Y , 0 = N)
        /// </summary>
        public int IsDefault { get; set; }

        /// <summary>
        /// 建立時間
        /// </summary>
        public DateTime CreateDate { get; set; }

        /// <summary>
        /// 銀行訊息編號
        /// </summary>
        public string MsgNo { get; set; }

        /// <summary>
        /// 銀行中文名稱 (全名)
        /// </summary>
        public string BankName { get; set; }

        /// <summary>
        /// 銀行中文簡稱 (四碼)
        /// </summary>
        public string BankShortName { get; set; }

        /// <summary>
        /// 收單行代碼
        /// </summary>
        public int PaymentSubTypeID { get; set; }

        /// <summary>
        /// 銀行帳號
        /// </summary>
        public string BankAccount { get; set; }
    }
}
