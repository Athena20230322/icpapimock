using ICP.Modules.Mvc.Admin.Models;
using ICP.Modules.Mvc.Admin.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICP.Modules.Mvc.Admin.Commands
{
    public class TopUpReportCommand
    {
        private readonly TopUpReportService _topUpReportService = null;
        private readonly ExportDataService _exportDataService = null;

        public TopUpReportCommand(TopUpReportService topUpReportService, ExportDataService exportDataService)
        {
            _topUpReportService = topUpReportService;
            _exportDataService = exportDataService;
        }

        /// <summary>
        /// 取得儲值明細資料清單
        /// </summary>
        /// <param name="topUpReportQueryCondition"></param>
        /// <returns></returns>
        public List<TopUpReportQueryResult> ListTopUpDetails(TopUpReportQueryCondition topUpReportQueryCondition)
        {
            return _topUpReportService.ListTopUpDetails(topUpReportQueryCondition);
        }

        /// <summary>
        /// 匯出儲值報表 Excel
        /// </summary>
        /// <param name="topUpReportQueryCondition"></param>
        /// <returns></returns>
        public MemoryStream ExportTopUpReport(TopUpReportQueryCondition topUpReportQueryCondition)
        {
            List<TopUpReportQueryResult> topUpReportQueryResultList = _topUpReportService.ListTopUpDetails(topUpReportQueryCondition);

            string functionName = "儲值報表";
            string dateRange = $"查詢日期：{topUpReportQueryCondition.StartDate.ToString("yyyy-MM-dd")} ~ {topUpReportQueryCondition.EndDate.ToString("yyyy-MM-dd")}";

            #region 報表欄位
            string[] header = new string[]
            {
                "訂單日期", "收款日期", "傳輸日期", "繳費期限(銀行轉帳)", "電支帳號", "icash 訂單編號", "儲值金額", "實收金額", "儲值方式", "款項來源(銀行/超商)",
                "銀行連結帳號/虛擬帳號", "超商店號", "銀行代碼", "銀行轉帳轉出帳號", "交易服務費( %數 / $筆)", "交易服務費金額", "應收款項(淨額)", "撥款狀態"
            };
            #endregion

            Func<TopUpReportQueryResult, string[]> arryDataGenerator = t =>
            {
                var values = new string[]
                {
                    t.CreateDate.ToString("yyyy/MM/dd HH:mm:ss"),
                    t.PaymentDate.ToString("yyyy/MM/dd HH:mm:ss"),
                    t.TransmittalDate.HasValue ? t.TransmittalDate.Value.ToString("yyyy/MM/dd") : "-",
                    t.ExpireDate.HasValue ? t.ExpireDate.Value.ToString("yyyy/MM/dd") : "-",
                    t.ICPMID,
                    t.TradeNo,
                    t.TopUpAmount.ToString("N0"),
                    t.RealReceiveAmount.ToString("N0"),
                    t.TopUpTypeMeaning,
                    t.TopUpTypeSource,
                    t.Account,
                    t.StoreID,
                    t.BankCode,
                    t.BankAccNo,
                    t.TradeServiceRate.ToString("N1"),
                    t.TradeServiceAmount.ToString("N1"),
                    t.NetAmount.ToString("N0"),
                    t.TopUpStatusMeaning
                };

                return values;
            };

            return _exportDataService.GetXlsStream(header, topUpReportQueryResultList, arryDataGenerator, functionName, dateRange);
        }
    }
}
