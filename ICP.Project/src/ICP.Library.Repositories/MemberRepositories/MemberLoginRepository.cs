using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICP.Library.Repositories.MemberRepositories
{
    using Infrastructure.Core.Frameworks.DbUtil;
    using Infrastructure.Core.Models;
    using Infrastructure.Abstractions.DbUtil;
    using Library.Models.AuthorizationApi;

    public class MemberLoginRepository
    {
        IDbConnectionPool _dbConnectionPool;

        public MemberLoginRepository(IDbConnectionPool dbConnectionPool)
        {
            _dbConnectionPool = dbConnectionPool;
        }

        [EnableDbProxy]
        public DataResult<long> UserCodeLogin(string UserCode, string UserPwd, byte LoginType, long CheckMID, byte MockLogin, string SMSAuthCode, string JsonApp, long RealIP, long ProxyIP)
        {
            var db = _dbConnectionPool.Create(DatabaseName.ICP_Member);

            string sql = "EXEC ausp_Member_MemberSecurity_UserCodeLogin_IU";

            var args = new
            {
                Account = UserCode,
                Pwd = UserPwd,
                LoginType,
                CheckMID,
                MockLogin,
                SMSAuthCode,
                JsonApp,
                RealIP,
                ProxyIP
            };

            sql += db.GenerateParameter(args);
            return db.QuerySingleOrDefault<DataResult<long>>(sql, args);
        }

        public BaseResult CheckChangeDevice(long MID, string DeviceID)
        {
            var db = _dbConnectionPool.Create(DatabaseName.ICP_Logging);

            string sql = "EXEC ausp_Member_MemberSecurity_CheckChangeDevice_S";

            var args = new
            {
                MID,
                DeviceID
            };

            sql += db.GenerateParameter(args);
            return db.QuerySingleOrDefault<DataResult<long>>(sql, args);
        }
    }
}
