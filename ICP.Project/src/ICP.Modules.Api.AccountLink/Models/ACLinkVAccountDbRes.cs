using ICP.Infrastructure.Core.Models;

namespace ICP.Modules.Api.AccountLink.Models
{
    public class ACLinkVAccountDbRes : BaseResult
    {
        /// <summary>
        /// 虛擬帳號
        /// </summary>
        public string VirtualAccount { get; set; }
    }
}
