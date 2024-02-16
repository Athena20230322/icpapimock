using ICP.Infrastructure.Abstractions.DbUtil;
using ICP.Infrastructure.Core.Models;
using ICP.Modules.Mvc.Admin.Models;
using ICP.Modules.Mvc.Admin.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICP.Modules.Mvc.Admin.Repositories
{
    public class SuspenseMainRepository
    {
        private readonly IDbConnectionPool _dbConnectionPool = null;
        
        public SuspenseMainRepository(IDbConnectionPool dbConnectionPool)
        {
            _dbConnectionPool = dbConnectionPool;
        }

        /// <summary>
        /// 新增會員黑名單
        /// </summary>
        /// <param name="model"></param>
        /// <param name="CreateUser"></param>
        /// <param name="RealIP"></param>
        /// <param name="ProxyIP"></param>
        /// <returns></returns>
        public DataResult<long> AddSuspenseMain(SuspenseMain model, string CreateUser, long RealIP, long ProxyIP)
        {
            var db = _dbConnectionPool.Create(DatabaseName.ICP_Member);

            string sql = "EXEC ausp_Admin_MemberInfo_AddSuspenseMain_SI";

            var args = new
            {
                model.CellPhone,
                model.SuspenseType,
                model.ReasonType,
                model.Note,
                model.Reason,
                model.IsBlockIDNO,
                CreateUser,
                RealIP,
                ProxyIP
            };

            sql += db.GenerateParameter(args);

            return db.QuerySingleOrDefault<DataResult<long>>(sql, args);
        }

        /// <summary>
        /// 取得會員黑名單列表
        /// </summary>
        /// <returns></returns>
        public List<SuspenseMainVM> ListSuspenseMain(QuerySuspenseMainVM model)
        {
            var db = _dbConnectionPool.Create(DatabaseName.ICP_Member);

            string sql = "EXEC ausp_Admin_MemberInfo_ListSuspenseMain_S";

            var args = new
            {
                model.StartDate,
                model.EndDate,
                model.CellPhone,
                model.Email,
                model.IDNO,
                model.PageNo,
                model.PageSize
            };

            sql += db.GenerateParameter(args);

            return db.Query<SuspenseMainVM>(sql, args).ToList();
        }

        /// <summary>
        /// 取得懲處方式列表
        /// </summary>
        /// <param name="ID"></param>
        /// <param name="Category"></param>
        /// <returns></returns>
        public List<SuspenseSetting> ListSuspenseSetting()
        {
            var db = _dbConnectionPool.Create(DatabaseName.ICP_Member);

            string sql = "EXEC ausp_Admin_MemberInfo_ListSuspenseSetting_S";

            return db.Query<SuspenseSetting>(sql).ToList();
        }

        /// <summary>
        /// 取得會員黑名單紀錄列表
        /// </summary>
        /// <param name="SuspenseID"></param>
        /// <returns></returns>
        public List<SuspenseMainLogVM> ListSuspenseMainLog(long SuspenseID)
        {
            var db = _dbConnectionPool.Create(DatabaseName.ICP_Logging);

            string sql = "EXEC ausp_Admin_MemberInfo_ListSuspenseMainLog_S";

            var args = new
            {
                SuspenseID
            };

            sql += db.GenerateParameter(args);

            return db.Query<SuspenseMainLogVM>(sql, args).ToList();
        }

        /// <summary>
        /// 審核交易黑名單審核交易黑名單
        /// </summary>
        /// <param name="model"></param>
        /// <param name="Modifier"></param>
        /// <param name="RealIP"></param>
        /// <param name="ProxyIP"></param>
        /// <returns></returns>
        public BaseResult UpdateSuspenseMain(UpdateSuspenseMainVM model, string Modifier, long RealIP, long ProxyIP)
        {
            var db = _dbConnectionPool.Create(DatabaseName.ICP_Member);

            string sql = "EXEC ausp_Admin_MemberInfo_UpdateSuspenseMain_IU";

            var args = new
            {
                model.SuspenseID,
                model.AuthStatus,
                model.Note,
                Modifier,
                RealIP,
                ProxyIP
            };

            sql += db.GenerateParameter(args);

            return db.QuerySingleOrDefault<BaseResult>(sql, args);
        }

        public SuspenseMain GetSuspenseMain(long SuspenseID)
        {
            var db = _dbConnectionPool.Create(DatabaseName.ICP_Member);

            string sql = "EXEC ausp_Admin_MemberInfo_GetSuspenseMain_S";

            var args = new
            {
                SuspenseID
            };

            sql += db.GenerateParameter(args);

            return db.QuerySingleOrDefault<SuspenseMain>(sql, args);
        }

        /// <summary>
        /// 取得有功能權限的使用者信箱
        /// </summary>
        /// <param name="ControllerName"></param>
        /// <param name="MethodName"></param>
        /// <param name="Action"></param>
        /// <returns></returns>
        public List<string> ListUserEmailByFunctionCategory(string ControllerName, string MethodName, int Action)
        {
            var db = _dbConnectionPool.Create(DatabaseName.ICP_Admin);

            string sql = "EXEC ausp_Admin_Privilege_ListUserEmailByFunctionCategory_S";

            var args = new
            {
                ControllerName,
                MethodName,
                Action
            };

            sql += db.GenerateParameter(args);

            return db.Query<string>(sql, args);
        }

        /// <summary>
        /// 解除交易黑名單
        /// </summary>
        /// <param name="SuspenseID"></param>
        /// <param name="Modifier"></param>
        /// <param name="RealIP"></param>
        /// <param name="ProxyIP"></param>
        /// <returns></returns>
        public BaseResult UnlockSuspenseMain(long SuspenseID, string Modifier, long RealIP, long ProxyIP)
        {
            var db = _dbConnectionPool.Create(DatabaseName.ICP_Member);

            string sql = "EXEC ausp_Admin_MemberInfo_UnlockSuspenseMain_SIU";

            var args = new
            {
                SuspenseID,
                Modifier,
                RealIP,
                ProxyIP
            };

            sql += db.GenerateParameter(args);

            return db.QuerySingleOrDefault<BaseResult>(sql, args);
        }
    }
}
