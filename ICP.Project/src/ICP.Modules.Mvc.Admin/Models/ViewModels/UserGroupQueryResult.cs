using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICP.Modules.Mvc.Admin.Models.ViewModels
{
    using Infrastructure.Core.Models;

    public class UserGroupQueryResult: BaseListModel
    {
        /// <summary>
        /// 使用者群組ID
        /// </summary>
        public int UserGroupID { get; set; }

        /// <summary>
        /// 使用者群組編號
        /// </summary>
        public string UserGroupCode { get; set; }

        /// <summary>
        /// 使用者群組名稱
        /// </summary>
        public string UserGroupName { get; set; }

        /// <summary>
        /// 備註
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// 顯示
        /// </summary>
        public byte Visible { get; set; }
    }
}
