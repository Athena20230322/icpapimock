using ICP.Infrastructure.Abstractions.Logging;
using ICP.Infrastructure.Core.Extensions;
using ICP.Infrastructure.Core.Models;
using ICP.Modules.Api.Payment.Models.Cashier;
using System.Linq;

namespace ICP.Modules.Api.Payment.Services.TradeMode
{
    public class TransactionService : BaseTradeMode
    {
        public TransactionService(
            ILogger<TransactionService> logger
        ) : base(logger)
        {
            _logger = logger;
        }

        public override BaseResult Validate(CashierReq cashierRequest)
        {
            //### 共用基本參數驗證
            BaseResult result = base.Validate(cashierRequest);

            if (!result.IsSuccess)
            {
                return result;
            }

            _logger.Trace($"交易驗證開始");

            #region 交易驗證 
            //### 驗證開始
            if (cashierRequest.Amount < 0)
            {
                result.SetCode(2005);
            }
            else if (cashierRequest.ItemAmt < 0)
            {
                result.SetCode(2006);
            }
            else if (cashierRequest.UtilityAmt < 0)
            {
                result.SetCode(2007);
            }
            else if (cashierRequest.CommAmt < 0)
            {
                result.SetCode(2008);
            }
            else if (cashierRequest.ExceptAmt1 < 0)
            {
                result.SetCode(2009);
            }
            else if (cashierRequest.ExceptAmt2 < 0)
            {
                result.SetCode(2010);
            }
            else if (!new int[] { 0, 1 }.Contains(cashierRequest.RedeemFlag))
            {
                result.SetCode(2011);
            }
            else if (cashierRequest.BonusAmt < 0)
            {
                result.SetCode(2012);
            }
            else if (cashierRequest.DebitPoint < 0)
            {
                result.SetCode(2013);
            }
            else if (cashierRequest.NonRedeemAmt < 0)
            {
                result.SetCode(2014);
            }
            else if (cashierRequest.NonPointAmt < 0)
            {
                result.SetCode(2015);
            }
            else
            {
                result.SetSuccess();
            }

            //### 驗證結束
            #endregion

            _logger.Trace($"交易驗證結束, 回傳代碼={result.RtnCode}");

            return result;
        }
    }
}