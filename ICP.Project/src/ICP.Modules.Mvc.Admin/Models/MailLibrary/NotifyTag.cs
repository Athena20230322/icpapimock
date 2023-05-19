using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using ICP.Infrastructure.Core.Models;

namespace ICP.Modules.Mvc.Admin.Models.MailLibrary
{
    public class NotifyTag: ValidatableObject
    {
        /// <summary>
        /// Notify 編號
        /// </summary>
        public long NotifyID { get; set; }

        /// <summary>
        /// 標簽代碼
        /// </summary>
        [Required]
        [StringLength(50)]
        public string TagKey { get; set; }

        /// <summary>
        /// 標簽編號
        /// </summary>
        public long TagID { get; set; }

        /// <summary>
        /// 標簽名稱
        /// </summary>
        [Required]
        [StringLength(10)]
        public string Name { get; set; }
    }
}
