using ICP.Library.Models.AuthorizationApi;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace ICP.Modules.Api.Payment.Models.Pos
{
    public class TopUpReq : BasePosReq
    {
        /// <summary>
        /// 錢包種類代碼
        /// </summary>
        [Display(Name = "錢包種類代碼")]
        [StringLength(6, ErrorMessage = "{0} 最大長度為6")]
        public string WalletID { get; set; }

        /// <summary>
        /// 儲值識別碼(條碼)
        /// </summary>
        [Display(Name = "儲值識別碼(條碼)")]
        [Required(ErrorMessage = "{0} 為必填")]
        [StringLength(18, ErrorMessage = "{0} 最大長度為18")]
        public string Barcode { get; set; }

        /// <summary>
        /// 儲值幣別
        /// </summary>
        [Display(Name = "儲值幣別")]
        [Required(ErrorMessage = "{0} 為必填")]
        [StringLength(3, ErrorMessage = "{0} 最大長度為3")]
        public string Ccy { get; set; }

        /// <summary>
        /// 儲值金額
        /// </summary>
        [Display(Name = "儲值金額")]
        [Range(1, 9999999999.99, ErrorMessage = "{0} 為必填")]
        public decimal TopUpAmt { get; set; }

        /// <summary>
        /// 商店編號
        /// </summary>
        [Display(Name = "商店編號")]
        [StringLength(20, ErrorMessage = "{0} 最大長度為20")]
        public string StoreID { get; set; }

        /// <summary>
        /// 商店名稱
        /// </summary>
        [Display(Name = "商店名稱")]
        [StringLength(30, ErrorMessage = "{0} 最大長度為30")]
        public string StoreName { get; set; }

        /// <summary>
        /// POS交易序號
        /// </summary>
        [Display(Name = "POS交易序號")]
        [StringLength(6, ErrorMessage = "{0} 最大長度為6")]
        public string PosRefNo { get; set; }

        /// <summary>
        /// POS編號
        /// </summary>
        [Display(Name = "POS編號")]
        [StringLength(2, ErrorMessage = "{0} 最大長度為2")]
        public string MerchantTID { get; set; }

        /// <summary>
        /// 交易編號
        /// </summary>
        [Display(Name = "交易編號")]
        [Required(ErrorMessage = "{0} 為必填")]
        [StringLength(50, ErrorMessage = "{0} 最大長度為50")]
        public string MerchantTradeNo { get; set; }

        /// <summary>
        /// 交易日期
        /// </summary>
        [Display(Name = "交易日期")]
        [Required(ErrorMessage = "{0} 為必填")]
        [StringLength(20, ErrorMessage = "{0} 最大長度為20")]
        [DataType(DataType.DateTime, ErrorMessage = "{0} 應為日期格式")]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd HH:mm:ss}")]
        public string MerchantTradeDate { get; set; }

        /// <summary>
        /// 組數因POS單筆可登錄明細上限最多到61組
        /// </summary>
        public string ItemList { get; set; }

        /// <summary>
        /// 備註
        /// </summary>
        [Display(Name = "備註")]
        [StringLength(1000, ErrorMessage = "{0} 最大長度為1000")]
        public string Remark { get; set; }
    }
}