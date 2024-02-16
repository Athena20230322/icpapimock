using ICP.Infrastructure.Core.Models.Consts;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICP.Modules.Mvc.Admin.Models.CustomerManager
{
    public class ReturnFreezeCoinsModel
    {
        public long FreezeID { get; set; }

        /// <summary>
        /// 會員編號
        /// </summary>
        public long MID { get; set; }

        /// <summary>
        /// 返還會員電支帳號
        /// </summary>
        [Required(ErrorMessage = "電支帳號格不得為空")]
        [RegularExpression(RegexConst.ICPMID, ErrorMessage = "{0} 格式錯誤")]
        [Display(Name = "電支帳號")]
        public long RtnICPMID { get; set; }

        /// <summary>
        /// 凍結金額
        /// </summary>        
        public decimal FreezeCash { get; set; }

        /// <summary>
        /// 狀態
        /// </summary>
        public int Status { get; set; }

        /// <summary>
        /// 原因備註
        /// </summary>
        [Required(ErrorMessage = "原因備註不得為空")]
        [MaxLength(200, ErrorMessage = "{0} 格式錯誤")]
        [Display(Name = "原因備註")]
        public string Remark { get; set; }

        /// <summary>
        /// 操作者
        /// </summary>
        public string Creator { get; set; }
    }
}
