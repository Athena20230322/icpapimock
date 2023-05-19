using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICP.Library.Repositories.AdminRepositories
{
    using ICP.Infrastructure.Abstractions.DbUtil;
    using ICP.Infrastructure.Core.Frameworks.DbUtil;
    using Models.AdminModels;

    public class AdminRepository
    {
        IDbConnectionPool _dbConnectionPool;

        public AdminRepository(IDbConnectionPool dbConnectionPool)
        {
            _dbConnectionPool = dbConnectionPool;
        }

        /// <summary>
        /// 取得App功能開關
        /// </summary>
        /// <param name="AppName"></param>
        /// <param name="FunctionID"></param>
        /// <param name="FunctionName"></param>
        /// <returns></returns>
        [EnableDbProxy]
        public List<AppFunctionSwitch> ListAppFunctionSwitch(string AppName = null, byte? FunctionID = null, string FunctionName = null)
        {
            var db = _dbConnectionPool.Create(DatabaseName.ICP_Admin);

            string sql = "EXEC ausp_Admin_ListAppFunctionSwitch_S";

            var args = new
            {
                AppName,
                FunctionID,
                FunctionName
            };

            sql += db.GenerateParameter(args);
            return db.Query<AppFunctionSwitch>(sql, args).ToList();
        }
    }
}
