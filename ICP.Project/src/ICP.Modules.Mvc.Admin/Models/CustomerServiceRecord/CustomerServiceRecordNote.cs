using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICP.Modules.Mvc.Admin.Models.CustomerServiceRecord
{
    public class CustomerServiceRecordNote
    {
        /// <summary>
        /// 時間
        /// </summary>
        public DateTime CreateDate { get; set; }
        /// <summary>
        /// 問題
        /// </summary>
        public string Note { get; set; }
    }
}
