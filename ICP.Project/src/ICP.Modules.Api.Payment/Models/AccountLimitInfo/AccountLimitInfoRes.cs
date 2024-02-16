using ICP.Infrastructure.Core.Models;
using ICP.Library.Models.AuthorizationApi;
using System.ComponentModel.DataAnnotations;

namespace ICP.Modules.Api.Payment.Models.AccountLimitInfo
{
    /// <summary>
    /// 帳戶資訊>交易限額 回應
    /// </summary>
    public class AccountLimitInfoRes : BaseAuthorizationApiResult
    {
        /// <summary>
        /// 帳戶餘額
        /// </summary>
        public int TotalCoins { get; set; }

        /// <summary>
        /// 會員類別 (01：一類會員 02：二類會員)
        /// </summary>
        public string MemberClass { get; set; }

        /// <summary>
        /// 付款+轉出-本月總額度
        /// </summary>
        public int MonthPayLimit { get; set; }

        /// <summary>
        /// 付款+轉出-本月累計金額
        /// </summary>
        public int MonthPayAmt { get; set; }

        /// <summary>
        /// 付款+轉出-本月剩餘可用額度
        /// </summary>
        public int MonthAvailablePayAmt { get; set; }

        /// <summary>
        /// 付款+轉出-本月已使用額度百分比(四捨五入至整數)
        /// </summary>
        public int MonthPayPercent { get; set; }

        /// <summary>
        /// 轉出-本日總額度
        /// </summary>
        public int DayTransLimit { get; set; }

        /// <summary>
        /// 轉出-本日累計金額
        /// </summary>
        public int DayTransAmt { get; set; }

        /// <summary>
        /// 轉出-本日剩餘可用額度
        /// </summary>
        public int DayAvailableTransAmt { get; set; }

        /// <summary>
        /// 轉出-本日已使用額度百分比(四捨五入至整數)
        /// </summary>
        public int DayTransPercent { get; set; }

        /// <summary>
        /// 轉出-本月總額度
        /// </summary>
        public int MonthTransLimit { get; set; }

        /// <summary>
        /// 轉出-本月累計金額
        /// </summary>
        public int MonthTransAmt { get; set; }

        /// <summary>
        /// 轉出-本月剩餘可用額度
        /// </summary>
        public int MonthAvailableTransAmt { get; set; }

        /// <summary>
        /// 轉出-本月已使用額度百分比(四捨五入至整數)
        /// </summary>
        public int MonthTransPercent { get; set; }

        /// <summary>
        /// 轉入-本月總額度
        /// </summary>
        public int MonthReceivedLimit { get; set; }

        /// <summary>
        /// 轉入-本月累計金額
        /// </summary>
        public int MonthReceivedAmt { get; set; }

        /// <summary>
        /// 轉入-本月剩餘可用額度
        /// </summary>
        public int MonthAvailableReceivedAmt { get; set; }

        /// <summary>
        /// 轉入-本月已使用額度百分比 (四捨五入至整數)
        /// </summary>
        public int MonthReceivedPercent { get; set; }

        /// <summary>
        /// 儲值-總額度
        /// </summary>
        public int TopUpLimit { get; set; }

        /// <summary>
        /// 儲值-目前儲值餘額
        /// </summary>
        public int TopUpAmt { get; set; }

        /// <summary>
        /// 儲值-佔用中的儲值額度
        /// </summary>
        public int TopUpUsedLimit { get; set; }

        /// <summary>
        /// 儲值-剩餘儲值額度
        /// </summary>
        public int AvailableTopUp { get; set; }

        /// <summary>
        /// 儲值-目前已使用額度百分比 (四捨五入至整數)
        /// </summary>
        public int TopUpPercent { get; set; }

        /// <summary>
        /// 爭議保留款
        /// </summary>
        public int DisputeFreezeCash { get; set; }
    }
}
