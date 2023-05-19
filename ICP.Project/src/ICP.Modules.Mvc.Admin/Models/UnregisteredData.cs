using ICP.Infrastructure.Core.Models;
using System;

namespace ICP.Modules.Mvc.Admin.Models
{
    public class UnregisteredData : BaseListModel
    {
        /// <summary>
        /// 會員編號
        /// </summary>
        public long MID { get; set; }

        /// <summary>
        /// ICASH 電支帳號
        /// </summary>
        public string ICPMID { get; set; }

        /// <summary>
        /// 姓名
        /// </summary>
        public string CName { get; set; }

        /// <summary>
        /// 手機號碼
        /// </summary>
        public string CellPhone { get; set; }

        /// <summary>
        /// 是否有身份證字號
        /// </summary>
        public bool isIDNO { get; set; }

        /// <summary>
        /// 是否有居留證
        /// </summary>
        public bool isUniformID { get; set; }

        /// <summary>
        /// 是否有Email
        /// </summary>
        public bool isEmail { get; set; }

        /// <summary>
        /// 備註
        /// </summary>
        public string Notes { get; set; }

        /// <summary>
        /// 來源
        /// 0 = 系統刪除
        /// 1 = 人工刪除
        /// </summary>
        public byte Source { get; set; }

        /// <summary>
        /// 建立時間
        /// </summary>
        public DateTime CreateDate { get; set; }

        /// <summary>
        /// 建立者
        /// </summary>
        public string CreateUser { get; set; }

        /// <summary>
        /// Real IP
        /// </summary>
        public long RealIP { get; set; }

        /// <summary>
        /// Proxy IP
        /// </summary>
        public long ProxyIP { get; set; }
    }
}
