using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace ICP.Library.Models.MailLibrary
{
    public class NotifyContent
    {
        /// <summary>
        /// Notify 編號
        /// </summary>
        public long NotifyID { get; set; }

        /// <summary>
        /// Notify 代碼
        /// </summary>
        public string NotifyKey { get; set; }

        /// <summary>
        /// 標題
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// 內容
        /// </summary>
        public string Body { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// 建立時間
        /// </summary>
        public DateTime CreateDate { get; set; }

        /// <summary>
        /// 建立者
        /// </summary>
        public string Creator { get; set; }

        /// <summary>
        /// 修改時間
        /// </summary>
        public DateTime? ModifyDate { get; set; }

        /// <summary>
        /// 修改者
        /// </summary>
        public string Modifier { get; set; }

        /// <summary>
        /// 版型編號
        /// </summary>
        public long LayoutID { get; set; }

        /// <summary>
        /// 版型代碼
        /// </summary>
        public string LayoutKey { get; set; }

        /// <summary>
        /// 不含Layout的內容
        /// </summary>
        public string BodyWithoutLayout { get; set; }
    }
}
