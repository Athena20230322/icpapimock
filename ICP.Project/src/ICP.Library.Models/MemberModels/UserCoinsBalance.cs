using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICP.Library.Models.MemberModels
{
    public class UserCoinsBalance
    {
        /// <summary>
        /// 現金帳戶餘額
        /// </summary>
        public decimal Balance { get; set; }

        /// <summary>
        /// 可提領金額
        /// </summary>
        public decimal TotalBalance { get; set; }

        /// <summary>
        /// 凍結金額
        /// </summary>
        public decimal FreezeCash { get; set; }

        /// <summary>
        /// 帳戶餘額
        /// </summary>
        public decimal TotalCash { get; set; }
    }
}
