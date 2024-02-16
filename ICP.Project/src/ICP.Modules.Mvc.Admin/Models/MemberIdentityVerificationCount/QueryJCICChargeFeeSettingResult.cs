using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICP.Modules.Mvc.Admin.Models.MemberIdentityVerificationCount
{
    public class QueryJCICChargeFeeSettingResult
    {
        /// <summary>
        /// 聯徵費用設定生效日
        /// </summary>
        public DateTime YYYYMMDD { get; set; }
        /// <summary>
        /// 聯徵費用
        /// </summary>
        public decimal ChargeFee { get; set; }
    }
}
