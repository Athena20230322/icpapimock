using ICP.Infrastructure.Abstractions.DbUtil;
using ICP.Infrastructure.Core.Models;
using ICP.Modules.Api.PaymentCenter.Models;

namespace ICP.Modules.Api.PaymentCenter.Repositories
{
    public class CoinsRepository
    {
        private readonly IDbConnectionPool _dbConnectionPool = null;
        private readonly IDbConnection db = null;

        public CoinsRepository(IDbConnectionPool dbConnectionPool)
        {
            _dbConnectionPool = dbConnectionPool;

            db = _dbConnectionPool.Create(DatabaseName.ICP_Member);
        }

        /// <summary>
        /// 新增電支帳戶金額(交易或退款)
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public BaseResult AddUserCoins(AddUserCoinsDbReq dto)
        {
            string sql = "EXEC ausp_Coins_UserCoin_AddUserCoins_SI";
            var args = new
            {
                dto.TradeNo,
                dto.MID,
                dto.MerchantID,
                dto.TradeModeID,
                dto.PaymentTypeID,
                dto.PaymentSubTypeID,
                dto.TradeRealCash,
                dto.TradeTopUpCash,
                dto.Currency,
                dto.Notes
            };
            sql += db.GenerateParameter(args);
            return db.QuerySingleOrDefault<BaseResult>(sql, args);
        }

        /// <summary>
        /// 扣款電支帳戶金額(收支金額&儲值金額)
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public BaseResult ReduceUserCoins(ReduceUserCoinsDbReq dto)
        {
            string sql = "EXEC ausp_Coins_UserCoin_ReduceUserCoins_SIU";
            var args = new
            {
                dto.TradeNo,
                dto.MID,
                dto.Account,
                dto.MerchantID,
                dto.TradeModeID,
                dto.PaymentTypeID,
                dto.PaymentSubTypeID,
                dto.ReduceCash,
                dto.Currency,
                dto.Notes
            };
            sql += db.GenerateParameter(args);
            return db.QuerySingleOrDefault<BaseResult>(sql, args);
        }
    }
}
