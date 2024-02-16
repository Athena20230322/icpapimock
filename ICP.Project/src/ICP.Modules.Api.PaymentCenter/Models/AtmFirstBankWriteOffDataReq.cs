using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICP.Modules.Api.PaymentCenter.Models
{
    public class AtmFirstBankWriteOffDataReq
    {
        /// <summary>
        /// 交易資料內容
        /// </summary>
        //[StringLength(91, MinimumLength = 91)]
        public string Content { get; set; }

        /// <summary>
        /// 交易資料類型 → P:真實交易資料
        /// </summary>
        //[Required]
        public string TxnType { get; set; }
    }
}
