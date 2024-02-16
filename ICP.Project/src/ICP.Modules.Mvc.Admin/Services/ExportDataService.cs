using ICP.Modules.Mvc.Admin.Repositories;
using NPOI.HSSF.UserModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICP.Modules.Mvc.Admin.Services
{
    public class ExportDataService
    {
        ConfigRepository _configRepository;

        public ExportDataService(ConfigRepository configRepository)
        {
            _configRepository = configRepository;
        }

        #region Command 呼叫範例
        /*
        /// <summary>
        /// 匯出Xls
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public MemoryStream GetSuspenseMainExport(QuerySuspenseMainVM model)
        {
            var list = _suspenseMainService.ListSuspenseMain(model);
            if (!list.Any()) return null;

            string functionName = "交易黑名單";

            string dateRange = $"查詢日期：{model.StartDate.ToString("yyyy-MM-dd")} ~ {model.EndDate.ToString("yyyy-MM-dd")}";

            #region 標題
            string[] header = new string[]
            {
                "姓名", "手機號碼", "身分證字號", "E-mail", "停權原因", "會員狀態", "建立時間", "建立者", "審核狀態"
            };
            #endregion

            Func<SuspenseMainVM, string[]> arryDataGenerator = t =>
            {
                string AuthStatus;
                if (t.AuthStatus == 0)
                    AuthStatus = "待審核";
                else if (t.AuthStatus == 1)
                    AuthStatus = "審核通過";
                else if (t.AuthStatus == 2)
                    AuthStatus = "審核失敗";
                else
                    AuthStatus = string.Empty;

                var values = new string[]
                {
                    t.CName,
                    t.CellPhone,
                    t.IDNO,
                    t.Email,
                    t.ReasonDesc,
                    t.SuspenseDesc,
                    t.CreateDate.ToString("yyyy-MM-dd HH:mm:ss"),
                    t.CreateUser,
                    AuthStatus
                };

                return values;
            };

            return _suspenseMainService.GetXlsStream(header, list, arryDataGenerator, functionName, dateRange);
        } 
        */
        #endregion        

        /// <summary>
        /// 取得XLS串流
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="header"></param>
        /// <param name="list"></param>
        /// <param name="arryDataGenerator"></param>
        /// <param name="detailDataGenerator"></param>
        /// <returns></returns>
        public MemoryStream GetXlsStream<T>(string[] header, List<T> list, Func<T, string[]> arryDataGenerator, string functionName, string dateRange = null, bool wrapText = false)
        {
            if (header == null || header.Length == 0 || list == null || list.Count == 0) return null;

            //建立工作表
            HSSFWorkbook workbook = new HSSFWorkbook();

            //建立sheet
            HSSFSheet sheet = (HSSFSheet)workbook.CreateSheet("Sheet1");

            //置中
            HSSFCellStyle css = (HSSFCellStyle)sheet.Workbook.CreateCellStyle();
            css.VerticalAlignment = NPOI.SS.UserModel.VerticalAlignment.Center;
            css.Alignment = NPOI.SS.UserModel.HorizontalAlignment.Center;

            //字型
            HSSFFont headerFont = (HSSFFont)sheet.Workbook.CreateFont();
            headerFont.Boldweight = (short)NPOI.SS.UserModel.FontBoldWeight.Bold;

            HSSFCellStyle headerCss = (HSSFCellStyle)sheet.Workbook.CreateCellStyle();
            headerCss.VerticalAlignment = NPOI.SS.UserModel.VerticalAlignment.Center;
            headerCss.Alignment = NPOI.SS.UserModel.HorizontalAlignment.Center;
            headerCss.SetFont(headerFont);

            string companyName = _configRepository.IcashCompanyName;
            sheet.CreateRow(0).CreateCell(0).SetCellValue(companyName);
            sheet.AddMergedRegion(new NPOI.SS.Util.CellRangeAddress(0, 0, 0, header.Length - 1));
            sheet.GetRow(0).GetCell(0).CellStyle = css;

            sheet.CreateRow(1).CreateCell(0).SetCellValue(functionName);
            sheet.AddMergedRegion(new NPOI.SS.Util.CellRangeAddress(1, 1, 0, header.Length - 1));
            sheet.GetRow(1).GetCell(0).CellStyle = css;

            int rowIndex = 2;
            if (!string.IsNullOrWhiteSpace(dateRange))
            {
                rowIndex = 3;
                sheet.CreateRow(2).CreateCell(0).SetCellValue(dateRange);
                sheet.AddMergedRegion(new NPOI.SS.Util.CellRangeAddress(2, 2, 0, header.Length - 1));
                sheet.GetRow(2).GetCell(0).CellStyle = css;
            }

            var row = sheet.CreateRow(rowIndex);
            for (int i = 0; i < header.Length; i++)
            {
                row.CreateCell(i).SetCellValue(header[i]);
                row.GetCell(i).CellStyle = css;
            }

            list.ForEach(item =>
            {
                rowIndex++;
                var data1 = arryDataGenerator(item);
                row = sheet.CreateRow(rowIndex);
                for (int i = 0; i < data1.Length; i++)
                {
                    row.CreateCell(i).SetCellValue(data1[i]);
                    row.GetCell(i).CellStyle = css;
                }
            });

            MemoryStream file = new MemoryStream();
            workbook.Write(file);
            workbook = null;
            sheet = null;
            return file;
        }

        /// <summary>
        /// 取得愛金卡股份有限公司的表頭
        /// </summary>
        /// <param name="header"></param>
        /// <param name="functionName"></param>
        /// <param name="sheetName"></param>
        /// <returns></returns>
        public HSSFWorkbook GetXlsHeader(string[] header, string functionName, string sheetName = "Sheet1")
        {
            if (header == null || header.Length == 0) return null;

            //建立工作表
            HSSFWorkbook workbook = new HSSFWorkbook();

            //建立sheet
            HSSFSheet sheet = (HSSFSheet)workbook.CreateSheet(!string.IsNullOrWhiteSpace(sheetName) ? sheetName : "Sheet1");

            //置中
            HSSFCellStyle css = (HSSFCellStyle)sheet.Workbook.CreateCellStyle();
            css.VerticalAlignment = NPOI.SS.UserModel.VerticalAlignment.Center;
            css.Alignment = NPOI.SS.UserModel.HorizontalAlignment.Center;

            //字型
            HSSFFont headerFont = (HSSFFont)sheet.Workbook.CreateFont();
            headerFont.Boldweight = (short)NPOI.SS.UserModel.FontBoldWeight.Bold;

            HSSFCellStyle headerCss = (HSSFCellStyle)sheet.Workbook.CreateCellStyle();
            headerCss.VerticalAlignment = NPOI.SS.UserModel.VerticalAlignment.Center;
            headerCss.Alignment = NPOI.SS.UserModel.HorizontalAlignment.Center;
            headerCss.SetFont(headerFont);

            string companyName = _configRepository.IcashCompanyName;
            sheet.CreateRow(0).CreateCell(0).SetCellValue(companyName);
            sheet.AddMergedRegion(new NPOI.SS.Util.CellRangeAddress(0, 0, 0, header.Length - 1));
            sheet.GetRow(0).GetCell(0).CellStyle = css;

            sheet.CreateRow(1).CreateCell(0).SetCellValue(functionName);
            sheet.AddMergedRegion(new NPOI.SS.Util.CellRangeAddress(1, 1, 0, header.Length - 1));
            sheet.GetRow(1).GetCell(0).CellStyle = css;

            return workbook;
        }

        /// <summary>
        /// 取得XLS串流
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="header"></param>
        /// <param name="list"></param>
        /// <param name="arryDataGenerator"></param>
        /// <param name="detailDataGenerator"></param>
        /// <returns></returns>
        public MemoryStream GetXlsStreamExt<T>
        (
            string[] header,
            List<T> list,
            Func<T, string[]> arryDataGenerator,
            string functionName,
            string dateRange = null,
            Func<List<T>, string[]> dataCounter = null,
            List<xlsItem> extraHeader = null
        )
        {
            if (header == null || header.Length == 0 || list == null || list.Count == 0) return null;

            //建立工作表
            HSSFWorkbook workbook = new HSSFWorkbook();

            //建立sheet
            HSSFSheet sheet = (HSSFSheet)workbook.CreateSheet("Sheet1");

            //置中
            HSSFCellStyle css = (HSSFCellStyle)sheet.Workbook.CreateCellStyle();
            css.VerticalAlignment = NPOI.SS.UserModel.VerticalAlignment.Center;
            css.Alignment = NPOI.SS.UserModel.HorizontalAlignment.Center;

            //字型
            HSSFFont headerFont = (HSSFFont)sheet.Workbook.CreateFont();
            headerFont.Boldweight = (short)NPOI.SS.UserModel.FontBoldWeight.Bold;

            HSSFCellStyle headerCss = (HSSFCellStyle)sheet.Workbook.CreateCellStyle();
            headerCss.VerticalAlignment = NPOI.SS.UserModel.VerticalAlignment.Center;
            headerCss.Alignment = NPOI.SS.UserModel.HorizontalAlignment.Center;
            headerCss.SetFont(headerFont);

            string companyName = _configRepository.IcashCompanyName;
            sheet.CreateRow(0).CreateCell(0).SetCellValue(companyName);
            sheet.AddMergedRegion(new NPOI.SS.Util.CellRangeAddress(0, 0, 0, header.Length - 1));
            sheet.GetRow(0).GetCell(0).CellStyle = css;

            sheet.CreateRow(1).CreateCell(0).SetCellValue(functionName);
            sheet.AddMergedRegion(new NPOI.SS.Util.CellRangeAddress(1, 1, 0, header.Length - 1));
            sheet.GetRow(1).GetCell(0).CellStyle = css;

            int rowIndex = 2;
            if (!string.IsNullOrWhiteSpace(dateRange))
            {
                rowIndex = 3;
                sheet.CreateRow(2).CreateCell(0).SetCellValue(dateRange);
                sheet.AddMergedRegion(new NPOI.SS.Util.CellRangeAddress(2, 2, 0, header.Length - 1));
                sheet.GetRow(2).GetCell(0).CellStyle = css;
            }

            if (extraHeader != null)
            {
                extraHeader.ForEach(act =>
                {
                    var extraHeaderRow = sheet.GetRow(act.row) ?? sheet.CreateRow(act.row);
                    extraHeaderRow.CreateCell(act.col).SetCellValue(act.value);
                    sheet.AddMergedRegion(new NPOI.SS.Util.CellRangeAddress(act.row, act.row, act.col, act.col + act.mergeLen - 1));
                    sheet.GetRow(act.row).GetCell(act.col).CellStyle = css;
                    if (act.row >= rowIndex) rowIndex = act.row + 1;
                });
            }

            var row = sheet.CreateRow(rowIndex);
            for (int i = 0; i < header.Length; i++)
            {
                row.CreateCell(i).SetCellValue(header[i]);
                row.GetCell(i).CellStyle = css;
            }

            list.ForEach(item =>
            {
                rowIndex++;
                var data1 = arryDataGenerator(item);
                row = sheet.CreateRow(rowIndex);
                for (int i = 0; i < data1.Length; i++)
                {
                    row.CreateCell(i).SetCellValue(data1[i]);
                    row.GetCell(i).CellStyle = css;
                }
            });

            if (dataCounter != null)
            {
                rowIndex++;
                var data2 = dataCounter(list);
                row = sheet.CreateRow(rowIndex);
                for (int i = 0; i < data2.Length; i++)
                {
                    row.CreateCell(i).SetCellValue(data2[i]);
                    row.GetCell(i).CellStyle = css;
                }
            }

            MemoryStream file = new MemoryStream();
            workbook.Write(file);
            workbook = null;
            sheet = null;
            return file;
        }
    }

    public class xlsItem
    {
        public int row;
        public int col;
        public int mergeLen;
        public string value;
    }
}
