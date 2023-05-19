using ICP.Infrastructure.Abstractions.Logging;
using ICP.Infrastructure.Core.Models;
using ICP.Modules.Api.Payment.Models.Cashier;
using ICP.Modules.Api.Payment.Services.TradeMode;

namespace ICP.Modules.Api.Payment.Services
{
    public class WithdrawalService : BaseTradeMode
    {
        public WithdrawalService(
            ILogger<WithdrawalService> logger
        ) : base(logger)
        {
            _logger = logger;
        }

        public override BaseResult Validate(CashierReq cashierRequest)
        {
            var result = base.Validate(cashierRequest);

            if (!result.IsSuccess)
            {
                return result;
            }

            _logger.Trace($"提領驗證開始");

            //### 驗證開始

            //### 驗證結束

            _logger.Trace($"提領驗證結束,RtnMsg = {result.RtnMsg}");

            return result;
        }
    }
}
