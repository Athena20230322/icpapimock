using ICP.Infrastructure.Abstractions.DbUtil;
using ICP.Modules.Api.Payment.Models;
using ICP.Modules.Api.Payment.Models.AccountLimitInfo;
using ICP.Modules.Api.Payment.Models.QueryOnlinecharge;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICP.Modules.Api.Payment.Repositories
{
    public class MemberInfoRepository
    {
        private readonly IDbConnectionPool _dbConnectionPool = null;

        public MemberInfoRepository(IDbConnectionPool dbConnectionPool)
        {
            _dbConnectionPool = dbConnectionPool;
        }

        /// <summary>
        /// 取得會員帳戶資料
        /// </summary>
        /// <param name="mid"></param>
        /// <returns></returns>
        public virtual MemberCoinsModel GetUserCoins(long mid)
        {
            var db = _dbConnectionPool.Create(DatabaseName.ICP_Coins);
            
            string sql = "EXEC [ICP_Coins].[dbo].[ausp_Coins_UserCoin_GetUserCoins_SI]";

            var args = new
            {
                MID = mid
            };

            sql += db.GenerateParameter(args);

            return db.QuerySingleOrDefault<MemberCoinsModel>(sql, args);
        }


        /// <summary>
        /// 取得會員本月各項交易限額資訊
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public AccountLimitInfoDbRes GetAccountLimitInfo(AccountLimitInfoDbReq request)
        {
            var db = _dbConnectionPool.Create(DatabaseName.ICP_Member);

            string sql = "EXEC ausp_Member_Law_GetAccountLimitInfo_S";
            var args = new
            {
                request.MID
            };
            sql += db.GenerateParameter(args);
            return db.QuerySingleOrDefault<AccountLimitInfoDbRes>(sql, args);
        }
    }
}
