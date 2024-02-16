using System;
using System.Collections.Generic;

namespace ICP.Library.Models.EinvoiceLibrary
{
    /// <summary>
    /// 取得發票明細Result
    /// </summary>
    public class QueryEinvoiceDetailResult
    {
        public string TimeStamp { get; set; }

        /// <summary>
        /// 發票開立日期 格式：2019/01/01 00:00:00
        /// </summary>
        public string EinvoiceCreateDate { get; set; }

        /// <summary>
        /// 發票期別 格式：YYYMM(民國年+當期別的偶數月份)e.g. 108年01-02月，帶入10802
        /// </summary>
        public string EinvoicePeriod { get; set; }

        /// <summary>
        /// 發票號碼
        /// </summary>
        public string EinvoiceNum { get; set; }

        /// <summary>
        /// 發票隨機碼
        /// </summary>
        public string EinvoiceRandomNumber { get; set; }

        /// <summary>
        /// 消費總金額
        /// </summary>
        public string EinvoiceSaleAmount { get; set; }

        /// <summary>
        /// 賣方名稱
        /// </summary>
        public string EinvoiceStoreName { get; set; }

        /// <summary>
        /// 賣方地址
        /// </summary>
        public string EinvoiceStoreAddress { get; set; }

        /// <summary>
        /// 載具類別 1：手機條碼載具 2：icash Pay載具
        /// </summary>
        public int CarrierType { get; set; }

        /// <summary>
        /// 載具號碼
        /// </summary>
        public string CarrierNumber { get; set; }

        /// <summary>
        /// 明細資料
        /// </summary>
        public List<EinvoiceItemDetail> EinvoiceItemDetail { get; set; }
    }

    /// <summary>
    /// 明細資料Model
    /// </summary>
    public class EinvoiceItemDetail
    {
        /// <summary>
        /// 品名
        /// </summary>
        public string description { get; set; }

        /// <summary>
        /// 單價
        /// </summary>
        public int unitPrice { get; set; }

        /// <summary>
        /// 數量
        /// </summary>
        public int quantity { get; set; }

        /// <summary>
        /// 小計
        /// </summary>
        public int amount { get; set; }
    }
}