using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.ComponentModel.DataAnnotations;

namespace ICP.Modules.Mvc.Admin.Models
{
    public class FunctionCatagory
    {
        /// <summary>
        /// 功能編號
        /// </summary>
        [Display(Name = "功能編號")]
        public int FunctionID { get; set; }

        /// <summary>
        /// 功能群組編號
        /// </summary>
        [Required(ErrorMessage = "必須輸入")]
        [Display(Name = "功能群組編號")]
        public int FunctionGroupID { get; set; }

        /// <summary>
        /// 功能層級
        /// </summary>
        [Display(Name = "功能層級")]
        public byte FunctionLevel { get; set; }

        /// <summary>
        /// 功能名稱
        /// </summary>
        [Required(ErrorMessage = "必須輸入")]
        [StringLength(100, ErrorMessage = "長度不可超過100")]
        [Display(Name = "功能名稱")]
        public String FunctionName { get; set; }

        /// <summary>
        /// Controller
        /// </summary>
        [StringLength(50, ErrorMessage = "長度不可超過50")]
        [Display(Name = "Controller")]
        public String Controller { get; set; }

        /// <summary>
        /// Method
        /// </summary>
        [StringLength(50, ErrorMessage = "長度不可超過50")]
        [Display(Name = "Method")]
        public String Method { get; set; }

        /// <summary>
        /// 排序編號
        /// </summary>
        [Required(ErrorMessage = "必須輸入")]
        [Display(Name = "排序編號")]
        public short OrderID { get; set; }

        /// <summary>
        /// Parent編號
        /// </summary>
        [Display(Name = "Parent")]
        public int ParentID { get; set; }

        /// <summary>
        /// 功能的作用編號總和
        /// </summary>
        [Required(ErrorMessage = "必須輸入")]
        [Display(Name = "功能按鈕合計")]
        public int ActionSum { get; set; }

        /// <summary>
        /// 狀態
        /// 0 = 停用
        /// 1 = 啟用
        /// </summary>
        [Display(Name = "狀態")]
        public byte Status { get; set; }

        /// <summary>
        /// 最後修改人
        /// </summary>
        [Display(Name = "最後修改人")]
        public string Modifier { get; set; }

        /// <summary>
        /// 最後修改日期
        /// </summary>
        [Display(Name = "最後修改日期")]
        public DateTime? ModifyDate { get; set; }
    }
}
