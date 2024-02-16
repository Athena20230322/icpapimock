using ICP.Infrastructure.Abstractions.DbUtil;
using ICP.Infrastructure.Core.Models;
using ICP.Modules.Api.PaymentCenter.Models;
using ICP.Modules.Api.PaymentCenter.Models.PaymentMethod.AccountLink;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICP.Modules.Api.PaymentCenter.Repositories.PaymentMethod
{
    public class ATMPaymentRepository
    {
        private readonly IDbConnectionPool _dbConnectionPool = null;
        private IDbConnection db;

        public ATMPaymentRepository(IDbConnectionPool dbConnectionPool)
        {
            _dbConnectionPool = dbConnectionPool;
        }

        /// <summary>
        /// 建立虛擬帳號
        /// </summary>
        /// <param name="tradeRequestModel"></param>
        /// <returns></returns>
        public VirtualAccountModel CreateVirtualAccount(TradeReqModel tradeRequestModel)
        {
            db = _dbConnectionPool.Create(DatabaseName.ICP_PaymentCenter);

            string sql = "EXEC ausp_PaymentCenter_VirtualAccount_AddGate_I";
            var args = new
            {
                tradeRequestModel.PaymentTypeID,
                tradeRequestModel.PaymentSubTypeID,
                tradeRequestModel.Amount,
                tradeRequestModel.TradeID
            };
            sql += db.GenerateParameter(args);
            return db.QuerySingleOrDefault<VirtualAccountModel>(sql, args);
        }

        /// <summary>
        /// 新增 ATM 交易明細
        /// </summary>
        /// <param name="tradeRequestModel"></param>
        /// <returns></returns>
        public BaseResult AddTrade(TradeReqModel tradeRequestModel)
        {
            db = _dbConnectionPool.Create(DatabaseName.ICP_PaymentCenter);

            string sql = "EXEC ausp_PaymentCenter_Trade_ATM_AddTradeDetail_I";
            var args = new
            {
                TradeID = tradeRequestModel.TradeID,
                ActualTradeAmount = tradeRequestModel.Amount,
                PaymentSubTypeID = tradeRequestModel.PaymentSubTypeID,
                ExpireDate = DateTime.Now.AddDays(3)
            };
            sql += db.GenerateParameter(args);
            return db.QuerySingleOrDefault<BaseResult>(sql, args);
        }

        /// <summary>
        /// 更新 ATM 交易明細
        /// </summary>
        /// <param name="tradeRequestModel"></param>
        /// <returns></returns>
        public BaseResult UpdateTrade(TradeResModel tradeRequestModel)
        {
            db = _dbConnectionPool.Create(DatabaseName.ICP_PaymentCenter);

            string sql = "EXEC ausp_PaymentCenter_Trade_ATM_UpdateTradeDetail_U";
            var args = new
            {
                TradeID = tradeRequestModel.PaymentCenterTradeID,
                VirtualAccount = tradeRequestModel.VirtualAccount,
                ExpireDate = tradeRequestModel.ATMExpireDate
            };
            sql += db.GenerateParameter(args);
            return db.QuerySingleOrDefault<BaseResult>(sql, args);
        }

        /// <summary>
        /// 更新通知銀行狀態
        /// </summary>
        /// <param name="virtualAccount"></param>
        /// <param name="notifyBankStatus"></param>
        /// <param name="notifyBankDateTime"></param>
        /// <returns></returns>
        public BaseResult UpdateNotifyBankStatus(string virtualAccount, int notifyBankStatus, DateTime notifyBankDateTime)
        {
            db = _dbConnectionPool.Create(DatabaseName.ICP_PaymentCenter);

            string sql = "EXEC ausp_PaymentCenter_ATM_UpdateNotifyBankStatus_U";
            var args = new
            {
                VirtualAccount = virtualAccount,
                NotifyBankStatus = notifyBankStatus,
                NotifyBankDateTime = notifyBankDateTime
            };
            sql += db.GenerateParameter(args);
            return db.QuerySingleOrDefault<BaseResult>(sql, args);
        }
    }
}
