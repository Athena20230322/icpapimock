using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICP.Modules.Mvc.Admin.Models.MerchantModels
{
    /// <summary>
    /// 廠商類型
    /// </summary>
    public class MerchantCategory
    {
        /// <summary>
        /// 類別代碼
        /// </summary>
        public string MCCCode { get; set; }

        /// <summary>
        /// 類別英文名稱
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 類別中文名稱
        /// </summary>
        public string CName { get; set; }
    }
}
