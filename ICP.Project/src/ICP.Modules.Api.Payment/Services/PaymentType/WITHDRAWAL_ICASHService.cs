using ICP.Infrastructure.Abstractions.Logging;
using ICP.Modules.Api.Payment.Repositories;

namespace ICP.Modules.Api.Payment.Services.PaymentType
{
    public class WITHDRAWAL_ICASHService : BasePaymentType
    {
        public WITHDRAWAL_ICASHService(
            ILogger<WITHDRAWAL_ICASHService> logger,
            PaymentTradeRepository paymentTradeRepository
        ) : base(logger, paymentTradeRepository)
        {
            _logger = logger;
            _paymentTradeRepository = paymentTradeRepository;
        }       
    }
}
