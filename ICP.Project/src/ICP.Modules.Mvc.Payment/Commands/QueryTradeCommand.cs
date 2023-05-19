using ICP.Infrastructure.Abstractions.Logging;
using ICP.Infrastructure.Core.Extensions;
using ICP.Infrastructure.Core.Models;
using ICP.Modules.Mvc.Payment.Models.QueryTrade;
using ICP.Modules.Mvc.Payment.Services;
using System.Collections.Generic;

namespace ICP.Modules.Mvc.Payment.Commands
{
    public class QueryTradeCommand
    {
        QueryTradeService _queryTradeService = null;
        private readonly ILogger _logger = null;

        public QueryTradeCommand(
            QueryTradeService queryTradeService,
            ILogger<QueryTradeCommand> logger)
        {
            _queryTradeService = queryTradeService;
            _logger = logger;
        }

        /// <summary>
        /// 取得ATM儲值訂單明細
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public DataResult<TopUpTradeInfo> ATMTopUpDetail(TradeReq request)
        {
            var result = new DataResult<TopUpTradeInfo>();
            result.SetError();

            // 驗證傳入參數
            if (!request.IsValid())
            {
                string msg = "查無訂單資料";
                result.SetFormatError(msg);
                return result;
            }

            var queryResult = _queryTradeService.GetATMTopUpDetail(request);
            result.RtnData = queryResult.RtnData;
            result.RtnCode = queryResult.RtnCode;
            result.RtnMsg = queryResult.RtnMsg;

            return result;
        }

        /// <summary>
        /// 取得ACLink儲值訂單明細
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public DataResult<TopUpTradeInfo> ACLinkTopUpDetail(TradeReq request)
        {
            var result = new DataResult<TopUpTradeInfo>();
            result.SetError();

            // 驗證傳入參數
            if (!request.IsValid())
            {
                string msg = "查無訂單資料";
                result.SetFormatError(msg);
                return result;
            }

            var queryResult = _queryTradeService.GetACLinkTopUpTradeDetail(request);
            result.RtnData = queryResult.RtnData;
            result.RtnCode = queryResult.RtnCode;
            result.RtnMsg = queryResult.RtnMsg;

            return result;
        }

        /// <summary>
        /// 取得現金儲值訂單明細
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public DataResult<TopUpTradeInfo> CashTopUpDetail(TradeReq request)
        {
            var result = new DataResult<TopUpTradeInfo>();
            result.SetError();

            // 驗證傳入參數
            if (!request.IsValid())
            {
                string msg = "查無訂單資料";
                result.SetFormatError(msg);
                return result;
            }

            var queryResult = _queryTradeService.GetCashTopUpTradeDetail(request);
            result.RtnData = queryResult.RtnData;
            result.RtnCode = queryResult.RtnCode;
            result.RtnMsg = queryResult.RtnMsg;

            return result;
        }

        /// <summary>
        /// 取得付款訂單明細
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public DataResult<List<PaidTradeInfo>> PaidDetail(TradeReq request)
        {
            var result = new DataResult<List<PaidTradeInfo>>();
            result.SetError();

            // 驗證傳入參數
            if (!request.IsValid())
            {
                string msg = "查無訂單資料";
                result.SetFormatError(msg);
                return result;
            }

            var queryResult = _queryTradeService.GetPaidTradeDetail(request);

            result.RtnData = queryResult.RtnData;
            result.RtnCode = queryResult.RtnCode;
            result.RtnMsg = queryResult.RtnMsg;

            return result;
        }

        /// <summary>
        /// 取得轉帳訂單明細
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public DataResult<TransferTradeInfo> TransferDetail(TransferTradeReq request)
        {
            var result = new DataResult<TransferTradeInfo>();
            result.SetError();

            // 驗證傳入參數
            if (!request.IsValid())
            {
                string msg = "查無訂單資料";
                result.SetFormatError(msg);
                return result;
            }

            var queryResult = _queryTradeService.GetTransferTradeDetail(request);

            result.RtnData = queryResult.RtnData;
            result.RtnCode = queryResult.RtnCode;
            result.RtnMsg = queryResult.RtnMsg;

            return result;
        }

        /// <summary>
        /// 取得提領訂單明細
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public DataResult<BankTransferTradeInfo> BankTransferDetail(TradeReq request)
        {
            var result = new DataResult<BankTransferTradeInfo>();
            result.SetError();

            // 驗證傳入參數
            if (!request.IsValid())
            {
                string msg = "查無訂單資料";
                result.SetFormatError(msg);
                return result;
            }

            var queryResult = _queryTradeService.GetBankTransferTradeDetail(request);

            result.RtnData = queryResult.RtnData;
            result.RtnCode = queryResult.RtnCode;
            result.RtnMsg = queryResult.RtnMsg;

            return result;
        }
    }
}
