using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICP.Modules.Mvc.Admin.Models.CustomerManager
{
    public class AuthCellPhoneListLogVM
    {
        /// <summary>
        /// 原手機號碼
        /// </summary>
        public string OCellPhone { get; set; }

        /// <summary>
        /// 修改後手機號碼
        /// </summary>
        public string CellPhone { get; set; }

        /// <summary>
        /// 修改原因
        /// </summary>
        public string Note { get; set; }

        /// <summary>
        /// 修改時間
        /// </summary>
        public DateTime CreateDate { get; set; }

        /// <summary>
        /// 操作人
        /// </summary>
        public string CreateUser { get; set; }

    }
}
