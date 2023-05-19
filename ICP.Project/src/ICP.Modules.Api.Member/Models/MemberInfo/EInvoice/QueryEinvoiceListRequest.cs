using ICP.Library.Models.AuthorizationApi;

namespace ICP.Modules.Api.Member.Models.MemberInfo
{
    /// <summary>
    /// 取得發票清單
    /// </summary>
    public class QueryEinvoiceListRequest : BaseAuthorizationApiRequest
    {
        /// <summary>
        /// 發票期別 格式：YYYMM(民國年+當期別的偶數月份)，ex:108年01-02月，帶入10802
        /// </summary>
        public string EinvoicePeriod { get; set; }
    }
}