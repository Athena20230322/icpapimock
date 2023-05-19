using ICP.Infrastructure.Core.Models;
using System;
using System.ComponentModel.DataAnnotations;

namespace ICP.Modules.Mvc.Admin.Models.Finance.MerchantTradeDetail
{
    /// <summary>
    /// 特店帳務進出明細 查詢結果
    /// </summary>
    public class QryMerchantTradeDetailRes : BaseListModel
    {
        /// <summary>
        /// 轉帳進出日期
        /// </summary>
        [Display(Name = "轉帳進出日期")]
        public DateTime CreateDate { get; set; }

        /// <summary>
        /// 電支帳號
        /// </summary>
        [Display(Name = "電支帳號")]
        public string ICPMID { get; set; }

        /// <summary>
        /// 名稱
        /// </summary>
        [Display(Name = "名稱")]
        public string UserName { get; set; }

        /// <summary>
        /// 交易時間
        /// </summary>
        [Display(Name = "交易時間")]
        public DateTime PaymentDate { get; set; }

        /// <summary>
        /// 交易編號
        /// </summary>
        [Display(Name = "交易編號")]
        public string TradeNo { get; set; }

        /// <summary>
        /// 帳務類型
        /// </summary>
        [Display(Name = "帳務類型")]
        public string TradeModeCName { get; set; }

        /// <summary>
        /// 交易類型
        /// </summary>
        [Display(Name = "交易類型")]
        public string PaymentTypeName { get; set; }

        /// <summary>
        /// 交易子類型
        /// </summary>
        [Display(Name = "交易子類型")]
        public string PaymentSubTypeName { get; set; }

        /// <summary>
        /// 交易收入
        /// </summary>
        [Display(Name = "交易收入")]
        public int Income { get; set; }

        /// <summary>
        /// 交易支出
        /// </summary>
        [Display(Name = "交易支出")]
        public int Payment { get; set; }

        /// <summary>
        /// 交易後餘額
        /// </summary>
        [Display(Name = "交易後餘額")]
        public int NewCash { get; set; }


        /// <summary>
        /// 總交易收入
        /// </summary>
        public int SumIncome { get; set; }

        /// <summary>
        /// 總交易支出
        /// </summary>
        public int SumPayment { get; set; }
    }
}
