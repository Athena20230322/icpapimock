using ICP.Batch.AccountLink.Models;
using ICP.Infrastructure.Abstractions.DbUtil;
using ICP.Infrastructure.Core.Frameworks.DbUtil;
using System.Collections.Generic;

namespace ICP.Batch.AccountLink.Repositories
{
    public class ACLinkRepository
    {
        protected IDbConnectionPool _dbConnectionPool = null;

        public ACLinkRepository(IDbConnectionPool dbConnectionPool)
        {
            _dbConnectionPool = dbConnectionPool;
        }

        /// <summary>
        /// 取得AccountLink設定
        /// </summary>
        /// <param name="bankCode"></param>
        /// <returns></returns>
        [EnableDbProxy]
        public List<ACLinkSettingDbRes> ListAccountLinkSetting(string bankCode)
        {
            var db = _dbConnectionPool.Create(DatabaseName.ICP_Member);
            string sql = "EXEC [ICP_Member].[dbo].[ausp_Member_ACLink_ListAccountLinkSetting_S]";

            var args = new
            {
                BankCode = bankCode
            };

            sql += db.GenerateParameter(args);
            return db.Query<ACLinkSettingDbRes>(sql, args);
        }
    }
}
