using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICP.Library.Repositories.MemberApi
{
    using Infrastructure.Core.Models;
    using Infrastructure.Abstractions.DbUtil;
    using ICP.Library.Models.MemberModels;

    public class OPMemberApiRepository
    {
        private readonly IDbConnectionPool _dbConnectionPool = null;

        public OPMemberApiRepository(IDbConnectionPool dbConnectionPool)
        {
            _dbConnectionPool = dbConnectionPool;
        }

        public MemberAppToken GetAppTokenByLoginToken(string LoginTokenID)
        {
            var db = _dbConnectionPool.Create(DatabaseName.ICP_Member);

            string sql = "EXEC ausp_Member_MemberInfo_GetAppTokenByLoginToken_S";

            var args = new
            {
                LoginTokenID
            };

            sql += db.GenerateParameter(args);

            return db.QuerySingleOrDefault<MemberAppToken>(sql, args);
        }
    }
}
