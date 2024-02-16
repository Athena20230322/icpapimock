using ICP.Infrastructure.Abstractions.DbUtil;
using ICP.Modules.Mvc.Payment.Models.BaseMember;
using System.Collections.Generic;

namespace ICP.Modules.Mvc.Payment.Repositories.BaseMember
{
    /// <summary>
    /// 帳戶紀錄
    /// </summary>
    public class AccountRecordRepository
    {
        private readonly IDbConnectionPool _dbConnectionPool = null;
        private readonly IDbConnection db = null;

        public AccountRecordRepository(IDbConnectionPool dbConnectionPool)
        {
            _dbConnectionPool = dbConnectionPool;

            db = _dbConnectionPool.Create(DatabaseName.ICP_Payment);
        }

        /// <summary>
        /// 查詢帳戶紀錄列表
        /// </summary>
        /// <param name="dbReq">查詢條件</param>
        /// <returns></returns>
        public List<AccountRecordDbRes> ListAccountRecord(AccountRecordDbReq dbReq)
        {
            string sql = "EXEC ausp_Payment_Trade_ListTransaction_S";
            var args = new
            {
                dbReq.MID,
                dbReq.AccRecordType,
                dbReq.StartDate,
                dbReq.EndDate,
                dbReq.KeyWords,
                dbReq.RowID
            };

            sql += db.GenerateParameter(args);
            return db.Query<AccountRecordDbRes>(sql, args);
        }
    }
}