using ICP.Library.Models.AuthorizationApi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICP.Modules.Api.Payment.Models
{
    public class TopUpATMRes : BaseAuthorizationApiResult
    {
        /// <summary>
        /// 儲值訂單編號
        /// </summary>
        public string IcpTradeNo { get; set; }

        /// <summary>
        /// 銀行代碼
        /// </summary>
        public string BankCode { get; set; }

        /// <summary>
        /// 銀行名稱(全名)
        /// </summary>
        public string BankName { get; set; }

        /// <summary>
        /// 銀行名稱(簡寫四碼)
        /// </summary>
        public string BankShortName { get; set; }

        /// <summary>
        /// ATM虛擬帳號
        /// </summary>
        public string ATMAccount { get; set; }

        /// <summary>
        /// 儲值金額
        /// </summary>
        public int Amount { get; set; }

        /// <summary>
        /// 建立日期 格式：2019/01/01 12:30
        /// </summary>
        public string CreateDate { get; set; }

        /// <summary>
        /// 轉帳截止日期(虛擬帳號有效期限) 格式：2019/01/01 23:59:59
        /// </summary>
        public string LimitDate { get; set; }
    }
}
