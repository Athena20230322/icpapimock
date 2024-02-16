using ICP.Infrastructure.Abstractions.DbUtil;
using ICP.Infrastructure.Core.Frameworks.DbUtil;
using ICP.Modules.Api.Member.Models.ACLink;
using System.Collections.Generic;
using System.Linq;

namespace ICP.Modules.Api.Member.Repositories
{
    public class ACLinkRepository
    {
        private readonly IDbConnectionPool _dbConnectionPool = null;

        public ACLinkRepository(IDbConnectionPool dbConnectionPool)
        {
            _dbConnectionPool = dbConnectionPool;
        }

        /// <summary>
        /// 取得會員綁定的AccountLink
        /// </summary>
        /// <param name="MID"></param>
        /// <returns></returns>
        [EnableDbProxy]
        public List<ACLinkModel> ListBindACLink(long MID)
        {
            var db = _dbConnectionPool.Create(DatabaseName.ICP_Member);
            string sql = "EXEC [ICP_Member].[dbo].[ausp_Member_ACLink_ListAccountLinkInfo_S]";

            var args = new
            {
                MID
            };

            sql += db.GenerateParameter(args);
            return db.Query<ACLinkModel>(sql, args).ToList();
        }

        /// <summary>
        /// 取得會員連結帳戶資訊
        /// </summary>
        /// <param name="MID"></param>
        /// <param name="AccountID"></param>
        /// <returns></returns>
        [EnableDbProxy]
        public ACLinkModel GetACLinkInfo(long MID, long AccountID)
        {
            var db = _dbConnectionPool.Create(DatabaseName.ICP_Member);
            string sql = "EXEC [ICP_Member].[dbo].[ausp_Member_ACLink_GetAccountLinkInfo_S]";

            var args = new
            {
                MID,
                AccountID
            };

            sql += db.GenerateParameter(args);
            return db.QuerySingleOrDefault<ACLinkModel>(sql, args);
        }

        /// <summary>
        /// 取得銀行快附設定
        /// </summary>
        /// <param name="BankCode"></param>
        /// <returns></returns>
        [EnableDbProxy]
        public ACLinkBankSetting GetACLinkBankSetting(string BankCode)
        {
            var db = _dbConnectionPool.Create(DatabaseName.ICP_Member);
            string sql = "EXEC [ICP_Member].[dbo].[ausp_Member_ACLink_GetBankSetting_S]";

            var args = new
            {
                BankCode
            };

            sql += db.GenerateParameter(args);
            return db.QuerySingleOrDefault<ACLinkBankSetting>(sql, args);
        }
    }
}
