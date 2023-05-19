using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICP.Modules.Api.PaymentCenter.Models
{
    /// <summary>
    /// 第一銀行 AP to AP 交易的 request model
    /// </summary>
    public class CardPayRegReq : CardPayRegBase
    {
        /// <summary>
        /// 功能代碼 → 應用於多組銷帳編號,開辦時,一銀給定
        /// </summary>
        public string FunCode { get; set; }

        /// <summary>
        /// 回傳URL → AP TO AP 不需指定回傳URL,系統將直接回傳訊息
        /// </summary>
        public string RsURL { get; set; }

        /// <summary>
        /// 押碼 → 以實際欄位內容運算：Base64(TDES(KEY,SHA-1(傳送序號+特店代碼+繳款期限+銷帳編號+繳款金額)))
        /// </summary>
        public string MAC => ComputeMacValue();
    }
}
