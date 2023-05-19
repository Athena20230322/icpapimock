using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICP.Library.Models.MemberModels
{
    using Infrastructure.Core.Models;

    public class SMSAuthVerifyResult : BaseResult
    {
        /// <summary>
        /// 驗證錯誤次數
        /// </summary>
        public int AuthErrorCount { get; set; }
    }
}
