using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICP.Modules.Mvc.Admin.Models.CustomerManager
{
    public class UnLockSMSVM
    {
        /// <summary>
        /// 手機號碼
        /// </summary>
        public string CellPhone { get; set; }

        /// <summary>
        /// 當日OTP次數
        /// </summary>
        public int OTPCounts { get; set; }

        /// <summary>
        /// 手機解鎖記錄
        /// </summary>
        public List<UnLockSMSLogModel> UnLockSMSLogs { get; set; }
    }
}
