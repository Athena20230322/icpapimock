using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using ICP.Infrastructure.Core.Models;

namespace ICP.Modules.Mvc.Admin.Models.MailLibrary
{
    public class NotifyContentModel: ValidatableObject
    {
        /// <summary>
        /// Notify 編號
        /// </summary>
        public long NotifyID { get; set; }

        /// <summary>
        /// Notify 代碼
        /// </summary>
        [Required]
        [StringLength(50)]
        [Display(Name = "Notify 代碼")]
        public string NotifyKey { get; set; }

        /// <summary>
        /// 標題
        /// </summary>
        [Required]
        [StringLength(300)]
        [Display(Name = "標題")]
        public string Title { get; set; }

        /// <summary>
        /// 內容
        /// </summary>
        [Required]
        [System.Web.Mvc.AllowHtml]
        [Display(Name = "內容")]
        public string Body { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        [Required]
        [StringLength(300)]
        [Display(Name = "描述")]
        public string Description { get; set; }

        /// <summary>
        /// 版型編號
        /// </summary>
        [Display(Name = "版型編號")]
        public long LayoutID { get; set; }

        /// <summary>
        /// 版型代碼
        /// </summary>
        [StringLength(50)]
        [Display(Name = "版型代碼")]
        public string LayoutKey { get; set; }

        /// <summary>
        /// 建立者
        /// </summary>
        [Display(Name = "建立者")]
        public string Creator { get; set; }

        /// <summary>
        /// 修改者
        /// </summary>
        [Display(Name = "修改者")]
        public string Modifier { get; set; }
    }
}
