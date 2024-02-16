using ICP.Infrastructure.Abstractions.Logging;
using ICP.Infrastructure.Core.Extensions;
using ICP.Infrastructure.Core.Models;
using ICP.Modules.Api.Payment.Interface;
using ICP.Modules.Api.Payment.Models.Cashier;
using ICP.Modules.Api.Payment.Models.Payment;
using System;
using System.Linq;

namespace ICP.Modules.Api.Payment.Services.TradeMode
{
    public abstract class BaseTradeMode : ITradeMode
    {
        public ILogger _logger = null;       

        public BaseTradeMode(
            ILogger logger
        )
        {
            _logger = logger;
        }
        
        public virtual BaseResult Validate(CashierReq cashierRequest)
        {
            string tradeMode = ((eTradeMode)cashierRequest.TradeModeID).ToString();

            _logger.Trace($"{tradeMode} 交易基本驗證開始");

            BaseResult result = new BaseResult();
            result.SetError();

            //### 廠商編號驗證
            if (cashierRequest.MerchantID <= 0 && (cashierRequest.TradeType == 2 || (cashierRequest.TradeType != 2 && eTradeMode.Topup != (eTradeMode)cashierRequest.TradeModeID)))
            {
                result.SetCode(2001);
            }

            //### 交易編號驗證
            else if (string.IsNullOrWhiteSpace(cashierRequest.MerchantTradeNo))
            {
                result.SetCode(2002);
            }
            //### 交易日期驗證
            else if (!cashierRequest.MerchantTradeDate.HasValue)
            {
                result.SetCode(2004);
            }
            //### 交易類型(0:其他 EC:1 POS:2)驗證
            else if (!new int[] { 0, 1, 2 }.Contains(cashierRequest.TradeType))
            {
                result.SetCode(2020);
            }
            //### 交易模式(交易:1 儲值:2 轉帳:3 提領:4)驗證
            else if (!Enum.IsDefined(typeof(eTradeMode), cashierRequest.TradeModeID))
            {
                result.SetCode(2021);
            }
            else if (!Enum.IsDefined(typeof(ePaymentType), cashierRequest.PaymentTypeID))
            {
                result.SetCode(2037);
            }
            else if (ePaymentType.ACCOUNTLINK == (ePaymentType)cashierRequest.PaymentTypeID && cashierRequest.AccountID <= 0)
            {
                result.SetCode(2059);
            }
            else
            {
                result.SetSuccess();
            }

            //### 交易日期
            _logger.Trace($"{tradeMode} 交易基本驗證結束, 回傳代碼為={ result.RtnCode }");

            return result;
        }
    }
}
