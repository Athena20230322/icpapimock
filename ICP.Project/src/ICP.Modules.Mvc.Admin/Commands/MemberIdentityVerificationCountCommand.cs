using ICP.Infrastructure.Abstractions.Logging;
using ICP.Infrastructure.Core.Extensions;
using ICP.Infrastructure.Core.Models;
using ICP.Modules.Mvc.Admin.Models.MemberIdentityVerificationCount;
using ICP.Modules.Mvc.Admin.Models.ViewModels.MemberIdentityVerificationCount;
using ICP.Modules.Mvc.Admin.Services;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICP.Modules.Mvc.Admin.Commands
{
    public class MemberIdentityVerificationCountCommand
    {
        MemberIdentityVerificationCountService  _memberIdentityVerificationCountService;
        ExportDataService _exportDataService;
        ILogger<IPRecordService> _logger;

        public MemberIdentityVerificationCountCommand(MemberIdentityVerificationCountService  memberIdentityVerificationCountService, ExportDataService exportDataService, ILogger<IPRecordService> logger)
        {
            _memberIdentityVerificationCountService = memberIdentityVerificationCountService;
            _exportDataService = exportDataService;
            _logger = logger;
        }

        public List<QueryResultVM> ListMemberIdentityVerificationCount(QueryVM model)
        {
            List<QueryResultVM> result = new List<QueryResultVM>();
            _logger.Info($"ListMemberIdentityVerificationCount Query: {JsonConvert.SerializeObject(model)}");
            try
            {
                var dbResults = _memberIdentityVerificationCountService.ListMemberIdentityVerificationCount(QueryVMToQueryMemberIdentityVerificationCount(model));
                foreach (var dbresult in dbResults)
                {
                    result.Add(QueryMemberIdentityVerificationCountResultToVM(dbresult));
                }
            }
            catch (Exception ex)
            {
                _logger.Error($"ListMemberIdentityVerificationCount DB Exception: {ex.ToString()}");
            }
            return result;
        }

        QueryMemberIdentityVerificationCount QueryVMToQueryMemberIdentityVerificationCount(QueryVM model)
        {
            QueryMemberIdentityVerificationCount result = new QueryMemberIdentityVerificationCount()
            {
                AuthType = model.AuthType,
                EndDate = model.EndDate,
                StartDate = model.StartDate
            };
            return result;
        }

        QueryResultVM QueryMemberIdentityVerificationCountResultToVM(QueryMemberIdentityVerificationCountResult model)
        {
            QueryResultVM result = new QueryResultVM()
            {
                AuthType = _memberIdentityVerificationCountService.GetAuthTypeString(model.AuthType),
                AuthCount = model.AuthCount,
                CreateDate = model.CreateDate,
                Price = model.Price
            };
            return result;
        }

        public MemoryStream ExportExcel(QueryVM model)
        {
            _logger.Info($"ExportExcel Data: {JsonConvert.SerializeObject(model)}");

            var results = ListMemberIdentityVerificationCount(model);
            //表頭
            string functionName = "會員身份驗證筆數查詢";

            string dateRange = $"查詢日期：{model.StartDate.ToString("yyyy-MM-dd")} ~ {model.EndDate.ToString("yyyy-MM-dd")}";

            string[] header = new string[]
            {
                "項目", "年月", "筆數", "金額"
            };

            Func<QueryResultVM, string[]> arryDataGenerator = t =>
            {
                var values = new string[]
                {
                    t.AuthType,
                    t.CreateDate.ToString("yyyy/MM"),
                    t.AuthCount.ToString("N0"),
                    t.Price.ToString("N0")
                };
                return values;
            };

            return _exportDataService.GetXlsStream(header, results, arryDataGenerator, functionName, dateRange);
        }
    }
}
