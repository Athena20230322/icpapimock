using ICP.Infrastructure.Core.Models;
using ICP.Infrastructure.Core.Models.Consts;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace ICP.Modules.Mvc.Admin.Models.Finance.TradeDetail
{
    /// <summary>
    /// 實質交易明細查詢 查詢條件
    /// </summary>
    public class QryTradeDetailReq : PageModel
    {
        /// <summary>
        /// 日期條件選單
        /// </summary>
        public IEnumerable<SelectListItem> DateTypeList { get; set; }

        /// <summary>
        /// 日期條件
        /// </summary>
        /// <remarks>1:訂單日期(預設) 2:付款日期 3:傳輸日期 4:撥款日期 5:退款日期</remarks>
        public int DateType { get; set; }

        /// <summary>
        /// 起始日期
        /// </summary>
        [Display(Name = "起始日期")]
        [Required(ErrorMessage = "請選擇日期")]
        [RegularExpression(RegexConst.yyyyMMdd, ErrorMessage = "日期格式錯誤")]
        public string DateStart { get; set; } = DateTime.Now.AddDays(-3).ToString("yyyy-MM-dd");

        /// <summary>
        /// 結束日期
        /// </summary>
        [Display(Name = "結束日期")]
        [Required(ErrorMessage = "請選擇日期")]
        [RegularExpression(RegexConst.yyyyMMdd, ErrorMessage = "日期格式錯誤")]
        public string DateEnd { get; set; } = DateTime.Now.ToString("yyyy-MM-dd");

        /// <summary>
        /// 訂單類別選單
        /// </summary>
        [Display(Name = "訂單類別")]
        public IEnumerable<SelectListItem> TradeNoTypeList { get; set; }

        /// <summary>
        /// 訂單類別
        /// </summary>
        /// <remarks>1:icashpay訂單編號(TradeNo) 2:特店訂單編號(MerchantTradeNo)</remarks>
        public int TradeNoType { get; set; }

        /// <summary>
        /// 訂單編號
        /// </summary>
        [Display(Name = "訂單編號")]
        public string TradeNo { get; set; }

        /// <summary>
        /// 付款狀態選單
        /// </summary>
        [Display(Name = "付款狀態")]
        public IEnumerable<SelectListItem> PaymentStatusList { get; set; }

        /// <summary>
        /// 付款狀態
        /// </summary>
        /// <remarks>0:全部(預設) 1:未付款 2:已付款 3:已退款 4:付款失敗 5:退款失敗</remarks>
        public int PaymentStatus { get; set; }

        /// <summary>
        /// 交易類型選單
        /// </summary>
        [Display(Name = "交易類型")]
        public IEnumerable<SelectListItem> TradeStatusList { get; set; }

        /// <summary>
        /// 交易類型
        /// </summary>
        /// <remarks>0:全部(預設) 1:銷售(交易) 2:銷退(退款) 3:銷售/銷退 4:沖正</remarks>
        public int TradeStatus { get; set; }

        /// <summary>
        /// 電支帳號類型選單
        /// </summary>
        [Display(Name = "電支帳號")]
       public IEnumerable<SelectListItem> ICPMIDTypeList { get; set; }

        /// <summary>
        /// 電支帳號類型
        /// </summary>
        /// <remarks>1:收款方 2:付款方</remarks>
        public int ICPMIDType { get; set; }

        /// <summary>
        /// 電支帳號
        /// </summary>
        public string ICPMID { get; set; }

        /// <summary>
        /// 撥款狀態選單
        /// </summary>
        [Display(Name = "撥款狀態")]
        public IEnumerable<SelectListItem> AllocateStatusList { get; set; }

        /// <summary>
        /// 撥款狀態
        /// </summary>
        /// <remarks>0:全部(預設) 1:未撥款 2:已撥款</remarks>
        public int AllocateStatus { get; set; }

        /// <summary>
        /// 付款方式選單
        /// </summary>
        [Display(Name = "付款方式")]
        public IEnumerable<SelectListItem> PaymentTypeList { get; set; }

        /// <summary>
        /// 付款方式
        /// </summary>
        /// <remarks>0:全部(預設) 1:icashpay帳戶 2:連結扣款帳戶</remarks>
        public int PaymentType { get; set; }

        /// <summary>
        /// 平台商編號
        /// </summary>
        [Display(Name = "平台商編號")]
        public long? PlatformID { get; set; } = null;

        public new int PageSize { get; set; } = 10;
    }
}
