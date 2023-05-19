using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICP.Modules.Api.Payment.Models.TopUp
{
    public class AutoTopUpModel
    {
        /// <summary>
        /// 會員編號
        /// </summary>
        public long MID { get; set; }

        /// <summary>
        /// 銀行帳戶編號(系統定義流水號)
        /// </summary>
        public long AccountID { get; set; }

        /// <summary>
        /// 帳號識別碼
        /// </summary>
        public string INDTAccount { get; set; }

        /// <summary>
        /// 自動儲值開關 (0：關閉, 1：開啟)
        /// </summary>
        public int TopUpSwitch { get; set; }

        /// <summary>
        /// 自動儲值模式 (1：差額儲值, 2：定額儲值)
        /// </summary>
        public int TopUpMode { get; set; }

        /// <summary>
        /// 定額儲值金額基數
        /// </summary>
        public int TopUpUnit { get; set; }

        /// <summary>
        /// 單次儲值上限
        /// </summary>
        public int LimitAmount { get; set; }

        /// <summary>
        /// 單日儲值上限
        /// </summary>
        public int DailyLimitAmount { get; set; }

        /// <summary>
        /// AccountLink綁定狀態
        /// </summary>
        public int Status { get; set; }

        /// <summary>
        /// 銀行代碼
        /// </summary>
        public string BankCode { get; set; }

        /// <summary>
        /// 銀行帳號
        /// </summary>
        public string BankAccount { get; set; }

        /// <summary>
        /// 銀行名稱 (全名)
        /// </summary>
        public string BankName { get; set; }

        /// <summary>
        /// 銀行名稱 (簡寫四碼)
        /// </summary>
        public string BankShortName { get; set; }

    }
}
