using ICP.Infrastructure.Abstractions.Logging;
using ICP.Infrastructure.Core.Extensions;
using ICP.Infrastructure.Core.Models;
using ICP.Modules.Mvc.Admin.Models.SystemLog.SystemError;
using ICP.Modules.Mvc.Admin.Repositories;
using System;
using System.Collections.Generic;

namespace ICP.Modules.Mvc.Admin.Services
{
    public class SystemLogService
    {
        private readonly SystemLogRepository _systemLogRepository = null;
        private readonly ILogger _logger = null;

        public SystemLogService(
            SystemLogRepository systemLogRepository,
            ILogger<PaymentStatisticsService> logger
        )
        {
            _systemLogRepository = systemLogRepository;
            _logger = logger;
        }

        /// <summary>
        /// 後台系統管理 LOG查詢 系統異常記錄查詢
        /// </summary>
        /// <param name="qrySystemErrorReq"></param>
        /// <returns></returns>
        public DataResult<List<QrySystemErrorRes>> ListSystemErrorLogResult(QrySystemErrorReq qrySystemErrorReq)
        {
            DataResult<List<QrySystemErrorRes>> result = new DataResult<List<QrySystemErrorRes>>();

            try
            {               
                result.SetSuccess(_systemLogRepository.ListSystemErrorLogResult(qrySystemErrorReq));
            }
            catch(Exception ex)
            {
                _logger.Error($"ListSystemErrorLogResult Error, Msg= {ex.ToString()}");
                result.SetError();
            }

            return result;
        }

        /// <summary>
        /// 後台系統管理 LOG查詢 系統異常記錄查詢 - 錯誤訊息Detail
        /// </summary>
        /// <param name="logID"></param>
        /// <param name="siteType"></param>
        /// <returns></returns>
        public DataResult<SystemErrorDetailRes> GetSystemErrorDetail(long logID, int siteType)
        {
            DataResult<SystemErrorDetailRes> result = new DataResult<SystemErrorDetailRes>();

            try
            {
                result.SetSuccess(_systemLogRepository.GetSystemErrorDetail(logID, siteType));
            }
            catch (Exception ex)
            {
                _logger.Error($"GetSystemErrorDetail Error, Msg= {ex.ToString()}");
                result.SetError();
            }

            return result;
        }
    }
}
