using AutoMapper;
using ICP.Infrastructure.Abstractions.Logging;
using ICP.Infrastructure.Core.Extensions;
using ICP.Infrastructure.Core.Models;
using ICP.Modules.Api.Payment.Services;
using ICP.Modules.Mvc.Payment.Models.QueryTrade;
using ICP.Modules.Mvc.Payment.Repositories;
using System;
using System.Collections.Generic;

namespace ICP.Modules.Mvc.Payment.Services
{
    public class QueryTradeService
    {
        private readonly ILogger _logger = null;
        private readonly QueryTradeRepository _queryTradeRepository = null;
        private readonly AccountLinkService _acLinkService = null;

        public QueryTradeService(
            ILogger<QueryTradeService> logger,
            QueryTradeRepository queryTradeRepository,
            AccountLinkService accountLinkService)
        {
            _logger = logger;
            _queryTradeRepository = queryTradeRepository;
            _acLinkService = accountLinkService;
        }

        /// <summary>
        /// 取得ATM儲值訂單
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public DataResult<TopUpTradeInfo> GetATMTopUpDetail(TradeReq request)
        {
            var result = new DataResult<TopUpTradeInfo>();
            result.SetCode(2092);

            TradeDbReq dbReq = Mapper.Map<TradeDbReq>(request);

            TopUpATMTradeDbRes dbRes = _queryTradeRepository.GetATMTopUpTradeDetail(dbReq);
            TopUpTradeInfo tradeInfo = Mapper.Map<TopUpTradeInfo>(dbRes);

            if (tradeInfo != null)
            {
                tradeInfo.IsExpired = DateTime.Compare(Convert.ToDateTime(tradeInfo.ExpireDate).AddDays(1), DateTime.Now) < 0 ;    // 虛擬帳號是否已過有效期限
                result.SetSuccess(tradeInfo);
            }

            return result;
        }

        /// <summary>
        /// 取得ACLink儲值訂單
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public DataResult<TopUpTradeInfo> GetACLinkTopUpTradeDetail(TradeReq request)
        {
            var result = new DataResult<TopUpTradeInfo>();
            result.SetCode(2092);

            TradeDbReq dbReq = Mapper.Map<TradeDbReq>(request);

            TopUpACLinkTradeDbRes dbRes = _queryTradeRepository.GetACLinkTopUpTradeDetail(dbReq);
            TopUpTradeInfo tradeInfo = Mapper.Map<TopUpTradeInfo>(dbRes);

            if (tradeInfo != null)
            {
                tradeInfo.BankAccountShowName = "末五碼" + tradeInfo.BankAccount.Substring(tradeInfo.BankAccount.Length - 5);
                result.SetSuccess(tradeInfo);
            }

            return result;
        }

        /// <summary>
        /// 取得現金儲值訂單
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public DataResult<TopUpTradeInfo> GetCashTopUpTradeDetail(TradeReq request)
        {
            var result = new DataResult<TopUpTradeInfo>();
            result.SetCode(2092);

            TradeDbReq dbReq = Mapper.Map<TradeDbReq>(request);

            TopUpCashTradeDbRes dbRes = _queryTradeRepository.GetCashTopUpTradeDetail(dbReq);
            TopUpTradeInfo tradeInfo = Mapper.Map<TopUpTradeInfo>(dbRes);

            if (tradeInfo != null)
            {
                result.SetSuccess(tradeInfo);
            }

            return result;
        }

        /// <summary>
        /// 取得付款訂單
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public DataResult<List<PaidTradeInfo>> GetPaidTradeDetail(TradeReq request)
        {
            var result = new DataResult<List<PaidTradeInfo>>();
            result.SetCode(2092);

            TradeDbReq dbReq = Mapper.Map<TradeDbReq>(request);

            var dbRes = _queryTradeRepository.GetPaidTradeDetail(dbReq);

            if (dbRes != null)
            {
                result.SetSuccess(dbRes);
            }

            return result;
        }

        /// <summary>
        /// 取得轉帳訂單
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public DataResult<TransferTradeInfo> GetTransferTradeDetail(TransferTradeReq request)
        {
            var result = new DataResult<TransferTradeInfo>();
            result.SetCode(2092);

            TradeDbReq dbReq = Mapper.Map<TradeDbReq>(request);

            var dbRes = _queryTradeRepository.GetTransferTradeDetail(dbReq);

            if (dbRes != null)
            {
                result.SetSuccess(dbRes);
            }

            return result;
        }

        /// <summary>
        /// 取得提領訂單
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public DataResult<BankTransferTradeInfo> GetBankTransferTradeDetail(TradeReq request)
        {
            var result = new DataResult<BankTransferTradeInfo>();
            result.SetCode(2092);

            TradeDbReq dbReq = Mapper.Map<TradeDbReq>(request);

            var dbRes = _queryTradeRepository.GetBankTransferTradeDetail(dbReq);

            if (dbRes != null)
            {
                result.SetSuccess(dbRes);
            }

            return result;
        }
    }
}