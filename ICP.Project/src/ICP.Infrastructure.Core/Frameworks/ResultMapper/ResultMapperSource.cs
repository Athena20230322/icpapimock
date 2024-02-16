using ICP.Infrastructure.Abstractions.DbUtil;
using ICP.Infrastructure.Abstractions.ResultMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICP.Infrastructure.Core.Frameworks.ResultMapper
{
    public class ResultMapperSource : IResultMapperSource
    {
        private readonly IDbConnectionPool _dbConnectionPool = null;

        public ResultMapperSource(IDbConnectionPool dbConnectionPool)
        {
            _dbConnectionPool = dbConnectionPool;
        }

        public void Dispose()
        {
            _dbConnectionPool.Dispose();
        }

        public IDictionary<int, string> GetResults()
        {
            var db = _dbConnectionPool.Create(DatabaseName.ICP_Share);

            string sql = "EXEC [ICP_Share].[dbo].[ausp_ResultDictionary_ListResult_S]";

            return db.Query<KeyValuePair<int, string>>(sql).ToDictionary(x => x.Key, x => x.Value);
        }
    }
}
