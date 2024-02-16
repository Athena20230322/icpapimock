using ICP.Infrastructure.Abstractions.Logging;
using ICP.Infrastructure.Core.Models;
using ICP.Modules.Mvc.Admin.Models.CustomerServiceRecord;
using ICP.Modules.Mvc.Admin.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ICP.Modules.Mvc.Admin.Services
{
    public class CustomServiceRecordService
    {
        CustomServiceRecordRepository _customServiceRecordRepository;
        ILogger<CustomServiceRecordService> _logger;

        public CustomServiceRecordService(CustomServiceRecordRepository customServiceRecordRepository,ILogger<CustomServiceRecordService> logger)
        {
            _customServiceRecordRepository = customServiceRecordRepository;
            _logger = logger;
        }

        #region 查詢
        /// <summary>
        /// 查詢客服紀錄清單(未包含明細)
        /// </summary>
        /// <param name="query">查詢物件</param>
        /// <returns></returns>
        public List<QueryCustomServiceRecordResult> ListCustomServiceRecords(QueryCustomServiceRecord query)
        {
            return _customServiceRecordRepository.ListCustomServiceRecords(query);
        }
        /// <summary>
        /// 查詢某一筆的客服紀錄
        /// </summary>
        /// <param name="CustomerServiceID">PK 紀錄編號流水號</param>
        /// <returns></returns>
        public QueryCustomServiceRecordResult GetCustomServiceRecord(long CustomerServiceID)
        {
            return _customServiceRecordRepository.GetCustomServiceRecord(CustomerServiceID);
        }
        /// <summary>
        /// 查詢某案件的內容
        /// </summary>
        /// <param name="CustomerServiceID">PK 案件流水號</param>
        /// <returns></returns>
        public List<CustomerServiceRecordNote> ListCustomServiceRecordLogs(long CustomerServiceID)
        {
            return _customServiceRecordRepository.ListCustomServiceRecordLogs(CustomerServiceID);
        }
        /// <summary>
        /// 取得案件進度選單
        /// </summary>
        /// <returns></returns>
        public Dictionary<string, string> GetStatusOptions()
        {
            Dictionary<string, string> options = new Dictionary<string, string>();
            byte tempValue = 0;
            options.Add(tempValue.ToString(), GetStatusName(tempValue));
            tempValue = 1;
            options.Add(tempValue.ToString(), GetStatusName(tempValue));
            tempValue = 2;
            options.Add(tempValue.ToString(), GetStatusName(tempValue));
            return options;
        }
        /// <summary>
        /// 取得案件進度的中文名稱
        /// </summary>
        /// <param name="Status">案件進度ID</param>
        /// <returns></returns>
        public string GetStatusName(byte Status)
        {
            switch (Status)
            {
                case 1:
                    return "客服處理";
                case 2:
                    return "客服更改處理結果";
                default:
                    return "建立案件";
            }
        }
        /// <summary>
        /// 取得進線管道的清單
        /// </summary>
        /// <returns></returns>
        public Dictionary<string, string> GetGateWaySettingOptions()
        {
            Dictionary<string, string> options = new Dictionary<string, string>();
            var results = _customServiceRecordRepository.ListSettingOptions(2);
            foreach (var result in results)
            {
                options.Add(result.ID.ToString(), result.Description);
            }
            return options;
        }
        /// <summary>
        /// 取得問題類別的清單
        /// </summary>
        /// <returns></returns>
        public Dictionary<string, string> GetTypeSettingOptions()
        {
            Dictionary<string, string> options = new Dictionary<string, string>();
            var results = _customServiceRecordRepository.ListSettingOptions(1);
            foreach (var result in results)
            {
                options.Add(result.ID.ToString(), result.Description);
            }
            return options;
        }
        #endregion

        /// <summary>
        /// 更新客服紀錄內容
        /// </summary>
        /// <param name="CustomerServiceID">PK 紀錄編號流水號</param>
        /// <param name="Status">案件進度 : 0建立案件 1 客服處理 2客服更改處理結果</param>
        /// <param name="Note">紀錄內容</param>
        /// <param name="Modifier">最後修改人員</param>
        /// <param name="RealIP">RealIP</param>
        /// <param name="ProxyIP">ProxyIP</param>
        /// <returns></returns>
        public BaseResult UpdateCustomServiceRecord(long CustomerServiceID, byte Status, string Note, string Modifier, long RealIP, long ProxyIP)
        {
            var dbResult = _customServiceRecordRepository.UpdateCustomServiceRecord(CustomerServiceID, Status, Note, Modifier, RealIP, ProxyIP);
            //回傳如果是null,先擋下,其他command使用就不用判斷null
            if (dbResult == null)
                return new BaseResult() { IsSuccess = false, RtnMsg = "更新失敗" };
            return dbResult;
        }
        /// <summary>
        /// 新增客服紀錄內容
        /// </summary>
        /// <param name="model">新增物件</param>
        /// <returns></returns>
        public BaseResult AddCustomServiceRecord(AddCustomServiceRecord model)
        {
            var dbResult = _customServiceRecordRepository.AddCustomServiceRecord(model);
            if (dbResult == null)
                return new BaseResult() { IsSuccess = false, RtnMsg = "新增失敗" };
            return dbResult;
        }
    }
}
