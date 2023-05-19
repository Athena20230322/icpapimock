namespace ICP.Modules.Api.Member.Models.MemberInfo
{
    /// <summary>
    /// 取得發票明細Request
    /// </summary>
    public class QueryEinvoiceDetailRequest
    {
        /// <summary>
        /// 發票期別 格式：YYYMM(民國年+當期別的偶數月份)e.g. 108年01-02月，帶入10802
        /// </summary>
        public string EinvoicePeriod { get; set; }
        /// <summary>
        /// 發票號碼
        /// </summary>
        public string EinvoiceNum { get; set; }
    }
}