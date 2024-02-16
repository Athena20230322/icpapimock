using System;

namespace ICP.Modules.Mvc.Admin.Models.Finance.MerchantTradeDetail
{
    /// <summary>
    /// 特店帳務進出明細 查詢結果
    /// </summary>
    public class QryMerchantTradeDetailDbRes
    {
        /// <summary>
        /// 轉帳進出日期
        /// </summary>
        public DateTime CreateDate { get; set; }

        /// <summary>
        /// 電支帳號
        /// </summary>
        public string ICPMID { get; set; }

        /// <summary>
        /// 名稱
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// 交易時間
        /// </summary>
        public DateTime PaymentDate { get; set; }

        /// <summary>
        /// 交易編號
        /// </summary>
        public string TradeNo { get; set; }

        /// <summary>
        /// 帳務類型
        /// </summary>
        public string TradeModeCName { get; set; }

        /// <summary>
        /// 交易類型
        /// </summary>
        public string PaymentTypeName { get; set; }

        /// <summary>
        /// 交易子類型
        /// </summary>
        public string PaymentSubTypeName { get; set; }

        /// <summary>
        /// 交易收入
        /// </summary>
        public int Income { get; set; }

        /// <summary>
        /// 交易支出
        /// </summary>
        public int Payment { get; set; }

        /// <summary>
        /// 交易後餘額
        /// </summary>
        public int NewCash { get; set; }


        /// <summary>
        /// 總交易收入
        /// </summary>
        public int SumIncome { get; set; }

        /// <summary>
        /// 總交易支出
        /// </summary>
        public int SumPayment { get; set; }

        /// <summary>
        /// 總筆數
        /// </summary>
        public int TotalCount { get; set; }
    }
}
