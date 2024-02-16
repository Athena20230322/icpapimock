using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICP.Modules.Mvc.Payment.Models.ChargeBack
{
    public class ChargeBackReq
    {
        /// <summary>
        /// 會員代碼
        /// </summary>
        [Display(Name = "付款會員代碼")]
        public long MID { get; set; }

        /// <summary>
        /// 廠商編號
        /// </summary>
        [Display(Name = "廠商編號")]
        [Range(1, long.MaxValue, ErrorMessage = "{0} 為必填")]
        public long MerchantID { get; set; }

        /// <summary>
        /// 平台編號
        /// </summary>
        [Display(Name = "平台商編號")]
        public string PlatformID { get; set; }

        /// <summary>
        /// 愛金卡交易序號
        /// </summary>
        [Display(Name = "愛金卡交易序號")]
        public string TransactionID { get; set; }

        /// <summary>
        /// 退貨金額
        /// </summary>
        [Display(Name = "退款金額")]
        public decimal Amount { get; set; }

        /// <summary>
        /// 商店ID
        /// </summary>
        [Display(Name = "商店ID")]
        public string StoreID { get; set; }

        /// <summary>
        /// 商店名稱
        /// </summary>
        [Display(Name = "商店名稱")]
        public string StoreName { get; set; }

        /// <summary>
        /// 交易編號
        /// </summary>
        [Display(Name = "交易編號")]
        [Required(ErrorMessage = "{0} 為必填")]
        public string MerchantTradeNo { get; set; }

        /// <summary>
        /// 交易退貨日期
        /// </summary>
        [Display(Name = "交易退貨日期")]
        [Required, DataType(DataType.DateTime, ErrorMessage = "{0} 為必填")]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd HH:mm:ss}", ApplyFormatInEditMode = true)]
        public string MerchantTradeDate { get; set; }

        /// <summary>
        /// 收銀機交易序號
        /// </summary>
        [Display(Name = "POS交易序號")]
        public string PosRefNo { get; set; }

        /// <summary>
        /// 商家的終端機編號或POS機編號
        /// </summary>
        [Display(Name = "POS編號")]
        public string MerchantTID { get; set; }

        /// <summary>
        /// 檢核碼
        /// </summary>
        public string CheckMacValue { get; set; }

        /// <summary>
        /// 紅利返還金額
        /// </summary>
        public decimal BonusAmt { get; set; }

        /// <summary>
        /// 紅利返還點數
        /// </summary>
        public decimal DebitPoint { get; set; }
    }
}
