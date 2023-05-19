using ICP.Infrastructure.Core.Models;

namespace ICP.Modules.Api.AccountLink.Models.ChinaTrust
{
    public class ACLinkRes : BaseResult
    {
        /// <summary>
        /// 簽章
        /// </summary>
        public string ResMsgSign { get; set; }

        /// <summary>
        /// 加密後AESKey
        /// </summary>
        public string ResAesKey { get; set; }

        /// <summary>
        /// 加密後原文
        /// </summary>
        public string ResSecData { get; set; }
    }
}
