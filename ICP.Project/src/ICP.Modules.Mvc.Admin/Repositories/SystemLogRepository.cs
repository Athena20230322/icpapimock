using ICP.Infrastructure.Abstractions.DbUtil;
using ICP.Modules.Mvc.Admin.Models.SystemLog.SystemError;
using System.Collections.Generic;

namespace ICP.Modules.Mvc.Admin.Repositories
{
    public class SystemLogRepository
    {
        private readonly IDbConnectionPool _dbConnectionPool = null;

        private IDbConnection db;

        public SystemLogRepository(IDbConnectionPool dbConnectionPool)
        {
            _dbConnectionPool = dbConnectionPool;
        }

        /// <summary>
        /// 後台系統管理 LOG查詢 系統異常記錄查詢 - 錯誤訊息Detail
        /// </summary>
        /// <param name="logID"></param>
        /// <param name="siteType"></param>
        /// <returns></returns>
        public SystemErrorDetailRes GetSystemErrorDetail(long logID, int siteType)
        {
            db = _dbConnectionPool.Create(DatabaseName.ICP_Logging);
            string sql = "EXEC ICP_Logging.dbo.ausp_Log_Record_GetSystemErrorDetail_S";

            var args = new
            {
                LogID = logID,
                SiteType = siteType               
            };

            sql += db.GenerateParameter(args);

            return db.QuerySingleOrDefault<SystemErrorDetailRes>(sql, args);
        }

        /// <summary>
        /// 後台系統管理 LOG查詢 系統異常記錄查詢
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public List<QrySystemErrorRes> ListSystemErrorLogResult(QrySystemErrorReq req)
        {
            db = _dbConnectionPool.Create(DatabaseName.ICP_Logging);
            string sql = "EXEC ICP_Logging.dbo.ausp_Log_Record_ListSystemError_S";

            var args = new
            {
                StartDate = req.StartDate.ToString("yyyy/MM/dd"),
                EndDate = req.EndDate.AddDays(1).ToString("yyyy/MM/dd"),
                SiteType = req.SiteType,
                ErrorType = req.ErrorType,
                MachineName = req.MachineName,
                PageNo = req.PageNo,
                PageSize = req.PageSize
            };

            sql += db.GenerateParameter(args);

            var result = db.Query<QrySystemErrorRes>(sql, args);

            return result;
        }
    }
}
