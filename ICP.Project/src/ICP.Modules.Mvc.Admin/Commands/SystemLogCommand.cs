using ICP.Infrastructure.Core.Models;
using ICP.Modules.Mvc.Admin.Models.SystemLog.SystemError;
using ICP.Modules.Mvc.Admin.Services;
using System.Collections.Generic;

namespace ICP.Modules.Mvc.Admin.Commands
{
    public class SystemLogCommand
    {
        private readonly SystemLogService _systemLogService = null;        

        public SystemLogCommand(
            SystemLogService systemLogService
        )
        {
            _systemLogService = systemLogService;
        }

        /// <summary>
        /// 後台系統管理 LOG查詢 系統異常記錄查詢
        /// </summary>
        /// <param name="Operator"></param>
        /// <param name="QrySystemErrorRes"></param>
        /// <returns></returns>
        public DataResult<List<QrySystemErrorRes>> ListSystemErrorLogResult(QrySystemErrorReq QrySystemErrorRes)
        {
            return _systemLogService.ListSystemErrorLogResult(QrySystemErrorRes);
        }

        /// <summary>
        /// 後台系統管理 LOG查詢 系統異常記錄查詢 - 錯誤訊息Detail
        /// </summary>
        /// <param name="logID"></param>
        /// <param name="siteType"></param>
        /// <returns></returns>
        public DataResult<SystemErrorDetailRes> GetSystemErrorDetail(long logID, int siteType)
        {
            return _systemLogService.GetSystemErrorDetail(logID, siteType);
        }
    }
}
