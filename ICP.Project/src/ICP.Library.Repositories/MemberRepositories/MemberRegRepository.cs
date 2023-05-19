using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICP.Library.Repositories.MemberRepositories
{
    using Library.Models.MemberModels;
    using Infrastructure.Core.Models;
    using Infrastructure.Abstractions.DbUtil;
    using Infrastructure.Core.Frameworks.DbUtil;

    public class MemberRegRepository
    {
        private readonly IDbConnectionPool _dbConnectionPool = null;

        public MemberRegRepository(IDbConnectionPool dbConnectionPool)
        {
            _dbConnectionPool = dbConnectionPool;
        }

        public DataResult<long> CheckReferrerCode(string ReferrerCode)
        {
            var db = _dbConnectionPool.Create(DatabaseName.ICP_Member);

            string sql = "EXEC ausp_Member_MemberInfo_CheckReferrerCode_S";

            var args = new
            {
                ReferrerCode
            };
            sql += db.GenerateParameter(args);
            return db.QuerySingleOrDefault<DataResult<long>>(sql, args);
        }

        [EnableDbProxy]
        public virtual DataResult<long> AddTempRegisterData(AddTempRegisterDataModel model)
        {
            var db = _dbConnectionPool.Create(DatabaseName.ICP_Member);

            string sql = "EXEC ausp_Member_MemberInfo_AddMemberTemp_S";

            sql += db.GenerateParameter(model);
            return db.QuerySingleOrDefault<DataResult<long>>(sql, model);
        }

        public BaseResult CheckMemberTemp(long TempMID)
        {
            var db = _dbConnectionPool.Create(DatabaseName.ICP_Member);

            string sql = "EXEC ausp_Member_MemberInfo_CheckMemberTemp_S";

            var args = new
            {
                TempMID
            };

            sql += db.GenerateParameter(args);
            return db.QuerySingleOrDefault<BaseResult>(sql, args);
        }

        [EnableDbProxy]
        public virtual DataResult<long> TempRegisterDataToMember(long TempMID, short MemberType)
        {
            var db = _dbConnectionPool.Create(DatabaseName.ICP_Member);

            string sql = "EXEC ausp_Member_MemberInfo_TempToMember_IU";

            var args = new
            {
                TempMID,
                MemberType
            };

            sql += db.GenerateParameter(args);
            return db.QuerySingleOrDefault<DataResult<long>>(sql, args);
        }
    }
}
