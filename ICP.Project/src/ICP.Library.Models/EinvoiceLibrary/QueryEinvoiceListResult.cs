using System;
using System.Collections.Generic;

namespace ICP.Library.Models.EinvoiceLibrary
{
    /// <summary>
    /// 取得發票清單Result
    /// </summary>
    public class QueryEinvoiceListResult
    {
        public string TimeStamp { get; set; }
        /// <summary>
        /// 發票總張數
        /// </summary>
        public int EinvoiceTotalCount { get; set; }
        /// <summary>
        /// 發票總金額
        /// </summary>
        public int EinvoiceTotalAmount { get; set; }
        /// <summary>
        /// 發票資料清單
        /// </summary>
        public List<EinvoiceItemList> EinvoiceData { get; set; }
    }

    /// <summary>
    /// 發票資料清單List
    /// </summary>
    public class EinvoiceItemList
    {
        /// <summary>
        /// 發票開立日期
        /// </summary>
        public string EinvoiceCreateDate { get; set; }
        /// <summary>
        /// 發票號碼
        /// </summary>
        public string EinvoiceNum { get; set; }
        /// <summary>
        /// 賣方名稱
        /// </summary>
        public string EinvoiceStoreName { get; set; }
        /// <summary>
        /// 賣方地址
        /// </summary>
        public string EinvoiceStoreAddress { get; set; }
        /// <summary>
        /// 消費金額
        /// </summary>
        public string EinvoiceSaleAmount { get; set; }


        /// <summary>
        /// 載具類別 1：手機條碼載具 2：icash Pay載具
        /// </summary>
        public int CarrierType { get; set; }
    }
}