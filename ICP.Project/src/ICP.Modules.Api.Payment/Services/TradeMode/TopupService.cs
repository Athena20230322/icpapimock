using ICP.Infrastructure.Abstractions.Logging;
using ICP.Infrastructure.Core.Extensions;
using ICP.Infrastructure.Core.Models;
using ICP.Modules.Api.Payment.Models.Cashier;
using ICP.Modules.Api.Payment.Models.Payment;
using System.Linq;

namespace ICP.Modules.Api.Payment.Services.TradeMode
{
    public class TopupService : BaseTradeMode
    {
        public TopupService(
            ILogger<TopupService> logger
        ) : base(logger)
        {
            _logger = logger;
        }

        public override BaseResult Validate(CashierReq cashierRequest)
        {
            //### 共用基本參數驗證
            var result = base.Validate(cashierRequest);

            if (!result.IsSuccess)
            {
                return result;
            }

            _logger.Trace($"儲值驗證開始");

            #region 儲值參數驗證
            //### 驗證開始
            if (new ePaymentType[] { ePaymentType.CASH, ePaymentType.INVOICE }.Contains((ePaymentType)cashierRequest.PaymentTypeID) && string.IsNullOrWhiteSpace(cashierRequest.Barcode))
            {
                result.SetCode(2016);
                return result;
            }

            if (cashierRequest.Amount < 0)
            {
                result.SetCode(2018);
                return result;
            }
            #endregion

            //### 驗證結束
            _logger.Trace($"儲值驗證結束,RtnMsg = {result.RtnMsg}");

            return result;
        }       
    }
}
