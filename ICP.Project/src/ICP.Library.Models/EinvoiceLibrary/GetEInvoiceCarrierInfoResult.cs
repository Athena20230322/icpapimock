using System;
using System.Collections.Generic;

namespace ICP.Library.Models.EinvoiceLibrary
{
    /// <summary>
    /// 取得電子發票載具頁面所需資訊列表
    /// </summary>
    public class GetEInvoiceCarrierInfoResult
    {
        public string TimeStamp { get; set; }
        public List<GetEInvoiceCarrierInfoResultType> CarrierList { get; set; }
    }

    /// <summary>
    /// 取得電子發票載具
    /// </summary>
    public class GetEInvoiceCarrierInfoResultType
    {
        public int CarrierType { get; set; }
        public string CarrierNumber { get; set; }
    }
}