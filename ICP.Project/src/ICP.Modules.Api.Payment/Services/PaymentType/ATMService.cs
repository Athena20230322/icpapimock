using ICP.Infrastructure.Abstractions.Logging;
using ICP.Infrastructure.Core.Extensions;
using ICP.Infrastructure.Core.Models;
using ICP.Modules.Api.Payment.Models.Cashier;
using ICP.Modules.Api.Payment.Repositories;
using System;

namespace ICP.Modules.Api.Payment.Services.PaymentType
{
    public class ATMService : BasePaymentType
    {
        public ATMService(
            ILogger<ATMService> logger,
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

            BaseResult updateATMTradeRes = _paymentTradeRepository.UpdatePaymentDetailATM(updateTradeDBReq);

            if (!updateATMTradeRes.IsSuccess)
            {
                result.SetCode(updateATMTradeRes.RtnCode);
            }

            if (updateTradeDBReq.RtnData.ATMExpireDate != "01/01/0001 00:00:00" && DateTime.TryParse(updateTradeDBReq.RtnData.ATMExpireDate, out DateTime expiredate))
            {
                result.ATMExpireDate = expiredate.ToString("yyyy/MM/dd");
            }

            result.VirtualAccount = updateTradeDBReq.RtnData.VirtualAccount;

            _logger.Trace($"更新ATM訂單結束，愛金卡交易編號(TransactionID)={ updateTradeDBReq.TradeNo }，訊息代碼={ result.RtnCode }");

            return result;
        }        
    }
}
