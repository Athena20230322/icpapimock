using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICP.Modules.Mvc.Admin.Models.MerchantModels
{
    /// <summary>
    /// 審核狀態
    /// </summary>
    public class AuditStatusModel
    {
        /// <summary>
        /// 審核狀態編號
        /// </summary>
        public byte AuditStatusID { get; set; }

        /// <summary>
        /// 審核狀態描述
        /// </summary>
        public string AuditStatusName { get; set; }
    }
}
