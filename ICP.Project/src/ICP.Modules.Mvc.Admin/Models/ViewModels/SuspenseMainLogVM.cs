using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICP.Modules.Mvc.Admin.Models.ViewModels
{
    public class SuspenseMainLogVM
    {
        /// <summary>
        /// 建立時間
        /// </summary>
        public DateTime CreateDate { get; set; }

        /// <summary>
        /// 建立者
        /// </summary>
        public string CreateUser { get; set; }

        /// <summary>
        /// 內部備註
        /// </summary>
        public string Note { get; set; }

        /// <summary>
        /// 最後修改時間
        /// </summary>
        public DateTime ModifyDate { get; set; }

        /// <summary>
        /// 修改人
        /// </summary>
        public string Modifier { get; set; }

        /// <summary>
        /// 帳號狀態
        /// </summary>
        public string SuspenseDesc { get; set; }

        /// <summary>
        /// 原因
        /// </summary>
        public string ReasonDesc { get; set; }
    }
}
