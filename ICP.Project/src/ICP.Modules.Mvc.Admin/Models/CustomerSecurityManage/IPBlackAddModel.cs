using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICP.Modules.Mvc.Admin.Models.CustomerSecurityManage
{
    public class IPBlackAddModel
    {
        public long Sn { get; set; }

        [Required(ErrorMessage = "請輸入完整的IP")]
        public string IP { get; set; }

        public string Modifier { get; set; }
        
        public int Status { get; set; }

        [Required(ErrorMessage = "請輸入原因")]
        public string LockMemo { get; set; }

        /// <summary>
        /// 建立人員
        /// </summary>
        public string CreateUser { get; set; }

        /// <summary>
        /// RealIP
        /// </summary>
        public long RealIP { get; set; }

        /// <summary>
        /// ProxyIP
        /// </summary>
        public long ProxyIP { get; set; }

    }
}
