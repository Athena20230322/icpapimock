using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICP.Library.Models.TopUp
{
    public class TopUpLimitModel
    {
        /// <summary>
        /// 會員儲值上限
        /// </summary>
        public int TopUpLimit { get; set; }

        /// <summary>
        /// 目前儲值餘額
        /// </summary>
        public int TopUpAmt { get; set; }

        /// <summary>
        /// 佔用中的儲值額度
        /// </summary>
        public int TopUpUsedLimit { get; set; }

        /// <summary>
        /// 可儲值金額
        /// </summary>
        public int AvailableTopUp { get; set; }

        /// <summary>
        /// 帳戶餘額
        /// </summary>
        public int TotalCoins { get; set; }
    }
}
