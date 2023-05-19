using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICP.Modules.Mvc.Admin.Models
{
    public class SuspenseSetting
    {
        /// <summary>
        /// 編號
        /// </summary>
        public int ID { get; set; }

        /// <summary>
        /// 類別
        /// 1 = 懲處類別
        /// 2 = 懲處原因類別
        /// 3 = 懲處原因罐頭訊息
        /// </summary>
        public byte Category { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        public string Description { get; set; }
    }
}
