using ICP.Library.Services.MemberServices;
using ICP.Modules.Mvc.Admin.Models;
using ICP.Modules.Mvc.Admin.Models.ViewModels;
using ICP.Modules.Mvc.Admin.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICP.Modules.Mvc.Admin.Commands
{
    public class UnregisteredDataCommand
    {
        UnregisteredDataService _unregisteredDataService;
        LibMemberInfoService _libMemberInfoService;
        ExportDataService _exportDataService;

        public UnregisteredDataCommand(
            UnregisteredDataService unregisteredDataService,
            LibMemberInfoService libMemberInfoService,
            ExportDataService exportDataService
            )
        {
            _unregisteredDataService = unregisteredDataService;
            _libMemberInfoService = libMemberInfoService;
            _exportDataService = exportDataService;
        }

        /// <summary>
        /// 取得被刪除的會員資料
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public List<UnregisteredData> ListUnregisteredData(QueryUnregisteredDataVM model)
        {
            var list = _unregisteredDataService.ListUnregisteredData(model);
            list.ForEach(t => t.CName = string.IsNullOrWhiteSpace(t.CName) ? t.CName : _libMemberInfoService.ConcealPartialCName(t.CName));

            return list;
        }

        /// <summary>
        /// 取得單筆被刪除的會員資料
        /// </summary>
        /// <param name="MID"></param>
        /// <returns></returns>
        public UnregisteredData GetUnregisteredData(long MID)
        {
            var result = _unregisteredDataService.GetUnregisteredData(MID);
            result.CName = string.IsNullOrWhiteSpace(result.CName) ? result.CName : _libMemberInfoService.ConcealPartialCName(result.CName);

            return result;
        }

        /// <summary>
        /// 匯出Xls
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public MemoryStream Export(QueryUnregisteredDataVM model)
        {
            var list = _unregisteredDataService.ListUnregisteredData(model);
            if (!list.Any()) return null;

            string functionName = "電支使用者刪除資料查詢";

            string dateRange = $"查詢日期：{model.StartDate.ToString("yyyy-MM-dd")} ~ {model.EndDate.ToString("yyyy-MM-dd")}";

            #region 標題
            string[] header = new string[]
            {
                "刪除時間", "姓名", "手機號碼", "身分證字號", "居留證號", "E-mail", "刪除方式", "刪除操作人", "刪除原因"
            };
            #endregion

            Func<UnregisteredData, string[]> arryDataGenerator = t =>
            {
                string CreateUser;
                string Source;
                if (t.Source == 0)
                {
                    Source = "系統刪除";
                    CreateUser = "系統";
                }
                else if (t.Source == 1)
                {
                    Source = "人工刪除";
                    CreateUser = t.CreateUser;
                }
                else
                {
                    Source = string.Empty;
                    CreateUser = string.Empty;
                }

                string isIdno;
                if (t.isIDNO)
                    isIdno = "Y";
                else
                    isIdno = "N";

                string isUniformID;
                if (t.isUniformID)
                    isUniformID = "Y";
                else
                    isUniformID = "N";

                string isEmail;
                if (t.isEmail)
                    isEmail = "Y";
                else
                    isEmail = "N";

                string cName;
                if (string.IsNullOrWhiteSpace(t.CName))
                    cName = string.Empty;
                else
                    cName = _libMemberInfoService.ConcealPartialCName(t.CName);

                var values = new string[]
                {
                    t.CreateDate.ToString("yyyy-MM-dd HH:mm:ss"),
                    cName,
                    t.CellPhone,
                    isIdno,
                    isUniformID,
                    isEmail,
                    Source,
                    CreateUser,
                    t.Notes
                };

                return values;
            };

            return _exportDataService.GetXlsStream(header, list, arryDataGenerator, functionName, dateRange);
        }
    }
}
