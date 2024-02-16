using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICP.Modules.Mvc.Admin.Repositories
{
    using Infrastructure.Abstractions.DbUtil;
    using Infrastructure.Core.Models;
    using Models;
    using Models.ViewModels;

    public class FunctionCatagoryRepository
    {
        private readonly IDbConnectionPool _dbConnectionPool = null;

        private IDbConnection db;

        public FunctionCatagoryRepository(IDbConnectionPool dbConnectionPool)
        {
            _dbConnectionPool = dbConnectionPool;

            db = _dbConnectionPool.Create(DatabaseName.ICP_Admin);
        }

        public List<FunctionCatagory> ListFunction()
        {
            string sql = "EXEC ausp_Admin_ListFunction_S";
            return db.Query<FunctionCatagory>(sql);
        }
    }
}
