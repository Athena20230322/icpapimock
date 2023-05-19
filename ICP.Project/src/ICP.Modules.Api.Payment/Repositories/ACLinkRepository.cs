using ICP.Infrastructure.Abstractions.DbUtil;
using ICP.Infrastructure.Core.Frameworks.DbUtil;
using ICP.Infrastructure.Core.Models;
using ICP.Modules.Api.Payment.Models.ACLink;
using System.Collections.Generic;
using System.Linq;

namespace ICP.Modules.Api.Payment.Repositories
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
        public List<ACLinkInfoDbRes> ListBindACLink(long MID)
        {
            var db = _dbConnectionPool.Create(DatabaseName.ICP_Member);
            string sql = "EXEC [ICP_Member].[dbo].[ausp_Member_ACLink_ListAccountLinkInfo_S]";

            var args = new
            {
                MID
            };

            sql += db.GenerateParameter(args);
            return db.Query<ACLinkInfoDbRes>(sql, args).ToList();
        }

        /// <summary>
        /// 取得會員AccountLink資訊
        /// </summary>
        /// <param name="MID"></param>
        /// <param name="AccountID"></param>
        /// <returns></returns>
        [EnableDbProxy]
        public ACLinkInfoDbRes GetACLinkInfo(long MID, long AccountID)
        {
            var db = _dbConnectionPool.Create(DatabaseName.ICP_Member);
            string sql = "EXEC [ICP_Member].[dbo].[ausp_Member_ACLink_GetAccountLinkInfo_S]";

            var args = new
            {
                MID,
                AccountID
            };

            sql += db.GenerateParameter(args);
            return db.QuerySingleOrDefault<ACLinkInfoDbRes>(sql, args);
        }

        /// <summary>
        /// 設定會員AccountLink預設的帳號
        /// </summary>
        /// <param name="MID"></param>
        /// <param name="BankCode"></param>
        /// <param name="INDTAccount"></param>
        /// <returns></returns>
        [EnableDbProxy]
        public BaseResult SetDefaultACLink(long MID, string BankCode, string INDTAccount)
        {
            var db = _dbConnectionPool.Create(DatabaseName.ICP_Member);
            string sql = "EXEC [ICP_Member].[dbo].[ausp_Member_ACLink_AccountLinkSetDefault_U]";

            var args = new
            {
                MID,
                BankCode,
                INDTAccount
            };

            sql += db.GenerateParameter(args);
            return db.QuerySingleOrDefault<BaseResult>(sql, args);
        }
    }
}
