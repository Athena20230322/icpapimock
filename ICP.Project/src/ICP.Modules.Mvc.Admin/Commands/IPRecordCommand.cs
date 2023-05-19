using System;
using System.Collections.Generic;
using System.IO;
using ICP.Modules.Mvc.Admin.Models.IPRecord;
using ICP.Modules.Mvc.Admin.Models.ViewModels;
using ICP.Modules.Mvc.Admin.Services;

namespace ICP.Modules.Mvc.Admin.Commands
{
    public class IPRecordCommand
    {
        IPRecordService _ipRecordService;
        ExportDataService _exportDataService;

        public IPRecordCommand(IPRecordService ipRecordService, ExportDataService exportDataService)
        {
            _ipRecordService = ipRecordService;
            _exportDataService = exportDataService;
        }
        /// <summary>
        /// IP紀錄查詢
        /// </summary>
        /// <param name="model">查詢物件</param>
        /// <returns></returns>
        public List<QueryIPRecordResultVM> ListLoginRecord(QueryIPRecordVM query)
        {
            List<QueryIPRecordResultVM> results = new List<QueryIPRecordResultVM>();
            var dbResult = _ipRecordService.ListLoginRecord(QueryIPRecordVMToQueryIPRecord(query));
            foreach (var data in dbResult)
                results.Add(QueryIPRecordResultToQueryIPRecordResultVM(data));
            return results;
        }
        /// <summary>
        /// 匯出Excel
        /// </summary>
        /// <param name="query">查詢的物件</param>
        /// <returns></returns>
        public MemoryStream ExportIPRecord(QueryIPRecordVM query)
        {
            var results = ListLoginRecord(query);
            //表頭
            string functionName = "IP查詢記錄";

            string dateRange = $"查詢日期：{query.StartDate.ToString("yyyy-MM-dd")} ~ {query.EndDate.ToString("yyyy-MM-dd")}";

            string[] header = new string[]
            {
                "電支帳號", "登入帳號", "手機號碼", "登入IP", "回應訊息", "登入時間", "裝置ID"
            };

            Func<QueryIPRecordResultVM, string[]> arryDataGenerator = t =>
            {
                var values = new string[]
                {
                    t.ICPMID,
                    t.Account,
                    t.CellPhone,
                    t.RealIP,
                    t.RtnMsg,
                    t.LogTime?.ToString("yyyy/MM/dd HH:mm"),
                    t.DeviceID
                };
                return values;
            };

            return _exportDataService.GetXlsStream(header, results, arryDataGenerator, functionName, dateRange);
        }

        QueryIPRecord QueryIPRecordVMToQueryIPRecord(QueryIPRecordVM model)
        {
            QueryIPRecord result = new QueryIPRecord()
            {
                Account = model.Account,
                CellPhone = model.CellPhone,
                DeviceID = model.DeviceID,
                EndDate = model.EndDate,
                ICPMID = model.ICPMID,
                PageNo = model.PageNo,
                PageSize = model.PageSize,
                StartDate = model.StartDate,
                UserIP = _ipRecordService.IPStringToLong(model.UserIP)
            };
            return result;
        }

        QueryIPRecordResultVM QueryIPRecordResultToQueryIPRecordResultVM(QueryIPRecordResult model)
        {
            QueryIPRecordResultVM result = new QueryIPRecordResultVM()
            {
                Account = model.Account,
                CellPhone = model.CellPhone,
                DeviceID = model.DeviceID,
                ICPMID = model.ICPMID,
                LogTime = model.LogTime,
                RealIP = (model.RealIP == null) ? string.Empty : _ipRecordService.IPLongToString((long)model.RealIP),
                RtnMsg = model.RtnMsg,
                TotalCount = model.TotalCount
            };
            return result;
        }
    }
}
