using ICP.Infrastructure.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICP.Modules.Mvc.Admin.Models.MemberIdentityVerificationCount
{
    public class QueryMemberIdentityVerificationCount : ValidatableObject
    {
        /// <summary>
        /// 起始日期
        /// </summary>
        public DateTime StartDate { get; set; }
        /// <summary>
        /// 結束時間
        /// </summary>
        public DateTime EndDate { get; set; }
        /// <summary>
        /// 項目
        /// null:全部
        /// 0:p11
        /// 1:p33
        /// </summary>
        public int? AuthType { get; set; }
    }
}
