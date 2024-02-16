using ICP.Infrastructure.Abstractions.Logging;
using ICP.Infrastructure.Core.Extensions;
using ICP.Infrastructure.Core.Models;
using ICP.Library.Repositories.MemberRepositories;
using ICP.Modules.Mvc.Admin.Models.IPRecord;
using ICP.Modules.Mvc.Admin.Models.MemberIdentityVerificationCount;
using ICP.Modules.Mvc.Admin.Repositories;
using Newtonsoft.Json;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;

namespace ICP.Modules.Mvc.Admin.Services
{
    public class MemberIdentityVerificationCountService
    {
        MemberIdentityVerificationCountRepository _memberIdentityVerificationCountRepository;

        public MemberIdentityVerificationCountService(MemberIdentityVerificationCountRepository memberIdentityVerificationCountRepository)
        {
            _memberIdentityVerificationCountRepository = memberIdentityVerificationCountRepository;
        }
        /// <summary>
        /// 聯徵費用查詢
        /// </summary>
        /// <param name="queryMemberIdentityVerificationCount">查詢物件</param>
        /// <returns></returns>
        public List<QueryMemberIdentityVerificationCountResult> ListMemberIdentityVerificationCount(QueryMemberIdentityVerificationCount queryMemberIdentityVerificationCount)
        {
            var logs = _memberIdentityVerificationCountRepository.ListMemberP11P33Log(queryMemberIdentityVerificationCount);
            var chargeFees = _memberIdentityVerificationCountRepository.ListJCICChargeFeeSetting(queryMemberIdentityVerificationCount.StartDate, queryMemberIdentityVerificationCount.EndDate);

            var resultsPutInFee = ChargeFeePutModelByTime(logs, chargeFees);
            List<QueryMemberIdentityVerificationCountResult> results = new List<QueryMemberIdentityVerificationCountResult>();
            var p11s = resultsPutInFee.FindAll(i => i.AuthType == 0);
            results.AddRange(ChargeFeeSettingGroupByMonth(p11s));
            var p33s = resultsPutInFee.FindAll(i => i.AuthType == 1);
            results.AddRange(ChargeFeeSettingGroupByMonth(p33s));
            return results;
        }
        /// <summary>
        /// 將聯徵費用放入計算小計By日期
        /// </summary>
        /// <param name="Logs"></param>
        /// <param name="authChargeFees"></param>
        /// <returns></returns>
        List<QueryMemberIdentityVerificationCountResult> ChargeFeePutModelByTime(List<QueryMemberIdentityVerificationCountResult> Logs, IEnumerable<QueryJCICChargeFeeSettingResult> authChargeFees)
        {
            List<QueryMemberIdentityVerificationCountResult> result = new List<QueryMemberIdentityVerificationCountResult>();
            foreach (var log in Logs)
            {
                QueryMemberIdentityVerificationCountResult resultPutInFee = new QueryMemberIdentityVerificationCountResult();
                var charge = authChargeFees.OrderByDescending(i => i.YYYYMMDD).FirstOrDefault(i => i.YYYYMMDD <= log.CreateDate);
                if (charge != null)
                {
                    resultPutInFee.Price = charge.ChargeFee * log.AuthCount;
                    resultPutInFee.AuthType = log.AuthType;
                    resultPutInFee.CreateDate = log.CreateDate;
                    resultPutInFee.AuthCount = log.AuthCount;
                    result.Add(resultPutInFee);
                }
            }
            return result;
        }
        /// <summary>
        /// 依照年月分組計算小計
        /// </summary>
        /// <param name="resultsPutInFee"></param>
        /// <returns></returns>
        List<QueryMemberIdentityVerificationCountResult> ChargeFeeSettingGroupByMonth(List<QueryMemberIdentityVerificationCountResult> resultsPutInFee)
        {
            List<QueryMemberIdentityVerificationCountResult> result = new List<QueryMemberIdentityVerificationCountResult>();
            //用年月分組
            var resultsByMonth = resultsPutInFee.GroupBy(i => new DateTime(i.CreateDate.Year, i.CreateDate.Month, 1));
            foreach (var tempDataByMonth in resultsByMonth)
            {
                QueryMemberIdentityVerificationCountResult data = new QueryMemberIdentityVerificationCountResult();
                data.AuthType = resultsPutInFee.First().AuthType;
                data.CreateDate = tempDataByMonth.Key;
                data.AuthCount = tempDataByMonth.Count();
                foreach (var tempData in tempDataByMonth)
                {
                    data.Price += tempData.Price;
                }
                result.Add(data);
            }
            return result.OrderBy(i=>i.CreateDate).ToList();
        }
        /// <summary>
        /// 取得AuthType的類別字串
        /// </summary>
        /// <param name="Type">類別</param>
        /// <returns></returns>
        public string GetAuthTypeString(int Type)
        {
            switch (Type)
            {
                case 0:
                    return "P11";
                case 1:
                    return "P33";
                default:
                    return "";
            }
        }
    }
}