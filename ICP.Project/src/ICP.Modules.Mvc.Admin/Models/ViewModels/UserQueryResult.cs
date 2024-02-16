using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICP.Modules.Mvc.Admin.Models.ViewModels
{
    using Infrastructure.Core.Models;

    public class UserQueryResult : BaseListModel
    {
        /// <summary>
        /// 使用者編號
        /// </summary>
        public int UserID { get; set; }

        /// <summary>
        /// 使用者帳號
        /// </summary>
        public string Account { get; set; }

        /// <summary>
        /// 使用者中文名稱
        /// </summary>
        public string CName { get; set; }

        /// <summary>
        /// 部門編號
        /// </summary>
        public int? DeptID { get; set; }

        /// <summary>
        /// 是否為主管
        /// </summary>
        public byte IsManager { get; set; }

        /// <summary>
        /// 使用者Email
        /// </summary>
        public string UserEmail { get; set; }

        /// <summary>
        /// 使用者狀態( 0: Disable, 1:Active)
        /// </summary>
        public byte UserStatus { get; set; }

        /// <summary>
        /// 員工編號
        /// </summary>
        public string EID { get; set; }

        /// <summary>
        /// 建檔時間
        /// </summary>
        public DateTime CreateDate { get; set; }
    }
}
