using ICP.Infrastructure.Abstractions.Logging;
using ICP.Library.Repositories.MemberRepositories;
using ICP.Modules.Mvc.Admin.Models.IPRecord;
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
    public class IPRecordService
    {
        IPRecordRepository _ipRecordRepository;
        MemberConfigCyptRepository _configCyptRepository;
        ILogger<IPRecordService> _logger;

        public IPRecordService(IPRecordRepository ipRecordRepository, MemberConfigCyptRepository configCyptRepository, ILogger<IPRecordService> logger)
        {
            _ipRecordRepository = ipRecordRepository;
            _configCyptRepository = configCyptRepository;
            _logger = logger;
        }
        /// <summary>
        /// IP紀錄查詢
        /// </summary>
        /// <param name="model">查詢物件</param>
        /// <returns></returns>
        public List<QueryIPRecordResult> ListLoginRecord(QueryIPRecord model)
        {
            model.Account = EncryptAccount(model.Account);
            var result = _ipRecordRepository.ListLoginRecord(model).OrderByDescending(i => i.LogTime).ToList();
            foreach(var data in result)
                data.Account = DecryptAccount(data.Account);
            return result;
        }
        /// <summary>
        /// IP 字串轉換成long
        /// </summary>
        /// <param name="ipString">ip字串</param>
        /// <returns></returns>
        public long? IPStringToLong(string ipString)
        {
            if (string.IsNullOrEmpty(ipString))
                return 0;

            var address = IPAddress.Parse(ipString);
            byte[] bytes = address.GetAddressBytes();
            if (BitConverter.IsLittleEndian)
            {
                Array.Reverse(bytes);
            }

            return BitConverter.ToUInt32(bytes, 0);
        }
        /// <summary>
        /// IP long轉換成字串
        /// </summary>
        /// <param name="ip">ip</param>
        /// <returns></returns>
        public string IPLongToString(long ip)
        {
            return IPAddress.Parse(ip.ToString()).ToString();
        }
        /// <summary>
        /// 帳號解密
        /// </summary>
        /// <param name="account">帳號</param>
        /// <returns></returns>
        public string DecryptAccount(string account)
        {
            if (string.IsNullOrWhiteSpace(account))
                return string.Empty;
            return _configCyptRepository.Decrypt_UserCode(account);
        }
        /// <summary>
        /// 帳號加密
        /// </summary>
        /// <param name="account">帳號</param>
        /// <returns></returns>
        public string EncryptAccount(string account)
        {
            if (string.IsNullOrWhiteSpace(account))
                return string.Empty;
            return _configCyptRepository.Encrypt_UserCode(account);
        }
    }
}
#region 匯出json 序列化 範例 多分頁(之後可參考，先暫時用ExportDataService)
///// <summary>
///// 匯出Excel
///// </summary>
///// <typeparam name="Q">查詢物件</typeparam>
///// <typeparam name="D">資料物件</typeparam>
///// <param name="QuerySheetName">查詢分頁名稱</param>
///// <param name="query">查詢物件</param>
///// <param name="DataSheetName">資料分頁名稱</param>
///// <param name="datas">資料物件清單</param>
///// <returns></returns>
//public MemoryStream Export<Q, D>(string QuerySheetName, Q query, string DataSheetName, List<D> datas)
//{
//    try
//    {
//        HSSFWorkbook workbook = new HSSFWorkbook();
//        //建立 IP查詢紀錄 分頁
//        var sheet = workbook.CreateSheet(DataSheetName);
//        if (datas.Any())
//        {
//            var dataJsonString = JsonConvert.SerializeObject(datas);
//            var list = JsonConvert.DeserializeObject<IEnumerable<Dictionary<string, object>>>(dataJsonString);
//            var titles = list.Select(x => x.Keys).First();
//            var contents = list.Select(x => x.Values).ToList();
//            CreatTitle(sheet, titles.ToList());
//            CreatData(sheet, contents);
//            AutoSizeColumn(sheet, titles.Count);
//        }
//        //建立 IP查詢條件 分頁
//        sheet = workbook.CreateSheet(QuerySheetName);
//        if (query != null)
//        {
//            var dataJsonString = JsonConvert.SerializeObject(query);
//            var data = JsonConvert.DeserializeObject<Dictionary<string, object>>(dataJsonString);
//            var titles = data.Keys;
//            List<Dictionary<string, object>.ValueCollection> contents = new List<Dictionary<string, object>.ValueCollection>();
//            contents.Add(data.Values);
//            CreatTitle(sheet, titles.ToList());
//            CreatData(sheet, contents);
//            AutoSizeColumn(sheet, titles.Count);
//        }
//        MemoryStream stream = new MemoryStream();
//        workbook.Write(stream);
//        workbook = null;
//        return stream;
//    }
//    catch (Exception ex)
//    {
//        _logger.Error(ex, "匯出失敗");
//        return null;
//    }
//}
///// <summary>
///// 建立資料標題
///// </summary>
///// <param name="sheet">節點</param>
///// <param name="titles">標題</param>
//void CreatTitle(ISheet sheet, List<string> titles)
//{
//    var row = sheet.CreateRow(sheet.PhysicalNumberOfRows);
//    for (int i = 0; i < titles.Count; i++)
//        row.CreateCell(i).SetCellValue(titles[i]);
//}
///// <summary>
///// 建立內容
///// </summary>
///// <param name="sheet">節點</param>
///// <param name="datas">內容</param>
//void CreatData(ISheet sheet, List<Dictionary<string, object>.ValueCollection> datas)
//{
//    for (int i = 0; i < datas.Count; i++)
//    {
//        var row = sheet.CreateRow(i + 1);
//        var rowdata = datas[i].ToList();
//        for (int j = 0; j < rowdata.Count; j++)
//        {
//            string result = string.Empty;
//            if (rowdata[j] != null)
//                result = rowdata[j].ToString();
//            row.CreateCell(j).SetCellValue(result);
//        }
//    }
//}
///// <summary>
///// 自動對齊
///// </summary>
///// <param name="sheet">節點</param>
///// <param name="Count">要對齊的行數</param>
//void AutoSizeColumn(ISheet sheet, int Count)
//{
//    for (int i = 0; i < Count; i++)
//        sheet.AutoSizeColumn(i);
//}
//public class ExportIPRecord
//{
//    /// <summary>
//    /// 起始日期
//    /// </summary>
//    [JsonIgnore]
//    public DateTime StartDate { get; set; }
//    /// <summary>
//    /// 結束時間
//    /// </summary>
//    [JsonIgnore]
//    public DateTime EndDate { get; set; }
//    /// <summary>
//    /// 查詢時間
//    /// </summary>
//    [JsonProperty("查詢時間")]
//    public string SearchTime
//    {
//        get
//        {
//            return $@"{StartDate.ToString("yyyy/MM/dd")}~{EndDate.ToString("yyyy/MM/dd")}";
//        }
//    }
//    /// <summary>
//    /// 登入帳號
//    /// </summary>
//    [JsonProperty("登入帳號")]
//    public string Account { get; set; }
//    /// <summary>
//    /// 手機號碼
//    /// </summary>
//    [JsonProperty("手機號碼")]
//    public string CellPhone { get; set; }
//    /// <summary>
//    /// 電支帳號
//    /// </summary>
//    [JsonProperty("電支帳號")]
//    public string ICPMID { get; set; }
//    /// <summary>
//    /// 登入IP
//    /// </summary>
//    [JsonProperty("登入IP")]
//    public string UserIP { get; set; }
//    /// <summary>
//    /// 裝置ID
//    /// </summary>
//    [JsonProperty("裝置ID")]
//    public string DeviceID { get; set; }
//}
#endregion