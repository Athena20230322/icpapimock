using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICP.Modules.Mvc.Admin.Models.CustomerManager
{
    public class UserCoinsModel
    {
        /// <summary>
        /// 現金帳戶金額
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
