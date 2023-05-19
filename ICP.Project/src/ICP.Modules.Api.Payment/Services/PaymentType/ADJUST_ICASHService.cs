using ICP.Infrastructure.Abstractions.Logging;
using ICP.Modules.Api.Payment.Repositories;

namespace ICP.Modules.Api.Payment.Services.PaymentType
{
    public class ADJUST_ICASHService : BasePaymentType
    {
        public ADJUST_ICASHService(
            ILogger<ADJUST_ICASHService> logger,
            PaymentTradeRepository paymentTradeRepository
        ) : base(logger, paymentTradeRepository)
        {
            _logger = logger;
            _paymentTradeRepository = paymentTradeRepository;
        }       
    }
}
