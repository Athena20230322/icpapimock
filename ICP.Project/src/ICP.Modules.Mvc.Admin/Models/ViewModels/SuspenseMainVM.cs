using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICP.Modules.Mvc.Admin.Models.ViewModels
{
    public class SuspenseMainVM : SuspenseMain
    {
        /// <summary>
        /// 懲處方式
        /// </summary>
        public string SuspenseDesc { get; set; }

        /// <summary>
        /// 停權原因
        /// </summary>
        public string ReasonDesc { get; set; }
    }
}
