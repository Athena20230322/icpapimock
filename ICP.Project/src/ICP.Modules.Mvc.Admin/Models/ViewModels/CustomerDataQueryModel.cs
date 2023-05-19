using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.ComponentModel.DataAnnotations;

namespace ICP.Modules.Mvc.Admin.Models.ViewModels
{
    using Infrastructure.Core.Models;
    using Infrastructure.Core.Models.Consts;

    /// <summary>
    /// 特店查詢條件
    /// </summary>
    public class CustomerDataQueryModel: PageModel
    {
        /// <summary>
        /// 過件狀態
        /// 0:未過件, 1:已過件
        /// </summary>
        public byte CustomerStatus { get; set; }

        /// <summary>
        /// 建立日期啟始
        /// </summary>
        public DateTime? CreateBegin { get; set; }

        /// <summary>
        /// 建立日期結束
        /// </summary>
        public DateTime? CreateEnd { get; set; }

        /// <summary>
        /// 商品類型 (and運算法)
        /// 1:實體
        /// 2:虛擬商品(點數/服務)
        /// 4:遞延商品(課程/SPA) 
        /// 8:商品代收 
        /// 16: 商品代售 
        /// </summary>
        public byte CommoditiyType { get; set; }

        /// <summary>
        /// 審核狀態
        /// </summary>
        public byte? AuditStatus { get; set; }

        /// <summary>
        /// 業務SaleID
        /// </summary>
        public int? SalesUserID { get; set; }

        /// <summary>
        /// 公司名稱
        /// </summary>
        public string CompanyName { get; set; }

        /// <summary>
        /// 負責人/個人姓名
        /// </summary>
        [RegularExpression(RegexConst.ChineseCName, ErrorMessage = "請輸入正確格式的{0}")]
        [StringLength(20, MinimumLength = 2, ErrorMessage = "請輸入正確格式的{0}")]
        [Display(Name = "姓名")]
        public string CName { get; set; }

        /// <summary>
        /// 統一編號
        /// </summary>
        [RegularExpression(RegexConst.UnifiedBusinessNo, ErrorMessage = "請輸入正確格式的{0}")]
        [Display(Name = "統一編號")]
        public string UnifiedBusinessNo { get; set; }

        /// <summary>
        /// 身分證字號
        /// </summary>
        [RegularExpression(RegexConst.IDNO, ErrorMessage = "請輸入正確格式的{0}")]
        [Display(Name = "身分證字號")]
        public string IDNO { get; set; }

        /// <summary>
        /// 統一證號/居留證號
        /// </summary>
        [RegularExpression(RegexConst.UniformID, ErrorMessage = "請輸入正確格式的{0}")]
        [Display(Name = "統一證號/居留證號")]
        public string UniformID { get; set; }

        /// <summary>
        /// MCCCode
        /// </summary>
        public int? MCCCode { get; set; }

        /// <summary>
        /// 金流維護狀態
        /// </summary>
        public byte? MerchantStates { get; set; }
    }
}
