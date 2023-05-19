using ICP.Infrastructure.Core.Extensions;
using ICP.Modules.Mvc.Payment.Models.Payment;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICP.Modules.Mvc.Payment.Models.Cashier
{
    public class CashierReq
    {
        /// <summary>
        /// 訂單流水號
        /// </summary>
        public long TradeID { get; set; }

        /// <summary>
        /// 訂單編號
        /// </summary>
        public string TradeNo { get; set; }

        /// <summary>
        /// 廠商編號
        /// </summary>
        [Display(Name="廠商編號")]
        [Range(1, long.MaxValue, ErrorMessage = "{0} 為必填")]
        public long MerchantID { get; set; }

        /// <summary>
        /// 平台編號
        /// </summary>
        [Display(Name = "平台商編號")]
        public string PlatformID { get; set; }

        /// <summary>
        /// 錢包種類代碼
        /// </summary>
        [Display(Name= "錢包種類代碼")]
        //[Required(ErrorMessage = "{0} 為必填")]
        public string WalletID { get; set; }

        /// <summary>
        /// 交易金額
        /// </summary>
        [Display(Name = "交易金額")]
        [Range(1, long.MaxValue, ErrorMessage = "{0} 為必填")]
        public decimal Amount { get; set; }

        /// <summary>
        /// 商店編號
        /// </summary>
        public string StoreID { get; set; }

        /// <summary>
        /// 商店名稱
        /// </summary>
        //[Display(Name = "廠商名稱")]
        //[Required(ErrorMessage = "{0} 欄位為必填")]
        public string StoreName { get; set; }

        /// <summary>
        /// 交易編號
        /// </summary>
        [Display(Name = "交易編號")]
        [Required(ErrorMessage = "{0} 為必填")]
        public string MerchantTradeNo { get; set; }

        /// <summary>
        /// 交易日期
        /// </summary>
        [Display(Name = "交易日期")]
        [Required,DataType(DataType.DateTime,ErrorMessage = "{0} 為必填")]
        public string MerchantTradeDate { get; set; }

        /// <summary>
        /// 載具類型
        /// </summary>
        [Display(Name = "載具類型")]
        //[Required(ErrorMessage = "{0} 為必填")]
        public string CarrierType { get; set; }

        /// <summary>
        /// 一般交易金額
        /// </summary>
        public decimal ItemAmt { get; set; }

        /// <summary>
        /// 代收交易金額
        /// </summary>
        public decimal UtilityAmt { get; set; }

        /// <summary>
        /// 代售交易金額
        /// </summary>
        public decimal CommAmt { get; set; }

        /// <summary>
        /// 排他一交易金額(菸品)
        /// </summary>
        public decimal ExceptAmt1 { get; set; }

        /// <summary>
        /// 排他二交易金額(預留)
        /// </summary>
        public decimal ExceptAmt2 { get; set; }

        /// <summary>
        /// 點抵金開關(0:關閉 1:開啟 用來設定此筆交易是否不可以使用紅利折抵。)
        /// </summary>
        public int RedeemFlag { get; set; }

        /// <summary>
        /// 紅利折抵金額
        /// </summary>
        public decimal BonusAmt { get; set; }

        /// <summary>
        /// 紅利折抵點數
        /// </summary>
        public decimal DebitPoint { get; set; }

        /// <summary>
        /// 不可折抵金額
        /// </summary>
        public decimal NonRedeemAmt { get; set; }

        /// <summary>
        /// 不可贈點金額
        /// </summary>
        public decimal NonPointAmt { get; set; }

        /// <summary>
        /// 交易幣別
        /// </summary>
        public string Ccy { get; set; }       

        /// <summary>
        /// 交易模式(交易:1 儲值:2 轉帳:3 提領:4)
        /// </summary>
        [Display(Name = "交易模式")]
        public int TradeModeID { get; set; }        

        /// <summary>
        /// 交易識別碼(條碼)
        /// </summary>
        public string Barcode { get; set; }

        /// <summary>
        /// 儲值金額
        /// </summary>
        public decimal TopUpAmt { get; set; }

        /// <summary>
        /// 組數因POS單筆可登錄明細上限最多到61組
        /// </summary>
        public string ItemList { get; set; }
        
        /// <summary>
        /// 付款人會員編號
        /// </summary>
        [Display(Name = "付款會員代碼")]
        [Range(1, long.MaxValue, ErrorMessage = "{0} 為必填")]
        public long MID { get; set; }

        /// <summary>
        /// 交易類型(EC:1 POS:2)
        /// </summary>
        [Display(Name = "交易類型(EC:1 POS:2)")]
        [Range(1, 2, ErrorMessage = "{0}僅能輸入1或2")]
        public int TradeType { get; set; }

        /// <summary>
        /// 付款方式代碼
        /// </summary>                
        [Display(Name = "付款方式代碼(PaymentTypeID)")]
        public int PaymentTypeID { get; set; }

        /// <summary>
        /// 收單行名稱代碼
        /// </summary>
        [Display(Name = "付款子類別(PaymentSubTypeID)")]
        [Range(1, long.MaxValue, ErrorMessage = "{0} 為必填")]
        public int PaymentSubTypeID { get; set; }

        /// <summary>
        /// 收銀機交易序號
        /// </summary>
        public string PosRefNo { get; set; }

        /// <summary>
        /// 商家的終端機編號或POS機編號
        /// </summary>
        public string MerchantTid { get; set; }

        /// <summary>
        /// 銀行帳號識別碼
        /// </summary>
        public string AccountID { get; set; }

        /// <summary>
        /// 檢核碼
        /// </summary>
        public string CheckMacValue { get; set; }

        /// <summary>
        /// 是否為自動流程(0:非自動(手動) 1:自動) ex:自動儲值
        /// </summary>
        public int Automation { get; set; }

        /// <summary>
        /// 付款方式ID
        /// </summary>
        public string PayID { get; set; }

        /// <summary>
        /// OW確認付款時間
        /// </summary>
        public DateTime? OWSubmitDate { get; set; }

        /// <summary>
        /// 備註
        /// </summary>
        public string Remark { get; set; }
    }
}

