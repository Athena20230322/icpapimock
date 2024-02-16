using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICP.Modules.Mvc.Admin.Models.CustomerSecurityManage
{
    public class IPBlackUpdateModel
    {
        public long Sn { get; set; }

        public string Modifier { get; set; }
        
        public int Status { get; set; }

        [Required(ErrorMessage = "請輸入原因")]
        public string Memo { get; set; }

        public long IP { get; set; }

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
