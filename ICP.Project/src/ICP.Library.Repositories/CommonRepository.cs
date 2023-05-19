using ICP.Infrastructure.Abstractions.DbUtil;
using ICP.Library.Models.MemberModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICP.Library.Repositories
{
    public class CommonRepository
    {
        IDbConnectionPool _dbConnectionPool;

        public CommonRepository(IDbConnectionPool dbConnectionPool)
        {
            _dbConnectionPool = dbConnectionPool;
        }

        public CountryTownIDModel GetArea(string ZipCode)
        {
            var db = _dbConnectionPool.Create(DatabaseName.ICP_Member);

            string sql = "EXEC ausp_Member_MemberInfo_GetCountyTownID_S";

            var args = new
            {
                ZipCode
            };

            sql += db.GenerateParameter(args);

            return db.QuerySingleOrDefault<CountryTownIDModel>(sql, args);
        }
    }
}