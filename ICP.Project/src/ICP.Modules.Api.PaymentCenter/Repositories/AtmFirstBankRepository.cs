using ICP.Infrastructure.Abstractions.DbUtil;
using ICP.Infrastructure.Core.Models;
using ICP.Modules.Api.PaymentCenter.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICP.Modules.Api.PaymentCenter.Repositories
{
    public class AtmFirstBankRepository
    {
        private readonly IDbConnectionPool _dbConnectionPool = null;
        private readonly IDbConnection db = null;

        public AtmFirstBankRepository(IDbConnectionPool dbConnectionPool)
        {
            _dbConnectionPool = dbConnectionPool;
            db = _dbConnectionPool.Create(DatabaseName.ICP_PaymentCenter);
        }

        /// <summary>
        /// 更新 PaymentCenter 訂單資料
        /// </summary>
        /// <param name="atmFirstBankWriteOffData"></param>
        /// <returns></returns>
        public TradeInfoAtm UpdateCenterAtmTrade(AtmFirstBankWriteOffData atmFirstBankWriteOffData)
        {
            string sql = "EXEC ausp_PaymentCenter_ATM_UpdateFirstBankTrade_U";
            var args = new
            {
                atmFirstBankWriteOffData.VirtualAccount,
                atmFirstBankWriteOffData.CompanyAccount,
                atmFirstBankWriteOffData.TransDate,
                atmFirstBankWriteOffData.TransID,
                atmFirstBankWriteOffData.TransNo,
                atmFirstBankWriteOffData.LenderAmt,
                atmFirstBankWriteOffData.DebitAmt,
                atmFirstBankWriteOffData.PNType,
                atmFirstBankWriteOffData.Balance,
                atmFirstBankWriteOffData.TransType,
                atmFirstBankWriteOffData.RouteType
            };
            sql += db.GenerateParameter(args);
            return db.QuerySingleOrDefault<TradeInfoAtm>(sql, args);
        }

        /// <summary>
        /// 新增銷帳錯誤 log
        /// </summary>
        /// <param name="tradeInfo"></param>
        /// <returns></returns>
        public BaseResult AddWriteOffErrorLog(TradeInfo tradeInfo)
        {
            string sql = "EXEC ausp_PaymentCenter_WriteOff_AddErrorLog_I";
            var args = new
            {
                PayType = tradeInfo?.MpType?.ToUpper(),
                PayFrom = tradeInfo?.MpName?.ToUpper(),
                PaymentDate = tradeInfo.PaymentDate.Year == 1 ? null : string.Format("{0:yyyy/MM/dd HH:mm:ss}", tradeInfo.PaymentDate),
                TradeNoCreateDate = tradeInfo.CreateDate.Year == 1 ? null : string.Format("{0:yyyy/MM/dd HH:mm:ss}", tradeInfo.CreateDate),
                ReceiveVirtualAccount = tradeInfo.MpReturnVirtualAccount,
                ReceivePaymentNo = tradeInfo.MpReturnPaymentNo,
                tradeInfo.TradeAMT,
                ReceiveTradeAMT = tradeInfo.MpReturnTradeAMT,
                tradeInfo.TradeNo,
                tradeInfo.PaymentStatus,
                tradeInfo.RtnCode,
                tradeInfo.RtnMsg,
                tradeInfo.ErrorCode,
                tradeInfo.Remark
            };
            sql += db.GenerateParameter(args);
            return db.QuerySingleOrDefault<BaseResult>(sql, args);
        }
    }
}
