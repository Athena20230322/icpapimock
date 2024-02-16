using ICP.Infrastructure.Core.Models.Consts;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICP.Modules.Mvc.Admin.Models.CustomerManager
{
    public class AddFreezeCoinsModel
    {
        /// <summary>
        /// 會員編號
        /// </summary>
        public long MID { get; set; }

        /// <summary>
        /// 帳戶餘額
        /// </summary>
        public decimal TotalCash { get; set; }

        /// <summary>
        /// 凍結款
        /// </summary>
        [Required(ErrorMessage = "欲凍結金額不得為空")]
        [Range(0.0001, double.MaxValue, ErrorMessage = "欲凍結金額不能為0")]
        [Display(Name = "欲凍結金額")]
        public decimal FreezeCash { get; set; }

        /// <summary>
        /// 狀態
        /// </summary>
        public int Status { get; set; }

        /// <summary>
        /// 備註
        /// </summary>
        [Required(ErrorMessage = "{0}不得為空")]
        [MaxLength(200, ErrorMessage = "{0}格式錯誤")]
        [Display(Name = "凍結原因")]
        public string Remark { get; set; }

        /// <summary>
        /// 建立者
        /// </summary>
        public string Creator { get; set; }
    }
}
