using ICP.Infrastructure.Abstractions.Logging;
using ICP.Modules.Api.Payment.Repositories;

namespace ICP.Modules.Api.Payment.Services.PaymentType
{
    public class TRANSFER_ICASHService : BasePaymentType
    {
        public TRANSFER_ICASHService(
            ILogger<TRANSFER_ICASHService> logger,
            PaymentTradeRepository paymentTradeRepository
        ) : base(logger, paymentTradeRepository)
        {
            _logger = logger;
            _paymentTradeRepository = paymentTradeRepository;
        }       
    }
}
