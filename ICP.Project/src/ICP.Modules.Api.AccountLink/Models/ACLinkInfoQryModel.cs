using ICP.Library.Models.AccountLinkApi.Enums;

namespace ICP.Modules.Api.AccountLink.Models
{
    /// <summary>
    /// 綁定資訊請求
    /// </summary>
    public class ACLinkInfoQryModel
    {
        /// <summary>
        /// 會員編號
        /// </summary>        
        public long MID { get; set; }

        /// <summary>
        /// 銀行回傳成功綁定帳號識別碼
        /// </summary>
        public string INDTAccount { get; set; }

        /// <summary>
        /// 銀行代碼
        /// </summary>
        public BankType BankType { get; set; }

    }
}
