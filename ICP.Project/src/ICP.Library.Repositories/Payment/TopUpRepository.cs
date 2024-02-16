using ICP.Infrastructure.Abstractions.DbUtil;
using ICP.Infrastructure.Core.Frameworks.DbUtil;
using ICP.Infrastructure.Core.Models;
using ICP.Library.Models.TopUp;

namespace ICP.Library.Repositories.Payment
{
    public class TopUpRepository
    {
        private readonly IDbConnectionPool _dbConnectionPool = null;

        public TopUpRepository(IDbConnectionPool dbConnectionPool)
        {
            _dbConnectionPool = dbConnectionPool;
        }

        #region 取得會員帳戶相關資訊
        /// <summary>
        /// 取得會員儲值帳戶資訊
        /// </summary>
        /// <param name="MID"></param>
        /// <returns></returns>
        [EnableDbProxy]
        public TopUpLimitModel GetTopUpLimit(long MID)
        {
            var db = _dbConnectionPool.Create(DatabaseName.ICP_Member);
            string sql = "EXEC [ICP_Member].[dbo].[ausp_Member_Law_GetAccountLimitInfo_S]";

            var args = new
            {
                MID
            };

            sql += db.GenerateParameter(args);
            return db.QuerySingleOrDefault<TopUpLimitModel>(sql, args);
        }
        #endregion

        #region 儲值滿額記錄
        /// <summary>
        /// 寫入儲值滿額記錄
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [EnableDbProxy]
        public BaseResult AddTopUpOverLimitRecords(OverLimitDbReq model)
        {
            var db = _dbConnectionPool.Create(DatabaseName.ICP_Payment);
            string sql = "EXEC [ICP_Member].[dbo].[ausp_Payment_Trade_AddTopUpOverLimitRecords_I]";

            var args = new
            {
                model.MID,
                model.TradeID,
                model.PaymentTypeID,
                model.PaymentSubTypeID,
                model.Amount
            };

            sql += db.GenerateParameter(args);
            return db.QuerySingleOrDefault<BaseResult>(sql, args);
        }
        #endregion
    }
}
