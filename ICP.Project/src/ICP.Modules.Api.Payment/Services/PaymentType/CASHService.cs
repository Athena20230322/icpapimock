using ICP.Infrastructure.Abstractions.Logging;
using ICP.Modules.Api.Payment.Repositories;

namespace ICP.Modules.Api.Payment.Services.PaymentType
{
    public class CASHService : BasePaymentType
    {
        public CASHService(
            ILogger<CASHService> logger,
            PaymentTradeRepository paymentTradeRepository
        ) : base(logger, paymentTradeRepository)
        {
            _logger = logger;
            _paymentTradeRepository = paymentTradeRepository;
        }       
    }
}
