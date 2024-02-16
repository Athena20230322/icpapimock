using System.ComponentModel.DataAnnotations;

namespace ICP.Modules.Api.AccountLink.Models.Cathay
{
    /// <summary>
    /// 國泰世華-綁定結果通知(前台)(銀行->業者)
    /// </summary>
    public class BankBindReplyWebReq
    {
        /// <summary>
        /// 交易序號
        /// </summary>
        public string txnseq { get; set; }

        /// <summary>
        /// 會員編號
        /// </summary>
        public string mbrActNo { get; set; }

        /// <summary>
        /// 回覆值/交易結果
        /// </summary>
        /// <remarks>成功:0000</remarks>
        public string returncode { get; set; }

        /// <summary>
        /// 回覆說明/交易結果說明
        /// </summary>
        public string returnmsg { get; set; }

    }
}
