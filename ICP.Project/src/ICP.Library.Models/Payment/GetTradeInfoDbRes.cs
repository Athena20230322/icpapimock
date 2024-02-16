using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICP.Library.Models.Payment
{
    public class GetTradeInfoDbRes
    {
        /// <summary>
        /// 交易記錄流水號
        /// </summary>
        public long TradeID { get; set; }

        /// <summary>
        /// 愛金卡交易序號
        /// </summary>
        public string TradeNo { get; set; }

        /// <summary>
        /// 付款方式代碼
        /// </summary>
        public int PaymentTypeID { get; set; }

        /// <summary>
        /// 付款方式子類別代碼
        /// </summary>
        public int PaymentSubTypeID { get; set; }

        /// <summary>
        /// 會員編號
        /// </summary>
        public long MID { get; set; }

        /// <summary>
        /// 廠商編號
        /// </summary>
        public long MerchantID { get; set; }

        /// <summary>
        /// 平台商編號
        /// </summary>
        public long PlatformID { get; set; }

        /// <summary>
        /// 交易編號
        /// </summary>
        public string MerchantTradeNo { get; set; }

        /// <summary>
        /// 交易日期時間
        /// </summary>
        public DateTime MerchantTradeDate { get; set; }

        /// <summary>
        /// 交易狀態 0 : 未付款 1 : 交易完成 2 : 全額退款 3 : 部分退款 4 : 取消交易(沖正) 5: 付款待入帳 6 : 入帳失敗
        /// </summary>
        public int TradeStatus { get; set; }

        /// <summary>
        /// 付款狀態 0:未付款 1: 已付款
        /// </summary>
        public int PaymentStatus { get; set; }

        /// <summary>
        /// 付款日期
        /// </summary>
        public DateTime? PaymentDate { get; set; }

        /// <summary>
        /// 撥款狀態 0:未撥款 1:已撥款
        /// </summary>
        public int AllocateStatus { get; set; }

        /// <summary>
        /// 撥款日期
        /// </summary>
        public DateTime AllocateDate { get; set; }

        /// <summary>
        /// 交易總金額
        /// </summary>
        public decimal Amount { get; set; }

        /// <summary>
        /// 紅利折抵金額
        /// </summary>
        public decimal BonusAmt { get; set; }

        /// <summary>
        /// 退款金額
        /// </summary>
        public decimal RefundAMT { get; set; }

        /// <summary>
        /// 退款時間
        /// </summary>
        public DateTime RefundDate { get; set; }

        /// <summary>
        /// 傳輸日期(僅超商使用-請款檔傳輸日期)
        /// </summary>
        public DateTime TransmittalDate { get; set; }

        /// <summary>
        /// 建立日期
        /// </summary>
        public DateTime CreateDate { get; set; }

        /// <summary>
        /// 付款方式代碼
        /// </summary>
        public string PayID { get; set; }

        /// <summary>
        /// 交易模式(交易:1 儲值:2 轉帳:3 提領:4)
        /// </summary>
        public int TradeModeID { get; set; }

        /// <summary>
        /// 備註
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// OW確認付款時間
        /// </summary>
        public DateTime OWSubmitDate { get; set; }

        /// <summary>
        /// 交易類型(EC:1 POS:2 其他:99)
        /// </summary>
        public int TradeType { get; set; }

        /// <summary>
        /// ICASH電支帳號
        /// </summary>
        public string ICPMID { get; set; }

        /// <summary>
        /// 銀行代碼
        /// </summary>
        public string BankCode { get; set; }

        /// <summary>
        /// 銀行名稱
        /// </summary>
        public string BankName { get; set; }

        /// <summary>
        /// 銀行簡稱
        /// </summary>
        public string BankShortName { get; set; }

        /// <summary>
        /// app 顯示名稱
        /// </summary>
        public string BankAppName { get; set; }

        /// <summary>
        /// 銀行的中文簡稱
        /// </summary>
        public string DisplayShortNameTW { get; set; }

        /// <summary>
        /// 銀行的中文簡稱
        /// </summary>
        public string DisplayShortNameEN { get; set; }
    }
}
