using ICP.Infrastructure.Core.Models;
using ICP.Infrastructure.Core.Models.Consts;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICP.Modules.Mvc.Admin.Models
{
    public class SuspenseMain : BaseListModel
    {
        /// <summary>
        /// 流水號
        /// </summary>
        public long SuspenseID { get; set; }

        /// <summary>
        /// 會員編號
        /// </summary>
        public long MID { get; set; }

        /// <summary>
        /// 姓名
        /// </summary>
        public string CName { get; set; }

        /// <summary>
        /// 身分證字號
        /// </summary>
        public string IDNO { get; set; }

        /// <summary>
        /// 電子郵件
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// 手機號碼
        /// </summary>
        [Required(ErrorMessage = "請輸入手機號碼")]
        [RegularExpression(RegexConst.CellPhone, ErrorMessage = "請輸入正確的手機號碼")]
        [Display(Name = "手機號碼")]
        public string CellPhone { get; set; }

        /// <summary>
        /// 懲處方式
        /// 1 = 臨時停權
        /// 2 = 永久停權
        /// 3 = 結清
        /// </summary>
        [Required]
        [Range(1, 3)]
        [Display(Name = "懲處方式")]
        public byte SuspenseType { get; set; }

        /// <summary>
        /// 停權原因類別
        /// </summary>
        [Required]
        [Display(Name = "原因")]
        public byte ReasonType { get; set; }

        /// <summary>
        /// 階段
        /// 0 = 待審核
        /// 1 = 審核通過
        /// 2 = 審核失敗
        /// </summary>
        [Required]
        [Display(Name = "審核狀態")]
        public byte AuthStatus { get; set; }

        /// <summary>
        /// 審核時間
        /// </summary>
        public DateTime? AuthDate { get; set; }

        /// <summary>
        /// 狀態
        /// 1 = 正常
        /// 2 = 解除
        /// </summary>
        public byte Status { get; set; }

        /// <summary>
        /// 備註
        /// </summary>
        [Required(ErrorMessage = "備註未填")]
        [StringLength(200)]
        public string Note { get; set; }

        /// <summary>
        /// 原因說明
        /// </summary>
        [StringLength(200)]
        public string Reason { get; set; }

        /// <summary>
        /// 是否同步加入黑名單
        /// 0 = 否
        /// 1 = 是
        /// </summary>
        [Required]
        [Range(0, 1)]
        public bool IsBlockIDNO { get; set; }

        /// <summary>
        /// 建立時間
        /// </summary>
        public DateTime CreateDate { get; set; }

        /// <summary>
        /// 建立人
        /// </summary>
        public string CreateUser { get; set; }

        /// <summary>
        /// 解除停權時間
        /// </summary>
        public DateTime? UnlockDate { get; set; }

        /// <summary>
        /// 解除停權者
        /// </summary>
        public string UnlockUser { get; set; }
    }
}
