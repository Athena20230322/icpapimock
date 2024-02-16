using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ICP.Modules.Mvc.Admin.Models
{
    public class Department
    {
        /// <summary>
        /// 部門編號
        /// </summary>
        [Display(Name = "部門編號")]
        public int DeptID { get; set; }

        /// <summary>
        /// 部門者名稱
        /// </summary>
        [Required]
        [Display(Name = "部門者名稱")]
        [StringLength(50)]
        public string DeptName { get; set; }

        /// <summary>
        /// 顯示
        /// </summary>
        [Display(Name = "顯示")]
        public byte Visible { get; set; }
    }
}
