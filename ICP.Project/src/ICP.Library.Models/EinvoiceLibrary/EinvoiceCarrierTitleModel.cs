namespace ICP.Library.Models.EinvoiceLibrary
{
     /// <summary>
    /// 載具發票表頭資料
    /// </summary>
    public class EinvoiceCarrierTitleModel
    {
        /// <summary>
        /// 載具表頭編號
        /// </summary>
        public string rowNum { get; set; }
        /// <summary>
        /// 載具表頭發票號碼
        /// </summary>
        public string invNum { get; set; }
        /// <summary>
        /// 載具表頭商店名稱
        /// </summary>
        public string sellerName { get; set; }
        /// <summary>
        /// 載具表頭發票狀態
        /// </summary>
        public string invStatus { get; set; }
        /// <summary>
        /// 載具表頭發票是否可捐贈
        /// </summary>
        public string invDonatable { get; set; }
        /// <summary>
        /// 載具表頭卡別
        /// </summary>
        public string cardType { get; set; }
        /// <summary>
        /// 載具表頭卡片(載具)隱碼
        /// </summary>
        public string cardNo { get; set; }
        /// <summary>
        /// 載具表頭總金額
        /// </summary>
        public string amount { get; set; }
        /// <summary>
        /// 載具表頭發票期數。例如：3、4月就是10604
        /// </summary>
        public string invPeriod { get; set; }
        /// <summary>
        /// 載具表頭開立時間
        /// </summary>
        public string invoiceTime { get; set; }
        /// <summary>
        /// 載具表頭商店統編
        /// </summary>
        public string sellerBan { get; set; }
        /// <summary>
        /// 載具表頭商店地址
        /// </summary>
        public string sellerAddress { get; set; }
        /// <summary>
        /// 載具表頭捐贈註記。0：未捐贈。1：已捐贈。
        /// </summary>
        public string donateMark { get; set; }
        /// <summary>
        /// 載具表頭發票開立時間：日
        /// </summary>
        public string invDatedate { get; set; }
        /// <summary>
        /// 載具表頭發票開立時間：星期
        /// </summary>
        public string invDateday { get; set; }
        /// <summary>
        /// 載具表頭發票開立時間：時
        /// </summary>
        public string invDatehours { get; set; }
        /// <summary>
        /// 載具表頭發票開立時間：分
        /// </summary>
        public string invDateminutes { get; set; }
        /// <summary>
        /// 載具表頭發票開立時間：月
        /// </summary>
        public string invDatemonth { get; set; }
        /// <summary>
        /// 載具表頭發票開立時間：秒
        /// </summary>
        public string invDateseconds { get; set; }
        /// <summary>
        /// 載具表頭發票開立時間：時間戳記
        /// </summary>
        public string invDatetime { get; set; }
        /// <summary>
        /// 載具表頭發票開立時間：時區
        /// </summary>
        public string invDatetimezoneOffset { get; set; }
        /// <summary>
        /// 載具表頭發票開立時間：年
        /// </summary>
        public string invDateyear { get; set; }
        /// <summary>
        /// 載具表頭排程批號
        /// </summary>
        public string BatchNo { get; set; }
        /// <summary>
        /// 載具號碼
        /// </summary>
        public string RealCardNo { get; set; }
        /// <summary>
        /// 貨幣名稱
        /// </summary>
        public string Currency { get; set; }
    }
}