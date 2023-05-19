using ICP.Infrastructure.Core.Models.Consts;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICP.Modules.Mvc.Admin.Models.CustomerSecurityManage
{
    public class OTPBlackLockModel
    {
        /// <summary>
        /// 手機號碼
        /// </summary>
        [Required(ErrorMessage = "請輸入正確的手機號碼")]
        [RegularExpression(RegexConst.CellPhone, ErrorMessage = "請輸入正確的手機號碼")]
        [Display(Name = "手機號碼")]
        public string CellPhone { get; set; }

        /// <summary>
        /// 操作者
        /// </summary>
        public string LockUser { get; set; }

        /// <summary>
        /// RealIP
        /// </summary>
        public long RealIP { get; set; }

        /// <summary>
        /// ProxyIP
        /// </summary>
        public long ProxyIP { get; set; }

        /// <summary>
        /// 鎖定原因
        /// </summary>
        [Required(ErrorMessage = "請輸入原因")]
        public string LockMemo { get; set; }
        
        /// <summary>
        /// 鎖定類別(0:自動, 1:手動)
        /// </summary>
        public int LockType { get; set; }
    }
}
