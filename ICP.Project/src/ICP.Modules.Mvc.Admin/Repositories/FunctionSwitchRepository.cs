using ICP.Infrastructure.Abstractions.DbUtil;
using ICP.Infrastructure.Core.Models;
using ICP.Modules.Mvc.Admin.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICP.Modules.Mvc.Admin.Repositories
{
    public class FunctionSwitchRepository
    {
        private readonly IDbConnectionPool _dbConnectionPool = null;

        public FunctionSwitchRepository(IDbConnectionPool dbConnectionPool)
        {
            _dbConnectionPool = dbConnectionPool;
        }

        /// <summary>
        /// 取得內部後台功能開關
        /// </summary>
        /// <param name="FunctionName"></param>
        /// <returns></returns>
        public List<FunctionCatagory> ListFunctionCategory(string FunctionName = null)
        {
            var db = _dbConnectionPool.Create(DatabaseName.ICP_Admin);

            string sql = "Exec ausp_Admin_ListFunctionCategory_S";

            var args = new
            {
                FunctionName
            };

            sql += db.GenerateParameter(args);

            return db.Query<FunctionCatagory>(sql, args).ToList();
        }

        /// <summary>
        /// 更新功能開關
        /// </summary>
        /// <param name="FunctionID"></param>
        /// <param name="Status"></param>
        /// <param name="Modifier"></param>
        /// <returns></returns>
        public BaseResult UpdateFunctionCategoryStatus(int FunctionID, byte Status, string Modifier, long RealIP, long ProxyIP)
        {
            var db = _dbConnectionPool.Create(DatabaseName.ICP_Admin);

            string sql = "Exec ausp_Admin_UpdateFunctionCatagoryStatus_U";

            var args = new
            {
                FunctionID,
                Status,
                Modifier,
                RealIP,
                ProxyIP
            };

            sql += db.GenerateParameter(args);

            return db.QuerySingleOrDefault<BaseResult>(sql, args);
        }

        /// <summary>
        /// 更新 App 功能開關
        /// </summary>
        /// <param name="model"></param>
        /// <param name="Modifier"></param>
        /// <param name="RealIP"></param>
        /// <param name="ProxyIP"></param>
        /// <returns></returns>
        public BaseResult UpdateAppFunctionSwitch(UpdateAppFunctionSwitch model, string Modifier, long RealIP, long ProxyIP)
        {
            var db = _dbConnectionPool.Create(DatabaseName.ICP_Admin);

            string sql = "Exec ausp_Admin_UpdateAPPFunctionSwitch_U";

            var args = new
            {
                model.AppName,
                model.FunctionID,
                model.Status,
                Modifier,
                RealIP,
                ProxyIP
            };

            sql += db.GenerateParameter(args);

            return db.QuerySingleOrDefault<BaseResult>(sql, args);
        }

        /// <summary>
        /// 取得內部後台預約開關列表
        /// </summary>
        /// <param name="FunctionID"></param>
        /// <returns></returns>
        public List<FunctionCategoryStatusRev> ListFunctionCategoryStatusRev(int FunctionID)
        {
            var db = _dbConnectionPool.Create(DatabaseName.ICP_Admin);

            string sql = "Exec ausp_Admin_ListFunctionCatagoryStatusRev_S";

            var args = new
            {
                FunctionID
            };

            sql += db.GenerateParameter(args);

            return db.Query<FunctionCategoryStatusRev>(sql, args).ToList();
        }

        /// <summary>
        /// 取得內部後台預約開關
        /// </summary>
        /// <param name="RevID"></param>
        /// <returns></returns>
        public FunctionCategoryStatusRev GetFunctionCategoryStatusRev(int RevID)
        {
            var db = _dbConnectionPool.Create(DatabaseName.ICP_Admin);

            string sql = "Exec ausp_Admin_GetFunctionCatagoryStatusRev_S";

            var args = new
            {
                RevID
            };

            sql += db.GenerateParameter(args);

            return db.QuerySingleOrDefault<FunctionCategoryStatusRev>(sql, args);
        }

        /// <summary>
        /// 新增內部後台預約開關
        /// </summary>
        /// <param name="model"></param>
        /// <param name="Modifier"></param>
        /// <param name="RealIP"></param>
        /// <param name="ProxyIP"></param>
        /// <returns></returns>
        public BaseResult AddFunctionCategoryStatusRev(FunctionCategoryStatusRev model, string Modifier, long RealIP, long ProxyIP)
        {
            var db = _dbConnectionPool.Create(DatabaseName.ICP_Admin);

            string sql = "Exec ausp_Admin_AddFunctionCatagoryStatusRev_IU";

            var args = new
            {
                model.RevID,
                model.FunctionID,
                model.FunctionStatus,
                model.StartDate,
                model.EndDate,
                Modifier,
                RealIP,
                ProxyIP
            };

            sql += db.GenerateParameter(args);

            return db.QuerySingleOrDefault<BaseResult>(sql, args);
        }

        /// <summary>
        /// 取得權限設定歷程
        /// </summary>
        /// <param name="FunctionID"></param>
        /// <returns></returns>
        public List<FunctionCategoryLog> ListFunctionCategoryLog(int FunctionID)
        {
            var db = _dbConnectionPool.Create(DatabaseName.ICP_Logging);

            string sql = "Exec ausp_Admin_ListFunctionCatagoryLog_S";

            var args = new
            {
                FunctionID
            };

            sql += db.GenerateParameter(args);

            return db.Query<FunctionCategoryLog>(sql, args).ToList();
        }

        /// <summary>
        /// 刪除內部後台預約開關
        /// </summary>
        /// <param name="RevID"></param>
        /// <param name="Modifier"></param>
        /// <param name="RealIP"></param>
        /// <param name="ProxyIP"></param>
        /// <returns></returns>
        public BaseResult DeleteFunctionCategoryStatusRev(int RevID, string Modifier, long RealIP, long ProxyIP)
        {
            var db = _dbConnectionPool.Create(DatabaseName.ICP_Admin);

            string sql = "Exec ausp_Admin_DeleteFunctionCatagoryStatusRev_U";

            var args = new
            {
                RevID,
                Modifier,
                RealIP,
                ProxyIP
            };

            sql += db.GenerateParameter(args);

            return db.QuerySingleOrDefault<BaseResult>(sql, args);
        }

        /// <summary>
        /// 取得APP預約開關列表
        /// </summary>
        /// <param name="AppName"></param>
        /// <param name="FunctionID"></param>
        /// <returns></returns>
        public List<AppFunctionSwitchRev> ListAppFunctionSwitchRev(string AppName, int FunctionID)
        {
            var db = _dbConnectionPool.Create(DatabaseName.ICP_Admin);

            string sql = "Exec ausp_Admin_ListAPPFunctionSwitchRev_S";

            var args = new
            {
                AppName,
                FunctionID
            };

            sql += db.GenerateParameter(args);

            return db.Query<AppFunctionSwitchRev>(sql, args).ToList();
        }

        /// <summary>
        /// 取得App預約開關
        /// </summary>
        /// <param name="RevID"></param>
        /// <returns></returns>
        public AppFunctionSwitchRev GetAppFunctionSwitchRev(int RevID)
        {
            var db = _dbConnectionPool.Create(DatabaseName.ICP_Admin);

            string sql = "Exec ausp_Admin_GetAppFunctionSwitchRev_S";

            var args = new
            {
                RevID
            };

            sql += db.GenerateParameter(args);

            return db.QuerySingleOrDefault<AppFunctionSwitchRev>(sql, args);
        }

        /// <summary>
        /// 新增App預約開關
        /// </summary>
        /// <param name="model"></param>
        /// <param name="Modifier"></param>
        /// <param name="RealIP"></param>
        /// <param name="ProxyIP"></param>
        /// <returns></returns>
        public BaseResult AddAppFunctionSwitchRev(AppFunctionSwitchRev model, string Modifier, long RealIP, long ProxyIP)
        {
            var db = _dbConnectionPool.Create(DatabaseName.ICP_Admin);

            string sql = "Exec ausp_Admin_AddAPPFunctionSwitchRev_IU";

            var args = new
            {
                model.RevID,
                model.AppName,
                model.FunctionID,
                model.FunctionStatus,
                model.StartDate,
                model.EndDate,
                Modifier,
                RealIP,
                ProxyIP
            };

            sql += db.GenerateParameter(args);

            return db.QuerySingleOrDefault<BaseResult>(sql, args);
        }

        /// <summary>
        /// App開關歷程
        /// </summary>
        /// <param name="AppName"></param>
        /// <param name="FunctionID"></param>
        /// <returns></returns>
        public List<AppFunctionSwitchLog> ListAppFunctionSwitchLog(string AppName, int FunctionID)
        {
            var db = _dbConnectionPool.Create(DatabaseName.ICP_Logging);

            string sql = "Exec ausp_Admin_ListAPPFunctionSwitchLog_S";

            var args = new
            {
                AppName,
                FunctionID
            };

            sql += db.GenerateParameter(args);

            return db.Query<AppFunctionSwitchLog>(sql, args).ToList();
        }

        /// <summary>
        /// 刪除App預約開關
        /// </summary>
        /// <param name="RevID"></param>
        /// <param name="Modifier"></param>
        /// <param name="RealIP"></param>
        /// <param name="ProxyIP"></param>
        /// <returns></returns>
        public BaseResult DeleteAppFunctionSwitchRev(int RevID, string Modifier, long RealIP, long ProxyIP)
        {
            var db = _dbConnectionPool.Create(DatabaseName.ICP_Admin);

            string sql = "Exec ausp_Admin_DeleteAPPFunctionSwitchRev_U";

            var args = new
            {
                RevID,
                Modifier,
                RealIP,
                ProxyIP
            };

            sql += db.GenerateParameter(args);

            return db.QuerySingleOrDefault<BaseResult>(sql, args);
        }
    }
}
