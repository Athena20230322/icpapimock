using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICP.Modules.Mvc.Admin.Models
{
    /// <summary>
    /// 使用者特殊權限
    /// </summary>
    public class UserPermissionSpecial
    {
        /// <summary>
        /// 使用者編號
        /// </summary>
        public int UserID { get; set; }

        /// <summary>
        /// 個資權限
        /// </summary>
        public int PersonalInfoActionSum { get; set; }
    }
}
