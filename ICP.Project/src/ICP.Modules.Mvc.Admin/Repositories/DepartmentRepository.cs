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

    public class DepartmentRepository
    {
        private readonly IDbConnectionPool _dbConnectionPool = null;

        private IDbConnection db;

        public DepartmentRepository(IDbConnectionPool dbConnectionPool)
        {
            _dbConnectionPool = dbConnectionPool;

            db = _dbConnectionPool.Create(DatabaseName.ICP_Admin);
        }

        public DataResult<int> AddDepartment(Department model)
        {
            string sql = "EXEC ausp_Admin_AddDepartment_I";
            var args = new
            {
                model.DeptName
            };
            sql += db.GenerateParameter(args);
            return db.QuerySingleOrDefault<DataResult<int>>(sql, args);
        }

        public List<Department> ListDepartment()
        {
            string sql = "EXEC ausp_Admin_ListDepartment_S";

            return db.Query<Department>(sql, null);
        }

        public Department GetDepartment(int DeptID)
        {
            string sql = "EXEC ausp_Admin_GetDepartment_S";
            var args = new
            {
                DeptID
            };
            sql += db.GenerateParameter(args);
            return db.QuerySingleOrDefault<Department>(sql, args);
        }

        public BaseResult UpdateDepartment(int DeptID, Department model)
        {
            string sql = "EXEC ausp_Admin_UpdateDepartment_U";
            var args = new
            {
                DeptID,
                model.DeptName,
                model.Visible
            };
            sql += db.GenerateParameter(args);
            return db.QuerySingleOrDefault<BaseResult>(sql, args);
        }
    }
}
