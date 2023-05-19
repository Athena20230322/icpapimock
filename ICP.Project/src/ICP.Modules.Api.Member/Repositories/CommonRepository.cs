using ICP.Infrastructure.Abstractions.DbUtil;
using ICP.Infrastructure.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICP.Modules.Api.Member.Repositories
{
    public class CommonRepository
    {
        IDbConnectionPool _dbConnectionPool;

        public CommonRepository(IDbConnectionPool dbConnectionPool)
        {
            _dbConnectionPool = dbConnectionPool;
        }


        /// <summary>
        /// 檢查密碼是否與上次相同
        /// </summary>
        /// <param name="mid"></param>
        /// <returns></returns>
        public BaseResult CheckResetPasswordRule(long mid, string password)
        {
            var db = _dbConnectionPool.Create(DatabaseName.ICP_Member);

            string sql = "ausp_Member_MemberSecurity_CheckNewPasswordLikeOldPassword_S";

            var args = new
            {
                MID = mid,
                Password = password
            };

            return db.QuerySingleOrDefault<BaseResult>(sql, args);
        }
    }
}
