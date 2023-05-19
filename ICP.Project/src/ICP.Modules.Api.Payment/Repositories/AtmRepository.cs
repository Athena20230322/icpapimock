using ICP.Infrastructure.Abstractions.DbUtil;
using ICP.Infrastructure.Core.Models;
using ICP.Modules.Api.Payment.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICP.Modules.Api.Payment.Repositories
{
    public class AtmRepository
    {
        private readonly IDbConnectionPool _dbConnectionPool = null;
        private readonly IDbConnection db = null;

        public AtmRepository(IDbConnectionPool dbConnectionPool)
        {
            _dbConnectionPool = dbConnectionPool;
            db = _dbConnectionPool.Create(DatabaseName.ICP_Payment);
        }

        /// <summary>
        /// 更新 ATM 在 Payment 的訂單狀態
        /// </summary>
        /// <param name="tradeInfoAtm"></param>
        /// <returns></returns>
        public BaseResult UpdateAtmPaymentTrade(TradeInfoAtm tradeInfoAtm)
        {
            string sql = "EXEC ausp_Payment_Trade_UpdatePayATMTrade_U";
            var args = new
            {
                tradeInfoAtm.MerchantID,
                tradeInfoAtm.MerchantTradeNo,
                tradeInfoAtm.RtnCode,
                tradeInfoAtm.RtnMsg,
                tradeInfoAtm.TradeNo,
                PayDate = tradeInfoAtm.PaymentDate,
                //TradeAmount = tradeInfoAtm.TradeAMT,
                AccBank = tradeInfoAtm.RtnBankCode,
                AccNo = tradeInfoAtm.RtnBankAcc,
                tradeInfoAtm.PayAmount
            };
            sql += db.GenerateParameter(args);
            return db.QuerySingleOrDefault<BaseResult>(sql, args);
        }

        /// <summary>
        /// 更新銀行通知狀態
        /// </summary>
        /// <param name="notifyBankModel"></param>
        /// <returns></returns>
        public BaseResult UpdateNotifyBankStatus(NotifyBankModel notifyBankModel)
        {
            string sql = "EXEC ausp_Payment_ATM_UpdateNotifyBankStatus_U";
            var args = new
            {
                notifyBankModel.VirtualAccount,
                notifyBankModel.NotifyBankStatus,
                notifyBankModel.NotifyBankDateTime
            };
            sql += db.GenerateParameter(args);
            return db.QuerySingleOrDefault<BaseResult>(sql, args);
        }
    }
}
