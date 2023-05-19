using Autofac.Extras.DynamicProxy;
using AutoMapper;
using ICP.Infrastructure.Abstractions.Logging;
using ICP.Infrastructure.Core.Extensions;
using ICP.Infrastructure.Core.Frameworks.AOP;
using ICP.Infrastructure.Core.Models;
using ICP.Modules.Mvc.Payment.Models.BaseMember;
using ICP.Modules.Mvc.Payment.Repositories.BaseMember;
using ICP.Modules.Mvc.Payment.Enums;
using System;
using System.Collections.Generic;

namespace ICP.Modules.Mvc.Payment.Services.BaseMember
{
    /// <summary>
    /// 帳戶紀錄
    /// </summary>
    [Intercept(typeof(LogInterceptor))]
    [Intercept(typeof(ValidatableObjectInterceptor))]
    public class AccountRecordService
    {
        private readonly ILogger _logger = null;

        AccountRecordRepository _accountRecordRepository = null;

        public AccountRecordService(
            ILogger<AccountRecordService> logger,
            AccountRecordRepository accountRecordRepository)
        {
            _logger = logger;
            _accountRecordRepository = accountRecordRepository;
        }

        /// <summary>
        /// 查詢帳戶紀錄列表
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public DataResult<List<AccountRecordDbRes>> ListAccountRecord(AccountRecordReq request)
        {
            DataResult<List<AccountRecordDbRes>> result = new DataResult<List<AccountRecordDbRes>>();

            #region 日期區間

            DateTime nowDate = DateTime.Now;
            DateTime startDate = nowDate;
            DateTime endDate = nowDate;

            switch (request.DateType)
            {
                case (int)DateType.Today:
                default:

                    break;
                case (int)DateType.Week:
                    startDate = nowDate.AddDays(-(int)nowDate.DayOfWeek);
                    endDate = nowDate.AddDays(-(int)nowDate.DayOfWeek + 6);
                    break;
                case (int)DateType.Month:
                    startDate = DateTime.Parse(string.Format("{0:yyyy/MM}/01", nowDate));
                    endDate = nowDate.AddDays(-nowDate.Day).AddMonths(1);
                    break;
                case (int)DateType.Custom:
                    startDate = (DateTime.TryParse(request.StartDate, out startDate)) ? startDate : nowDate.AddMonths(-6);
                    endDate = (DateTime.TryParse(request.EndDate, out endDate)) ? endDate : nowDate;
                    break;
            }

            if (startDate > endDate)
            {
                //自訂日期的起始日不可晚於結束日
                result.RtnCode = 0;//(int)ErrorCode.Error.StartDateLaterEndDate;
                result.RtnMsg = "自訂日期的起始日不可晚於結束日";
            }
            else if ((endDate.Day >= startDate.Day && endDate.Month - startDate.Month > 6)
                    || (endDate.Day < startDate.Day && endDate.Month - startDate.Month >= 6))
            {
                //自訂日期的區間不可超過6個月
                result.RtnCode = 0;//(int)ErrorCode.Error.DateOverRange;
                result.RtnMsg = "自訂日期的區間不可超過6個月";
            }

            #endregion

            AccountRecordDbReq dbReq = Mapper.Map<AccountRecordDbReq>(request);
            dbReq.StartDate = startDate.ToString("yyyy/MM/dd") + " 00:00:00.000";
            dbReq.EndDate = endDate.ToString("yyyy/MM/dd") + " 23:59:59.997";

            List<AccountRecordDbRes> oList = _accountRecordRepository.ListAccountRecord(dbReq);

            // _logger.Trace("ListAccountSummary");

            result.SetSuccess(oList);

            return result;
        }
    }
}