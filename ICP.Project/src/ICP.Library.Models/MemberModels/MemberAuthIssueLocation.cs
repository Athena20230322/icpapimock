using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICP.Library.Models.MemberModels
{
    /// <summary>
    /// 發證地點
    /// </summary>
    public class MemberAuthIssueLocation
    {
        /// <summary>
        /// 發證地點編號
        /// </summary>
        public string IssueLocationID { get; set; }

        /// <summary>
        /// 發證地點名稱
        /// </summary>
        public string IssueLocationName { get; set; }
    }
}
