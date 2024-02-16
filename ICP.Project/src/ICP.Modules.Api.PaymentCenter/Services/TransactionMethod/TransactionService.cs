using ICP.Infrastructure.Core.Extensions;
using ICP.Infrastructure.Core.Models;
using ICP.Modules.Api.PaymentCenter.Enums;
using ICP.Modules.Api.PaymentCenter.Interface;
using ICP.Modules.Api.PaymentCenter.Models;
using ICP.Modules.Api.PaymentCenter.Models.TransactionMethod;
using ICP.Modules.Api.PaymentCenter.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICP.Modules.Api.PaymentCenter.Services
{
    public class TransactionService : ITransactionMethod
    {
        TransactionRepository _transactionRepository = null;

        public TransactionService(TransactionRepository transactionRepository)
        {
            _transactionRepository = transactionRepository;
        }

        /// <summary>
        /// 驗證
        /// </summary>
        /// <param name="tradeRequestModel"></param>
        /// <returns></returns>
        public BaseResult Validate(object tradeRequestModel)
        {
            var result = new BaseResult().SetSuccess();
            
            var request = tradeRequestModel as TradeReqModel;
            var paymentType = (ePaymentType)request.PaymentTypeID;
            if (paymentType < ePaymentType.TranscationMin ||
                paymentType > ePaymentType.TranscationMax)
            {
                result.SetCode(7022);
            }

            return result;
        }

        /// <summary>
        /// 新增訂單
        /// </summary>
        /// <param name="tradeRequestModel"></param>
        /// <returns></returns>
        public AddTradeResult AddTrade(TradeReqModel tradeRequestModel)
        {
            return _transactionRepository.AddTrade(tradeRequestModel);
        }

        /// <summary>
        /// 更新訂單
        /// </summary>
        /// <param name="tradeResponseModel"></param>
        /// <returns></returns>
        public BaseResult UpdateTrade(DataResult<TradeResModel> tradeResponseModel)
        {
            return _transactionRepository.UpdateTrade(tradeResponseModel);
        }
    }
}
