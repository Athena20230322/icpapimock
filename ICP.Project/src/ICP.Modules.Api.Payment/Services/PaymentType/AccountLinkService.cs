using ICP.Infrastructure.Abstractions.Logging;
using ICP.Infrastructure.Core.Extensions;
using ICP.Infrastructure.Core.Models;
using ICP.Modules.Api.Payment.Models.Cashier;
using ICP.Modules.Api.Payment.Repositories;

namespace ICP.Modules.Api.Payment.Services.PaymentType
{
    public class ACCOUNTLINKService : BasePaymentType
    {
        public ACCOUNTLINKService(
             ILogger<ACCOUNTLINKService> logger,
             PaymentTradeRepository paymentTradeRepository
         ) : base(logger, paymentTradeRepository)
        {
            _logger = logger;
            _paymentTradeRepository = paymentTradeRepository;
        }

        public override UpdateTradeDBRes UpdateTrade(UpdateTradeDBReq updateTradeDBReq)
        {
            UpdateTradeDBRes result = new UpdateTradeDBRes();
            result.SetError();

            result = base.UpdateTrade(updateTradeDBReq);

            if (!result.IsSuccess || updateTradeDBReq.RtnCode != 1)
            {
                return result;
            }

            BaseResult updateATMTradeRes = _paymentTradeRepository.UpdatePaymentDetailAccountLink(updateTradeDBReq);

            if (!updateATMTradeRes.IsSuccess)
            {
                result.SetCode(updateATMTradeRes.RtnCode);
            }

            _logger.Trace($"更新AccountLink訂單結束，愛金卡交易編號(TransactionID)={ updateTradeDBReq.TradeNo }，訊息代碼={ result.RtnCode }");

            return result;
        }
    }
}
