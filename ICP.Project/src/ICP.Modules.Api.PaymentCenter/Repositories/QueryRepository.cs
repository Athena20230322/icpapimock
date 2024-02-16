using ICP.Infrastructure.Abstractions.DbUtil;
using ICP.Modules.Api.PaymentCenter.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICP.Modules.Api.PaymentCenter.Repositories
{
    public class QueryRepository
    {
        private readonly IDbConnectionPool _dbConnectionPool = null;
        private readonly IDbConnection db = null;

        public QueryRepository(IDbConnectionPool dbConnectionPool)
        {
            _dbConnectionPool = dbConnectionPool;
            db = _dbConnectionPool.Create(DatabaseName.ICP_PaymentCenter);
        }

        /// <summary>
        /// 取得 ATM 交易資料
        /// </summary>
        /// <param name="merchantID"></param>
        /// <param name="merchantTradeNo"></param>
        /// <returns></returns>
        public TradeInfoAtm GetAtmTradeInfo(long merchantID, string merchantTradeNo)
        {
            string sql = "EXEC ausp_PaymentCenter_ATM_GetTradeInfo_S";
            var args = new
            {
                MerchantID = merchantID,
                MerchantTradeNo = merchantTradeNo
            };
            sql += db.GenerateParameter(args);
            return db.QuerySingleOrDefault<TradeInfoAtm>(sql, args);
        }

        /// <summary>
        /// 以虛擬帳號查詢交易資料
        /// </summary>
        /// <param name="virtualAccount"></param>
        /// <returns></returns>
        public TradeInfo GetTradeInfoByVirtualAccount(string virtualAccount)
        {
            string sql = "EXEC ausp_PaymentCenter_VirtualAccount_GetTradeInfo_S";
            var args = new
            {
                VirtualAccount = virtualAccount
            };
            sql += db.GenerateParameter(args);
            return db.QuerySingleOrDefault<TradeInfo>(sql, args);
        }
    }
}
