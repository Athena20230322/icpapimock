using ICP.Infrastructure.Abstractions.DbUtil;
using ICP.Modules.Api.Payment.Models;
using System.Collections.Generic;

namespace ICP.Modules.Api.Payment.Repositories
{
    public class MemberPaymentInfoRepository
    {
        private readonly IDbConnectionPool _dbConnectionPool = null;

        public MemberPaymentInfoRepository(IDbConnectionPool dbConnectionPool)
        {
            _dbConnectionPool = dbConnectionPool;
        }

        /// <summary>
        /// 撈取訂單資訊 
        /// </summary>
        /// <param name="paymentParameter"></param>
        /// <returns></returns>
        public virtual List<AccountLinkDbRes> ListAccountLinkInfo(long mid)
        {
            var db = _dbConnectionPool.Create(DatabaseName.ICP_Member);

            string sql = "EXEC [ICP_Member].[dbo].[ausp_Member_ACLink_ListAccountLinkInfo_S]";

            var args = new
            {
                MID = mid
            };

            sql += db.GenerateParameter(args);

            return db.Query<AccountLinkDbRes>(sql, args);
        }
    }
}
