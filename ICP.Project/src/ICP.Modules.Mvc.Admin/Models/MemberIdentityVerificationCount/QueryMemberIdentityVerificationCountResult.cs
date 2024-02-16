using ICP.Infrastructure.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICP.Modules.Mvc.Admin.Models.MemberIdentityVerificationCount
{
    public class QueryMemberIdentityVerificationCountResult
    {
        /// <summary>
        /// 項目
        /// 0:p11
        /// 1:p33
        /// </summary>
        public int AuthType { get; set; }
        /// <summary>
        /// 時間(年/月)
        /// </summary>
        public DateTime CreateDate { get; set; }
        /// <summary>
        /// 筆數
        /// </summary>
        public decimal AuthCount { get; set; }
        /// <summary>
        /// 小計金額(該時間內的小計)
        /// </summary>
        public decimal Price { get; set; }
    }
}
