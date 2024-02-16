using ICP.Infrastructure.Core.Models;

namespace ICP.Library.Models.MemberModels
{
    public class WithdrawBalanceResult : BaseResult
    {
        public long TradeID { get; set; }

        public string TradeNo { get; set; }
    }
}
