namespace ICP.Library.Models.EinvoiceLibrary
{
    public class QueryInvDetailModel
    {
        /// <summary>
        /// 發票開立日期
        /// </summary>        
        public string EinvoiceCreateDate { get; set; }

        /// <summary>
        /// 發票期別
        /// </summary>        
        public string EinvoicePeriod { get; set; }

        /// <summary>
        /// 發票號碼
        /// </summary>        
        public string EinvoiceNum { get; set; }

        /// <summary>
        /// 隨機碼
        /// </summary>        
        public string EinvoiceRandomNumber { get; set; }

        /// <summary>
        /// 發票銷售金額(消費金額)
        /// </summary>
        public string EinvoiceSaleAmount { get; set; }

        /// <summary>
        /// 發票商店名稱(賣方名稱)
        /// </summary>
        public string EinvoiceStoreName { get; set; }

        /// <summary>
        /// 發票明細
        /// </summary>
        public string EinvoiceItemDetail { get; set; }

        /// <summary>
        /// 發票類型：1、載具。2、紙本
        /// </summary>
        public string EinvoiceType { get; set; }

        /// <summary>
        /// 載具隱碼
        /// </summary>
        public string BindCarrierID { get; set; }

        /// <summary>
        /// 載具名稱
        /// </summary>
        public string BindCarrierName { get; set; }

        /// <summary>
        ///  是否捐贈
        /// </summary>
        public bool IsEnvoiceDonated { get; set; }

        /// <summary>
        /// 是否有中獎(0：未中獎。1：中獎。2：未對獎。)
        /// </summary>
        public string EinvoiceAward { get; set; }

        /// <summary>
        /// 賣方地址
        /// </summary>
        public string EinvoiceStoreAddress { get; set; }
    }
}