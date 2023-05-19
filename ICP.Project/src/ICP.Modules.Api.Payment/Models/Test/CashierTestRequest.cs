using ICP.Infrastructure.Core.Extensions;
using ICP.Library.Models.AuthorizationApi;
using ICP.Modules.Api.Payment.Models.Cashier;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICP.Modules.Api.Payment.Models.Test
{
    public class CashierTestRequest : BaseAuthorizationApiRequest
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
        public long PlatformID { get; set; }

        /// <summary>
        /// 錢包種類代碼
        /// </summary>
        [Display(Name= "錢包種類代碼")]
        [Required(ErrorMessage = "{0} 為必填")]
        public string walletId { get; set; }

        /// <summary>
        /// 交易金額
        /// </summary>
        [Display(Name = "交易金額")]
        [Range(1, long.MaxValue, ErrorMessage = "{0} 為必填")]
        public decimal amount { get; set; }

        /// <summary>
        /// 商店編號
        /// </summary>
        public string storeid { get; set; }

        /// <summary>
        /// 商店名稱
        /// </summary>
        //[Display(Name = "廠商名稱")]
        //[Required(ErrorMessage = "{0} 欄位為必填")]
        public string storeName { get; set; }

        /// <summary>
        /// 交易編號
        /// </summary>
        [Display(Name = "交易編號")]
        [Required(ErrorMessage = "{0} 為必填")]
        public string merchantTradeNo { get; set; }

        /// <summary>
        /// 交易日期
        /// </summary>
        [Display(Name = "交易日期")]
        [Required,DataType(DataType.DateTime,ErrorMessage = "{0} 為必填")]
        [DisplayFormat(DataFormatString= "{yyyy/MM/dd HH:mm:ss}")]
        public string merchantTradeDate { get; set; }

        /// <summary>
        /// 載具類型
        /// </summary>
        [Display(Name = "載具類型")]
        [Required(ErrorMessage = "{0} 為必填")]
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
        public int TradeModeID { get; set; }        

        /// <summary>
        /// 交易識別碼(條碼)
        /// </summary>
        public string barcode { get; set; }

        /// <summary>
        /// 儲值金額
        /// </summary>
        public decimal TopUpAmt { get; set; }

        private string xmlStr = "";

        /// <summary>
        /// 組數因POS單筆可登錄明細上限最多到61組
        /// </summary>
        public string ItemList
        {
            set; get;
        }

        /// <summary>
        /// ItemList轉化為xml String
        /// </summary>
        public string ItemXml
        {
            get
            {
                return xmlStr;
            }
        }

        public List<ItemModel> Items
        {
            set
            {
                ItemList = JsonConvert.SerializeObject(value);
            }
        }

        /// <summary>
        /// 付款人會員編號
        /// </summary>
        public long MID { get; set; }

        /// <summary>
        /// 交易類型(EC:1 POS:2)
        /// </summary>
        public int TradeType { get; set; }

        /// <summary>
        /// 付款方式代碼
        /// </summary>
        public int PaymentTypeID { get; set; }

        /// <summary>
        /// 收單行名稱代碼
        /// </summary>
        public int PaymentSubTypeID { get; set; }

        /// <summary>
        /// 收銀機交易序號
        /// </summary>
        public string posRefNo { get; set; }

        /// <summary>
        /// 商家的終端機編號或POS機編號
        /// </summary>
        public string merchantTid { get; set; }

        /// <summary>
        /// 銀行帳號識別碼
        /// </summary>
        public string AccountID { get; set; }

        /// <summary>
        /// 檢核碼
        /// </summary>
        public string CheckMacValue { get; set; }

        /// <summary>
        /// 愛金交易序號
        /// </summary>
        public string transactionId { get; set; }

        public string Account { get; set; }
    }
}

