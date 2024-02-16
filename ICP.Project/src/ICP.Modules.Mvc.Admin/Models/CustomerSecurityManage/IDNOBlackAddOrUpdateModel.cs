using ICP.Infrastructure.Core.Models.Consts;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICP.Modules.Mvc.Admin.Models.CustomerSecurityManage
{
    public class IDNOBlackAddOrUpdateModel
    {
        /// <summary>
        /// 身份證字號/居留證
        /// </summary>
        [Required(ErrorMessage = "請輸入正確的身分證字號/居留證")]
        [RegularExpression(RegexConst.IDNOOrUniformID, ErrorMessage = "請輸入正確的身分證字號/居留證")]
        public string IDNO { get; set; }

        /// <summary>
        /// 修改者
        /// </summary>
        public string ModifyUser { get; set; }
        
        /// <summary>
        /// 狀態 0:解除 1:鎖定
        /// </summary>
        public int Status { get; set; }

        /// <summary>
        /// 是否為新增 0:否 1:是
        /// </summary>
        public int IsAdd { get; set; }

        /// <summary>
        /// 解鎖/封鎖原因
        /// </summary>
        [Required(ErrorMessage = "請輸入原因")]
        public string ModifyMemo { get; set; }

        public long RealIP { get; set; }


        public long ProxyIP { get; set; }
       
    }
}
