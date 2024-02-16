using ICP.Infrastructure.Abstractions.Logging;
using ICP.Infrastructure.Core.Models;
using ICP.Modules.Api.Payment.Models.Cashier;

namespace ICP.Modules.Api.Payment.Services.TradeMode
{
    public class TransferService : BaseTradeMode
    {
        public TransferService(
            ILogger<TransferService> logger
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

            base._logger.Trace($"轉帳驗證開始");

            //### 驗證開始

            //### 驗證結束

            base._logger.Trace($"轉帳驗證結束,RtnMsg = {result.RtnMsg}");

            return result;
        }
    }
}
