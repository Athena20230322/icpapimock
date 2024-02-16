using ICP.Infrastructure.Core.ExtraAttribute;
using ICP.Infrastructure.Core.Models;
using Newtonsoft.Json;
using System;

namespace ICP.Modules.Api.Payment.Models.ChargeOnline
{
    public class PaymentTempTradeDbRes : BaseResult
    {
        /// <summary>
        /// 廠商編號
        /// </summary>
        public long MerchantID { get; set; }

        /// <summary>
        /// 平台編號
        /// </summary>
        public long PlatformID { get; set; }

        /// <summary>
        /// 錢包種類代碼
        /// </summary>
        public string WalletId { get; set; }

        /// <summary>
        /// 交易金額
        /// </summary>
        public decimal Amount { get; set; }

        /// <summary>
        /// 商店編號
        /// </summary>
        public string StoreId { get; set; }

        /// <summary>
        /// 商店名稱
        /// </summary>
        public string StoreName { get; set; }

        /// <summary>
        /// 交易編號
        /// </summary>
        public string MerchantTradeNo { get; set; }

        /// <summary>
        /// 交易日期
        /// </summary>
        [JsonConverter(typeof(CustomDateTimeConverter))]
        public DateTime MerchantTradeDate { get; set; }

        /// <summary>
        /// 載具類型
        /// </summary>
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

        /// <summary>
        /// 組數因POS單筆可登錄明細上限最多到61組
        /// </summary>
        public string ItemList
        {
            get; set;
        }

        /// <summary>
        /// 付款人會員編號
        /// </summary>
        public long MID { get; set; }

        /// <summary>
        /// 付款方式識別碼
        /// </summary>
        public string PayID { get; set; }

        /// <summary>
        /// 備註
        /// </summary>
        public string Remark { get; set; }
    }
}

