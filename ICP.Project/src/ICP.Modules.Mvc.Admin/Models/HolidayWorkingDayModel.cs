using ICP.Infrastructure.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace ICP.Modules.Mvc.Admin.Models
{
    public class HolidayWorkingDayModel : BaseListModel
    {
        /// <summary>
        /// 流水號
        /// </summary>
        public int DayID { get; set; }

        /// <summary>
        /// 假日或補班日名稱或原因
        /// </summary>
        [StringLength(100, ErrorMessage = "假日或補班的【原因】不符字數限制", MinimumLength = 3)]
        public string DayDescription { get; set; }

        /// <summary>
        /// 假日或補班日日期
        /// </summary>
        [Required]
        public DateTime DayDate { get; set; }

        /// <summary>
        /// 類型：1 休假日 2 補班日
        /// </summary>
        [Range(1, 2, ErrorMessage = "假日或補班的【類型】不符")]
        public int DayType { get; set; }

        /// <summary>
        /// 建立時間
        /// </summary>
        public DateTime CreateDate { get; set; }

        /// <summary>
        /// 建立人
        /// </summary>
        public string Creator { get; set; }

        /// <summary>
        /// 修改時間
        /// </summary>
        public DateTime? ModifyDate { get; set; }

        /// <summary>
        /// 修改人
        /// </summary>
        public string Modifier { get; set; }
    }
}
