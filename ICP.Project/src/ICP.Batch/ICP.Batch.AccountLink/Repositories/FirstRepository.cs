using ICP.Batch.AccountLink.Models.First;
using ICP.Infrastructure.Abstractions.DbUtil;
using ICP.Infrastructure.Core.Frameworks.DbUtil;
using ICP.Infrastructure.Core.Models;

namespace ICP.Batch.AccountLink.Repositories
{
    public class FirstRepository
    {
        private readonly IDbConnectionPool _dbConnectionPool = null;

        public FirstRepository(IDbConnectionPool dbConnectionPool)
        {
            _dbConnectionPool = dbConnectionPool;
        }

        /// <summary>
        /// 寫入記錄
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [EnableDbProxy]
        public BaseResult AddFirstBatchLog(TradeDbReq model)
        {
            var db = _dbConnectionPool.Create(DatabaseName.ICP_Logging);
            string sql = "EXEC Log_Member_AccountLink_First_Batch";

            sql += db.GenerateParameter(model);
            return db.QuerySingleOrDefault<BaseResult>(sql, model);
        }

    }
}
