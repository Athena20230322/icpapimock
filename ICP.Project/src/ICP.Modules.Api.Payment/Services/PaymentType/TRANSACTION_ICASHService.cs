using ICP.Infrastructure.Abstractions.Logging;
using ICP.Modules.Api.Payment.Repositories;

namespace ICP.Modules.Api.Payment.Services.PaymentType
{
    public class TRANSACTION_ICASHService : BasePaymentType
    {
        public TRANSACTION_ICASHService(
            ILogger<TRANSACTION_ICASHService> logger,
            PaymentTradeRepository paymentTradeRepository
        ) : base(logger, paymentTradeRepository)
        {
            _logger = logger;
            _paymentTradeRepository = paymentTradeRepository;
        }       
    }
}
