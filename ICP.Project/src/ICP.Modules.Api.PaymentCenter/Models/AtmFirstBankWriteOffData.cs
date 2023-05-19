using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICP.Modules.Api.PaymentCenter.Models
{
    public class AtmFirstBankWriteOffData
    {
        /// <summary>
        /// 帳號：本行存款帳號
        /// </summary>
        public string CompanyAccount { get; set; }

        /// <summary>
        /// 交易日期(yyyymmdd 民國年)
        /// </summary>
        public string TransDate { get; set; }

        /// <summary>
        /// 交易流水序號(交易編號)
        /// </summary>
        public int TransID { get; set; }

        /// <summary>
        /// 交易代號 (詳見說明文件的備註一)
        /// </summary>
        public string TransNo { get; set; }

        /// <summary>
        /// 提領金額 (11位 + 2位小數)
        /// </summary>
        public decimal LenderAmt { get; set; }

        /// <summary>
        /// 存入金額 (11位 + 2位小數)
        /// </summary>
        public decimal DebitAmt { get; set; }

        /// <summary>
        /// 餘額正負號 (＋ 或 －)
        /// </summary>
        public string PNType { get; set; }

        /// <summary>
        /// 餘額 (13位 + 2位小數)
        /// </summary>
        public decimal Balance { get; set; }

        /// <summary>
        /// 交易狀態 → 0:正常交易 1:沖正交易 9:被沖正交易
        /// </summary>
        public int TransType { get; set; }

        /// <summary>
        /// 銷帳編號 → 5碼(存戶編號 ) + 11碼
        /// </summary>
        public string VirtualAccount { get; set; }

        /// <summary>
        /// 通路別 (詳見說明文件的備註三)
        /// </summary>
        public string RouteType { get; set; }
        
        //public string TransTime { get; set; }
        //public string BankCode { get; set; }
        //public string BankAcc { get; set; }
    }
}
