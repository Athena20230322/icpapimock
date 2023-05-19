using System;

namespace ICP.Modules.Mvc.Payment.Models.BaseMember
{
    /// <summary>
    /// 帳戶資訊>帳戶明細 請求
    /// </summary>
    public class AccountRecordDbRes
    {
        /// <summary>
        /// 訂單流水號
        /// </summary>
        public long TradeID { get; set; }

        /// <summary>
        /// 訂單建立時間
        /// </summary>
        public DateTime CreateDate { get; set; }

        /// <summary>
        /// 付款時間
        /// </summary>
        public DateTime PaymentDate { get; set; }

        /// <summary>
        /// 退款時間
        /// </summary>
        public DateTime RefundDate { get; set; }

        /// <summary>
        /// 轉帳時間
        /// </summary>
        public DateTime TransferTime { get; set; }

        /// <summary>
        /// 提領排程日期
        /// </summary>
        public DateTime YYYYMMDD { get; set; }

        /// <summary>
        /// 交易模式(1:交易, 2:儲值, 3:轉帳, 4:提領)
        /// </summary>
        public int TradeModeID { get; set; }

        /// <summary>
        /// 交易狀態 0:未付款, 1:交易完成, 2:全額退款, 3:部分退款, 4:取消交易(沖正), 5:付款待入帳, 6:入帳失敗
        /// </summary>
        public int TradeStatus { get; set; }

        /// <summary>
        /// 總金額
        /// </summary>
        public decimal TotalAmount { get; set; }

        /// <summary>
        /// 實際付款金額
        /// </summary>
        public decimal Amount { get; set; }

        /// <summary>
        /// 退款金額
        /// </summary>
        public decimal RefundAMT { get; set; }

        /// <summary>
        /// 項目名稱
        /// </summary>
        public string TitleName { get; set; }

        /// <summary>
        /// 付款方式
        /// </summary>
        public int PaymentTypeID { get; set; }

        /// <summary>
        /// 轉帳是否為轉入(1:轉入, 0:轉出)
        /// </summary>
        public int ReceiveTransfer { get; set; }

        /// <summary>
        /// 會員編號
        /// </summary>
        public long MID { get; set; }

        #region 自動載入下頁用

        /// <summary>
        /// 帳戶紀錄類別
        /// </summary>
        public int AccRecordType { get; set; }

        /// <summary>
        /// 帳戶查詢區間類別
        /// </summary>
        public int DateType { get; set; }

        /// <summary>
        /// 查詢起始日期
        /// </summary>
        public DateTime StartDate { get; set; }

        /// <summary>
        /// 查詢結束日期
        /// </summary>
        public DateTime EndDate { get; set; }

        /// <summary>
        /// 查詢關鍵字
        /// </summary>
        public string KeyWords { get; set; }

        /// <summary>
        /// 每頁筆數
        /// </summary>
        public int PageSize { get; set; }

        /// <summary>
        /// 最大資料行
        /// </summary>
        public int MaxRow { get; set; }

        /// <summary>
        /// 資料流水號
        /// </summary>
        public int RowID { get; set; }

        #endregion
    }
}