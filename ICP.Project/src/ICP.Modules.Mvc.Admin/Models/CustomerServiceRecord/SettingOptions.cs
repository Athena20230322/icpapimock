using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICP.Modules.Mvc.Admin.Models.CustomerServiceRecord
{
    public class SettingOptions
    {
        /// <summary>
        /// 設定編號
        /// </summary>
        public long ID { get; set; }
        /// <summary>
        /// 設定顯示名稱
        /// </summary>
        public string Description { get; set; }
    }
}
