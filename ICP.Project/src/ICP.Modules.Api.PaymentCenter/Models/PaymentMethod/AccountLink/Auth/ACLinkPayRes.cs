using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICP.Modules.Api.PaymentCenter.Models.PaymentMethod.AccountLink.Auth
{
    /// <summary>
    /// AccountLink交易扣款(ACLinkPay) 回傳值
    /// </summary>
    public class ACLinkPayRes
    {
        /// <summary>
        /// 銀行單號
        /// </summary>
        public string BankTradeNo { get; set; }

        /// <summary>
        /// 回應碼
        /// </summary>
        public int RtnCode { get; set; }

        /// <summary>
        /// 回應訊息
        /// </summary>
        public string RtnMsg { get; set; }

        /*
        /// <summary>
        /// 訊息序號
        /// </summary>
        public string MSG_NO { get; set; }

        /// <summary>
        /// 回應代碼
        /// </summary>
        public string RTN_CODE { get; set; }

        /// <summary>
        /// 回應訊息說明
        /// </summary>
        public string RTN_MSG { get; set; }

        /// <summary>
        /// 平台代碼
        /// </summary>
        public string EC_ID { get; set; }

        /// <summary>
        /// 平台會員代號
        /// </summary>
        public string EC_USER { get; set; }

        /// <summary>
        /// 訊息雜湊值
        /// </summary>
        public string RTN_DIGEST { get; set; }
        */
    }
}
