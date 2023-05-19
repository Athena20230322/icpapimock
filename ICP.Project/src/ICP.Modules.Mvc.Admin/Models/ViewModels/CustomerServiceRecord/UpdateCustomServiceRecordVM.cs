using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICP.Modules.Mvc.Admin.Models.ViewModels.CustomerServiceRecord
{
    public class UpdateCustomServiceRecordVM
    {
        /// <summary>
        /// 案件進度 : 0建立案件 1 客服處理 2客服更改處理結果
        /// </summary>
        [Required(ErrorMessage = "請選擇案件進度")]
        public byte? Status { get; set; }
        /// <summary>
        /// 紀錄內容
        /// </summary>
        [Display(Name = "紀錄內容")]
        [StringLength(1000, MinimumLength = 1, ErrorMessage = "{0} 有長度限制1~1000")]
        [Required(ErrorMessage = "{0} 不可為空")]
        public string Note { get; set; }
    }
}
