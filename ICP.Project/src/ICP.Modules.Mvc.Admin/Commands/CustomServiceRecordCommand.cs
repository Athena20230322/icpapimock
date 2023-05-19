using System;
using System.Collections.Generic;
using System.IO;
using ICP.Infrastructure.Core.Extensions;
using ICP.Infrastructure.Core.Models;
using ICP.Modules.Mvc.Admin.Models.CustomerServiceRecord;
using ICP.Modules.Mvc.Admin.Models.ViewModels.CustomerServiceRecord;
using ICP.Modules.Mvc.Admin.Services;
using System.Linq;
using System.Web.Mvc;

namespace ICP.Modules.Mvc.Admin.Commands
{
    public class CustomServiceRecordCommand
    {
        CustomServiceRecordService _customServiceRecordService;
        ExportDataService _exportDataService;

        public CustomServiceRecordCommand(CustomServiceRecordService customServiceRecordService, ExportDataService exportDataService)
        {
            _customServiceRecordService = customServiceRecordService;
            _exportDataService = exportDataService;
        }
        /// <summary>
        /// 查詢客服紀錄清單
        /// </summary>
        /// <param name="query">查詢物件</param>
        /// <returns></returns>
        public List<QueryCustomServiceRecordResultVM> ListRecords(QueryCustomServiceRecordVM query, bool IsGetNotesLog = false)
        {
            var dbResults = _customServiceRecordService.ListCustomServiceRecords(QueryRecordVMToQueryRecord(query));
            if (IsGetNotesLog)
            {
                foreach (var dbResult in dbResults)
                {
                    dbResult.Notes = new List<CustomerServiceRecordNote>();
                    dbResult.Notes.AddRange(_customServiceRecordService.ListCustomServiceRecordLogs(dbResult.CustomerServiceID));
                }
            }
            List<QueryCustomServiceRecordResultVM> results = new List<QueryCustomServiceRecordResultVM>();
            foreach(var dbResult in dbResults)
                results.Add(QueryResultToQueryResultVM(dbResult));
            return results;
        }
        /// <summary>
        /// 查詢單筆客服紀錄
        /// </summary>
        /// <param name="CustomerServiceID">PK 紀錄編號流水號</param>
        /// <returns></returns>
        public QueryCustomServiceRecordResultVM GetRecordAndDetail(long CustomerServiceID)
        {
            var dbResult = _customServiceRecordService.GetCustomServiceRecord(CustomerServiceID);
            var dbDetailResult = _customServiceRecordService.ListCustomServiceRecordLogs(CustomerServiceID);

            dbResult.Notes = new List<CustomerServiceRecordNote>();
            dbResult.Notes.AddRange(dbDetailResult);

            QueryCustomServiceRecordResultVM result = QueryResultToQueryResultVM(dbResult);
            return result;
        }
        /// <summary>
        /// 查詢 View物件 轉 內部資料物件
        /// </summary>
        /// <param name="model">查詢View</param>
        /// <returns></returns>
        QueryCustomServiceRecord QueryRecordVMToQueryRecord(QueryCustomServiceRecordVM model)
        {
            QueryCustomServiceRecord dbQuery = new QueryCustomServiceRecord()
            {
                CaseNo = model.CaseNo,
                CellPhone = model.CellPhone,
                Cname = model.CName,
                Email = model.Email,
                EndDate = model.EndDate,
                ICPMID = model.ICPMID,
                PageNo = model.PageNo,
                PageSize = model.PageSize,
                StartDate = model.StartDate,
                Status = model.Status,
                TradeNo = model.TradeNo
            };
            return dbQuery;
        }
        /// <summary>
        /// 查詢結果 內部資料物件 轉 View物件
        /// </summary>
        /// <param name="model">查詢結果內部資料物件</param>
        /// <returns></returns>
        QueryCustomServiceRecordResultVM QueryResultToQueryResultVM(QueryCustomServiceRecordResult model)
        {
            QueryCustomServiceRecordResultVM result = new QueryCustomServiceRecordResultVM()
            {
                CaseNo = model.CaseNo,
                CellPhone = model.CellPhone,
                CName = model.Cname,
                CreateDate = model.CreateDate,
                CreateUser = model.CreateUser,
                CustomerServiceID = model.CustomerServiceID,
                Email = model.Email,
                ICPMID = model.ICPMID,
                TradeNo = model.TradeNo,
                GateWayDescription = model.GateWayDescription,
                Modifier = model.Modifier,
                ModifyDate = model.ModifyDate,
                StatusName = _customServiceRecordService.GetStatusName(model.Status),
                TotalCount = model.TotalCount,
                TypeDescription = model.TypeDescription
            };
            foreach (var note in model.Notes)
                result.Notes.Add(RecordNoteToRecordNoteVM(note));
            return result;
        }
        /// <summary>
        /// 案件內容 內部資料物件轉View
        /// </summary>
        /// <param name="model">案件內容 內部資料物件</param>
        /// <returns></returns>
        CustomerServiceRecordNoteVM RecordNoteToRecordNoteVM(CustomerServiceRecordNote model)
        {
            CustomerServiceRecordNoteVM result = new CustomerServiceRecordNoteVM()
            {
                CreateDate = model.CreateDate,
                Note = model.Note
            };
            return result;
        }
        /// <summary>
        /// 更新客服紀錄
        /// </summary>
        /// <param name="CustomerServiceID"></param>
        /// <param name="Note">此次修改內容</param>
        /// <param name="Modifier">修改者</param>
        /// <param name="RealIP">RealIP</param>
        /// <param name="ProxyIP">ProxyIP</param>
        /// <returns></returns>
        public BaseResult UpdateRecord(long CustomerServiceID, byte Status, string Note, string Modifier, long RealIP, long ProxyIP)
        {
            BaseResult result = new BaseResult();
            result.SetError();
            var updateResult = _customServiceRecordService.UpdateCustomServiceRecord(CustomerServiceID, Status, Note, Modifier, RealIP, ProxyIP);
            //如果失敗直接回傳
            if (!result.IsSuccess)
            {
                result.SetError(updateResult);
                return result;
            }
            result.SetSuccess();
            return result;
        }
        /// <summary>
        /// 新增客服紀錄
        /// </summary>
        /// <param name="model">新增物件</param>
        /// <param name="RealIP">RealIP</param>
        /// <param name="ProxyIP">ProxyIP</param>
        /// <param name="CreateUser">建立者</param>
        /// <returns></returns>
        public BaseResult AddRecord(AddCustomServiceRecordVM model, long RealIP, long ProxyIP, string CreateUser)
        {
            var result = new BaseResult();
            result.SetError();

            AddCustomServiceRecord addModel = new AddCustomServiceRecord()
            {
                CellPhone = model.CellPhone,
                Cname = model.Cname,
                CreateUser = CreateUser,
                Email = model.Email,
                GateWay = (byte)model.GateWay,
                ICPMID = model.ICPMID,
                Note = model.Note,
                Status = (byte)model.Status,
                TradeNo = model.TradeNo,
                Type = (byte)model.Type,
                RealIP = RealIP,
                ProxyIP = ProxyIP
            };
            var addResult = _customServiceRecordService.AddCustomServiceRecord(addModel);
            if (!addResult.IsSuccess)
            {
                result.SetError(addResult);
                return result;
            }
            result.SetSuccess();
            return result;
        }
        /// <summary>
        /// 匯出Excel
        /// </summary>
        /// <param name="query"></param>
        /// <param name="models"></param>
        /// <returns></returns>
        public MemoryStream ExportRecords(QueryCustomServiceRecordVM query)
        {
            var results = ListRecords(query, true);

            //表頭
            string functionName = "客服記錄查詢";

            string dateRange = $"查詢日期：{query.StartDate.ToString("yyyy-MM-dd")} ~ {query.EndDate.ToString("yyyy-MM-dd")}";

            string[] header = new string[]
            {
                "案件編號", "問題類別", "進線管道", "電支帳號", "E-mail", "訂單編號", "記錄建立日期", "最後記錄人", "最後修改時間", "案件進度", "記錄內容"
            };

            Func<QueryCustomServiceRecordResultVM, string[]> arryDataGenerator = t =>
            {
                List<string> noteStrings = new List<string>();
                foreach (var note in t.Notes)
                {
                    noteStrings.Add($"{note.CreateDate.ToString("MM/dd")} {note.Note}");
                }
                var values = new string[]
                {
                    t.CaseNo,
                    t.TypeDescription,
                    t.GateWayDescription,
                    t.ICPMID,
                    t.Email,
                    t.TradeNo,
                    t.CreateDate.ToString("yyyy-MM-dd HH:mm:ss"),
                    string.IsNullOrWhiteSpace(t.Modifier) ? t.CreateUser : t.Modifier,
                    (t.ModifyDate == null) ? t.CreateDate.ToString("yyyy-MM-dd HH:mm:ss"): ((DateTime)t.ModifyDate).ToString("yyyy-MM-dd HH:mm:ss"),
                    t.StatusName,
                    string.Join(";", noteStrings.ToArray())
                };
                return values;
            };

            return _exportDataService.GetXlsStream(header, results, arryDataGenerator, functionName, dateRange);
        }
        /// <summary>
        /// 取得案件進度選單
        /// </summary>
        /// <returns></returns>
        public IEnumerable<SelectListItem> GetStatusOptions()
        {
            var selectList = new List<SelectListItem>();
            var dic = _customServiceRecordService.GetStatusOptions();
            foreach (var data in dic)
                selectList.Add(new SelectListItem() { Text = data.Value, Value = data.Key });
            return selectList;
        }
        /// <summary>
        /// 取得問題類別的清單
        /// </summary>
        /// <returns></returns>
        public IEnumerable<SelectListItem> GetTypeSettingOptions()
        {
            var selectList = new List<SelectListItem>();
            var dic = _customServiceRecordService.GetTypeSettingOptions();
            foreach (var data in dic)
                selectList.Add(new SelectListItem() { Text = data.Value, Value = data.Key });
            return selectList;
        }
        /// <summary>
        /// 取得進線管道的清單
        /// </summary>
        /// <returns></returns>
        public IEnumerable<SelectListItem> GetGateWaySettingOptions()
        {
            var selectList = new List<SelectListItem>();
            var dic = _customServiceRecordService.GetGateWaySettingOptions();
            foreach (var data in dic)
                selectList.Add(new SelectListItem() { Text = data.Value, Value = data.Key });
            return selectList;
        }
    }
}
