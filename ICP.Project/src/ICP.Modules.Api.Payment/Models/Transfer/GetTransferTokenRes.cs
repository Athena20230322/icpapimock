using ICP.Infrastructure.Core.Models;

namespace ICP.Modules.Api.Payment.Models.Transfer
{
    public class GetTransferTokenRes : DataResult
    {
        /// <summary>
        /// 轉帳的交易Token
        /// </summary>
        public string Token { get; set; }
    }
}

