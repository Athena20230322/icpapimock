using ICP.Infrastructure.Core.Models;

namespace ICP.Modules.Mvc.Member.Models.ACLink
{
    public class ACLinkResultModel : BaseResult
    {
        /// <summary>
        /// 訊息序號
        /// </summary>
        public string MsgNo { get; set; }

        /// <summary>
        /// 銀行代碼
        /// </summary>
        public string BankCode { get; set; }

        /// <summary>
        /// 銀行RtnCode
        /// </summary>
        public string ServiceCode { get; set; }

        /// <summary>
        /// 銀行RtnMsg
        /// </summary>
        public string ServiceMessage { get; set; }

        /// <summary>
        /// 網頁識別碼
        /// </summary>
        public string AuthId { get; set; }
    }
}
