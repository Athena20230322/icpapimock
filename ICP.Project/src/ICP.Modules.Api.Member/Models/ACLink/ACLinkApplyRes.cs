using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICP.Modules.Api.Member.Models.ACLink
{
    public class ACLinkApplyRes
    {
        /// <summary>
        /// 網頁識別碼
        /// </summary>
        public string AuthId { get; set; }

        /// <summary>
        /// CTBC Internal ErrorCode
        /// </summary>
        public string ServiceCode { get; set; }

        /// <summary>
        /// CTBC Internal ErrorMessage
        /// </summary>
        public string ServiceMessage { get; set; }
    }
}
