using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICP.Modules.Mvc.Admin.Models.ViewModels
{
    using ICP.Infrastructure.Core.Models.Consts;
    using Infrastructure.Core.Models;
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// 特店查詢結果
    /// </summary>
    public class CustomerDataQueryResult : BaseListModel
    {
        /// <summary>
        /// 特店編號
        /// </summary>
        public long CustomerID { get; set; }

        /// <summary>
        /// 會員編號
        /// </summary>
        public long MID { get; set; }

        /// <summary>
        /// 審核狀態
        /// </summary>
        public byte AuditStatus { get; set; }

        /// <summary>
        /// 備註
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// 業務SaleID
        /// </summary>
        public int SalesUserID { get; set; }

        /// <summary>
        /// 建立日期
        /// </summary>
        public DateTime CreateDate { get; set; }

        /// <summary>
        /// 最後修改日期
        /// </summary>
        public DateTime? ModifyDate { get; set; }

        /// <summary>
        /// 網站名稱
        /// </summary>
        public string WebSiteName { get; set; }

        /// <summary>
        /// 網站網址
        /// </summary>
        public string WebSiteURL { get; set; }

        /// <summary>
        /// 公司名稱
        /// </summary>
        public string CompanyName { get; set; }

        /// <summary>
        /// 統一編號
        /// </summary>
        public string UnifiedBusinessNo { get; set; }

        /// <summary>
        /// 身分證字號
        /// </summary>
        public string IDNO { get; set; }

        /// <summary>
        /// 統一證號/居留證號
        /// </summary>
        public string UniformID { get; set; }

        /// <summary>
        /// 護照號碼
        /// </summary>
        public string OverSeasIDNO { get; set; }

        /// <summary>
        /// 負責人/個人姓名
        /// </summary>
        public string CName { get; set; }

        /// <summary>
        /// 負責業務
        /// </summary>
        public string SalesUserName { get; set; }

        /// <summary>
        /// 金流維護狀態
        /// </summary>
        public byte States { get; set; }
    }
}
