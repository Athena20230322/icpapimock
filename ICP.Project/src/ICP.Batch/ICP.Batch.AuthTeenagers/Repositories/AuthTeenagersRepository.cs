using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICP.Batch.AuthTeenagers.Repositories
{
    using ICP.Infrastructure.Core.Models;
    using Infrastructure.Abstractions.DbUtil;
    using Infrastructure.Core.Frameworks.DbUtil;
    using Models;

    public class AuthTeenagersRepository
    {
        private readonly IDbConnectionPool _dbConnectionPool = null;

        public AuthTeenagersRepository(IDbConnectionPool dbConnectionPool)
        {
            _dbConnectionPool = dbConnectionPool;
        }

        /// <summary>
        /// 取得已通過法定代理人驗證會員資料
        /// </summary>
        /// <returns></returns>
        [EnableDbProxy]
        public virtual List<AuthTeenagersData> ListAuthTeenagersData()
        {
            var db = _dbConnectionPool.Create(DatabaseName.ICP_Member);

            string sql = "EXEC ausp_Member_MemberAuth_ListAuthTeenagers_S";

            return db.Query<AuthTeenagersData>(sql).ToList();
        }

        /// <summary>
        /// 刪除已到期審查資料
        /// </summary>
        /// <returns></returns>
        [EnableDbProxy]
        public virtual BaseResult DeleteAuthTeenagers(long MID, string Modifier)
        {
            var db = _dbConnectionPool.Create(DatabaseName.ICP_Member);

            string sql = "EXEC ausp_Member_MemberAuth_DeleteAuthTeenagers_IU";

            var args = new
            {
                MID,
                Modifier
            };

            sql += db.GenerateParameter(args);

            return db.QuerySingleOrDefault<BaseResult>(sql, args);
        }

        /// <summary>
        /// 更新未成年會員為已成年會員
        /// </summary>
        /// <returns></returns>
        [EnableDbProxy]
        public virtual BaseResult UpdateMemberType(long MID)
        {
            var db = _dbConnectionPool.Create(DatabaseName.ICP_Member);

            string sql = "EXEC ausp_Member_MemberAuth_UpdateMemberTypeByAuthTeenagers_SU";

            var args = new
            {
                MID
            };

            sql += db.GenerateParameter(args);

            return db.QuerySingleOrDefault<BaseResult>(sql, args);
        }
    }
}
