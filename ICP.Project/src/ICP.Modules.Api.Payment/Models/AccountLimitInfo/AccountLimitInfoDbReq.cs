using ICP.Infrastructure.Core.Models;
using ICP.Library.Models.AuthorizationApi;
using System.ComponentModel.DataAnnotations;

namespace ICP.Modules.Api.Payment.Models.AccountLimitInfo
{
    /// <summary>
    /// 帳戶資訊>交易限額 DB請求
    /// </summary>
    public class AccountLimitInfoDbReq : BaseResult
    {
        /// <summary>
        /// 會員編號
        /// </summary>
        public long MID { get; set; }
    }
}
