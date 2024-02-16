using ICP.Modules.Mvc.Admin.Models.PaymentCenter;
using ICP.Modules.Mvc.Admin.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ICP.Modules.Mvc.Admin.Commands
{
    /// <summary>
    /// 金流中心
    /// </summary>
    public class PaymentCenterCommand
    {
        PaymentCenterService _paymentCenterService;
        ExportDataService _exportDataService;

        public PaymentCenterCommand(PaymentCenterService paymentCenterService, ExportDataService exportDataService)
        {
            _paymentCenterService = paymentCenterService;
            _exportDataService = exportDataService;
        }

        #region 金流中心統計資訊
        /// <summary>
        /// 金流中心統計資訊查詢
        /// </summary>
        public List<TradeStatisticsModel> ListTradeStatistics(TradeStatisticsQueryModel model)
        {
            return _paymentCenterService.ListTradeStatistics(model);
        }

        /// <summary>
        /// 金流中心統計資訊明細查詢
        /// </summary>
        public List<TradeStatisticsDetailModel> ListTradeStatisticsDetail(TradeStatisticsDetailQueryModel model)
        {
            return _paymentCenterService.ListTradeStatisticsDetail(model);
        }

        /// <summary>
        /// 匯出金流中心統計資訊報表
        /// FileFormat : Excel
        /// </summary>
        public void ExportTradeStatistics(HttpResponseBase response, TradeStatisticsQueryModel model)
        {
            model.PageNo = 1;
            model.PageSize = 65535;

            // 取得金流中心統計資訊
            var list = _paymentCenterService.ListTradeStatistics(model);
            if (list == null)
                return;

            string functionName = "金流中心統計資訊";

            string dateRange = $"查詢日期：{model.StartDate.ToString("yyyy-MM-dd")} ~ {model.EndDate.ToString("yyyy-MM-dd")}";

            string[] header = new string[]
            {
                "訂單日期",
                "icash Pay 帳戶筆數", "icash Pay 帳戶交易金額", "icash Pay 帳戶手續費", "連結扣款帳戶筆數", "連結扣款帳戶交易金額", "連結扣款帳戶手續費",
                "icash Pay 帳戶筆數", "icash Pay 帳戶交易金額", "icash Pay 帳戶手續費", "連結扣款帳戶筆數", "連結扣款帳戶交易金額", "連結扣款帳戶手續費"
            };

            Func<TradeStatisticsModel, string[]> arryDataGenerator = t =>
            {
                var values = new string[]
                {
                    t.PaymentDate.ToShortDateString(),
                    t.FirstATMTopupCount.ToString("N0"),
                    t.FirstATMTopupAmount.ToString("N0"),
                    t.FirstACLTopupCount.ToString("N0"),
                    t.FirstACLTopupAmount.ToString("N0"),
                    t.FirstACLTradeCount.ToString("N0"),
                    t.FirstACLTradeAmount.ToString("N0"),

                    t.CtbcATMTopupCount.ToString("N0"),
                    t.CtbcATMTopupAmount.ToString("N0"),
                    t.CtbcACLTopupCount.ToString("N0"),
                    t.CtbcACLTopupAmount.ToString("N0"),
                    t.CtbcACLTradeCount.ToString("N0"),
                    t.CtbcACLTradeAmount.ToString("N0")
                };

                return values;
            };

            Func<List<TradeStatisticsModel>, string[]> dataCounter = t =>
            {
                var values = new string[]
                {
                    $"小計：共{t.Count.ToString("N0")}筆",
                    t.Sum(x=>x.FirstATMTopupCount).ToString("N0"),
                    t.Sum(x=>x.FirstATMTopupAmount).ToString("N0"),
                    t.Sum(x=>x.FirstACLTopupCount).ToString("N0"),
                    t.Sum(x=>x.FirstACLTopupAmount).ToString("N0"),
                    t.Sum(x=>x.FirstACLTradeCount).ToString("N0"),
                    t.Sum(x=>x.FirstACLTradeAmount).ToString("N0"),

                    t.Sum(x=>x.CtbcATMTopupCount).ToString("N0"),
                    t.Sum(x=>x.CtbcATMTopupAmount).ToString("N0"),
                    t.Sum(x=>x.CtbcACLTopupCount).ToString("N0"),
                    t.Sum(x=>x.CtbcACLTopupAmount).ToString("N0"),
                    t.Sum(x=>x.CtbcACLTradeCount).ToString("N0"),
                    t.Sum(x=>x.CtbcACLTradeAmount).ToString("N0")
                };

                return values;
            };

            var extraHeader = new List<xlsItem>();
            extraHeader.Add(new xlsItem() { row = 3, col = 1, mergeLen = 6, value = "第一銀行" });
            extraHeader.Add(new xlsItem() { row = 3, col = 7, mergeLen = 6, value = "中國信託銀行" });

            extraHeader.Add(new xlsItem() { row = 4, col = 1, mergeLen = 2, value = "虛擬帳號 / 儲值" });
            extraHeader.Add(new xlsItem() { row = 4, col = 3, mergeLen = 2, value = "連結扣款帳戶 / 儲值" });
            extraHeader.Add(new xlsItem() { row = 4, col = 5, mergeLen = 2, value = "連結扣款帳戶 / 交易" });
            extraHeader.Add(new xlsItem() { row = 4, col = 7, mergeLen = 2, value = "虛擬帳號 / 儲值" });
            extraHeader.Add(new xlsItem() { row = 4, col = 9, mergeLen = 2, value = "連結扣款帳戶 / 儲值" });
            extraHeader.Add(new xlsItem() { row = 4, col = 11, mergeLen = 2, value = "連結扣款帳戶 / 交易" });

            // 產出報表內容
            var file = _exportDataService.GetXlsStreamExt<TradeStatisticsModel>(header, list, arryDataGenerator, functionName, dateRange, dataCounter, extraHeader);
            if (file == null)
                return;

            // 取得匯出檔名
            var fileName = _paymentCenterService.GetExportTradeStatisticsFileName();

            // 匯出報表
            _paymentCenterService.ExportExcelFile(response,
                                                  "Content-Disposition",
                                                  $"attachment;filename={fileName}",
                                                  file.GetBuffer());
        }

        /// <summary>
        /// 匯出金流中心統計資訊明細報表
        /// FileFormat : Excel
        /// </summary>
        public void ExportTradeStatisticsDetail(HttpResponseBase response, TradeStatisticsDetailQueryModel model)
        {
            // 取得匯出檔名
            var fileName = _paymentCenterService.GetExportTradeStatisticsFileName();

            // 取得金流中心統計資訊明細
            var list = _paymentCenterService.ListTradeStatisticsDetail(model);
            if (list == null)
                return;

            string functionName = "金流中心統計明細資訊";

            string dateRange = $"查詢日期：{model.PaymentDate.ToString("yyyy-MM-dd")}";

            string[] header = new string[]
            {
                "電支帳號", "名稱", "筆數", "金額"
            };

            Func<TradeStatisticsDetailModel, string[]> arryDataGenerator = t =>
            {
                var values = new string[]
                {
                    t.MID.ToString(),
                    t.Name,
                    t.Count.ToString("N0"),
                    t.Amount.ToString("N0")
                };

                return values;
            };

            Func<List<TradeStatisticsDetailModel>, string[]> dataCounter = t =>
            {
                var values = new string[]
                {
                    $"小計：共{t.Count.ToString("N0")}筆",
                    string.Empty,
                    t.Sum(x=>x.Count).ToString("N0"),
                    t.Sum(x=>x.Amount).ToString("N0")
                };

                return values;
            };

            // 產出報表內容
            var file = _exportDataService.GetXlsStreamExt(header, list, arryDataGenerator, functionName, dateRange, dataCounter);
            if (file == null)
                return;

            // 匯出報表
            _paymentCenterService.ExportExcelFile(response,
                                                  "Content-Disposition",
                                                  $"attachment;filename={fileName}",
                                                  file.GetBuffer());
        }
        #endregion

        #region 每日營收統計
        /// <summary>
        /// 每日營收統計資訊查詢
        /// </summary>
        public List<DailyRevenueStatisticsModel> ListDailyRevenueStatistics(DailyRevenueStatisticsQueryModel model)
        {
            return _paymentCenterService.ListDailyRevenueStatistics(model);
        }

        /// <summary>
        /// 匯出金流中心統計資訊報表
        /// FileFormat : Excel
        /// </summary>
        public void ExportDailyRevenueStatistics(HttpResponseBase response, DailyRevenueStatisticsQueryModel model)
        {
            model.PageNo = 1;
            model.PageSize = 65535;

            // 取得金流中心統計資訊
            var list = _paymentCenterService.ListDailyRevenueStatistics(model);
            if (list == null)
                return;

            string functionName = "每日營收統計報表";

            string dateRange = $"查詢日期：{model.StartDate.ToString("yyyy-MM-dd")} ~ {model.EndDate.ToString("yyyy-MM-dd")}";

            string[] header = new string[]
            {
                "訂單日期", "icash Pay 帳戶筆數", "icash Pay 帳戶交易金額", "icash Pay 帳戶手續費", "連結扣款帳戶筆數", "連結扣款帳戶交易金額", "連結扣款帳戶手續費"
            };

            Func<DailyRevenueStatisticsModel, string[]> arryDataGenerator = t =>
            {
                var values = new string[]
                {
                    t.PaymentDate.ToShortDateString(),
                    t.ICashCount.ToString("N0"),
                    t.ICashAmount.ToString("N0"),
                    t.ICashFee.ToString("N0"),
                    t.ACLCount.ToString("N0"),
                    t.ACLAmount.ToString("N0"),
                    t.ACLFee.ToString("N0")
                };

                return values;
            };

            Func<List<DailyRevenueStatisticsModel>, string[]> dataCounter = t =>
            {
                var values = new string[]
                {
                    $"小計：共{t.Count.ToString("N0")}筆",
                    t.Sum(x=>x.ICashCount).ToString("N0"),
                    t.Sum(x=>x.ICashAmount).ToString("N0"),
                    t.Sum(x=>x.ICashFee).ToString("N0"),
                    t.Sum(x=>x.ACLCount).ToString("N0"),
                    t.Sum(x=>x.ACLAmount).ToString("N0"),
                    t.Sum(x=>x.ACLFee).ToString("N0")
                };

                return values;
            };

            // 產出報表內容
            var file = _exportDataService.GetXlsStreamExt(header, list, arryDataGenerator, functionName, dateRange, dataCounter);
            if (file == null)
                return;

            // 取得匯出檔名
            var fileName = _paymentCenterService.GetExportDailyRevenueStatisticsFileName();

            // 匯出報表
            _paymentCenterService.ExportExcelFile(response,
                                                  "Content-Disposition",
                                                  $"attachment;filename={fileName}",
                                                  file.GetBuffer());
        }
        #endregion

        #region 金流手續費統計(月結)
        #region 頁面選單設定
        /// <summary>
        /// 特店查詢方式
        /// </summary>
        public List<SelectListItem> GetMerchantQueryList()
        {
            var merchantQueryList = new List<SelectListItem>
            {
                new SelectListItem { Value = "1", Text = "電支帳號" },
                new SelectListItem { Value = "2", Text = "名稱" }
            };
            return merchantQueryList;
        }
        /// <summary>
        /// 統計方式(年月) - 年
        /// </summary>
        public List<SelectListItem> GetYearList()
        {
            var yearList = new List<SelectListItem>();

            for (int i = 0; i < 30; i++)
            {
                string year = (2019 + i).ToString();
                yearList.Add(new SelectListItem { Value = year, Text = year });
            }
            return yearList;
        }
        /// <summary>
        /// 統計方式(年月) - 月
        /// </summary>
        public List<SelectListItem> GetMonthList()
        {
            var monthList = new List<SelectListItem>();

            for (int i = 1; i < 13; i++)
            {
                string month = i.ToString();
                monthList.Add(new SelectListItem { Value = month, Text = month });
            }
            return monthList;
        }
        /// <summary>
        /// 金流方式
        /// </summary>
        public List<SelectListItem> GetTradeModeList()
        {
            var tradeModeList = new List<SelectListItem>()
            {
                new SelectListItem { Value = "0", Text = "全部" },
                new SelectListItem { Value = "1", Text = "交易" },
                new SelectListItem { Value = "2", Text = "儲值" }
            };
            return tradeModeList;
        }
        #endregion

        /// <summary>
        /// 金流手續費統計資訊查詢
        /// </summary>
        public List<FeeStatisticsModel> ListFeeStatistics(FeeStatisticsQueryModel model)
        {
            return _paymentCenterService.ListFeeStatistics(model);
        }

        /// <summary>
        /// 金流手續費統計資訊明細查詢
        /// </summary>
        public List<FeeStatisticsDetailModel> ListFeeStatisticsDetail(FeeStatisticsDetailQueryModel model)
        {
            return _paymentCenterService.ListFeeStatisticsDetail(model);
        }

        /// <summary>
        /// 匯出金流手續費統計資訊報表
        /// FileFormat : Excel
        /// </summary>
        public void ExportFeeStatistics(HttpResponseBase response, FeeStatisticsQueryModel model)
        {
            model.PageNo = 1;
            model.PageSize = 65535;

            // 取得金流手續費統計資訊
            var list = _paymentCenterService.ListFeeStatistics(model);
            if (list == null)
                return;

            string functionName = "金流手續費統計表";

            string dateRange = $"查詢日期：{model.StartDate.ToString("yyyy-MM-dd")} ~ {model.EndDate.ToString("yyyy-MM-dd")}";

            string[] header = new string[]
            {
                model.StatisticsType == 1 ? "撥款日期" : "撥款月份",
                "電支帳號", "特店名稱", "統一編號", "金流方式", "交易/儲值筆數", "交易/儲值金額", "退款金額", "交易手續費 (%數 / $筆)", "交易手續費金額", "撥款淨額", "應收淨額", "儲值佣金費率", "儲值佣金"
            };

            Func<FeeStatisticsModel, string[]> arryDataGenerator = t =>
            {
                var values = new string[]
                {
                    model.StatisticsType == 1 ? t.AllocateDate.ToShortDateString():t.AllocateDate.ToString("yyyy/MM"),
                    t.MerchantID.ToString(),
                    t.MerchantName,
                    t.UnifiedBusinessNo,
                    t.TradeModeName,
                    t.Count.ToString("N0"),
                    t.Amount.ToString("N0"),
                    t.RefundAmount.ToString("N0"),
                    t.FeeRate,
                    t.Fee.ToString("N0"),
                    t.AllocateAmount.ToString("N0"),
                    t.ReceivableAmount.ToString("N0"),
                    t.TopupBrokerageRate,
                    t.TopupBrokerageAmount.ToString("N0")
                };

                return values;
            };

            Func<List<FeeStatisticsModel>, string[]> dataCounter = t =>
            {
                var values = new string[]
                {
                    $"小計：共{t[0].TotalCount.ToString("N0")}筆",
                    string.Empty,
                    string.Empty,
                    string.Empty,
                    string.Empty,
                    t[0].TotalTradeCount.ToString("N0"),
                    t[0].TotalTradeAmount.ToString("N0"),
                    t[0].TotalRefundAmount.ToString("N0"),
                    "-",
                    t[0].TotalChargeFee.ToString("N0"),
                    t[0].TotalAllocateAmount.ToString("N0"),
                    t[0].TotalReceivableAmount.ToString("N0"),
                    "-",
                    t[0].TotalTopupBrokerageAmount.ToString("N0")
                };

                return values;
            };

            // 產出報表內容
            var file = _exportDataService.GetXlsStreamExt(header, list, arryDataGenerator, functionName, dateRange, dataCounter);
            if (file == null)
                return;

            // 取得匯出檔名
            var fileName = _paymentCenterService.GetExportFeeStatisticsFileName();

            // 匯出報表
            _paymentCenterService.ExportExcelFile(response,
                                                  "Content-Disposition",
                                                  $"attachment;filename={fileName}",
                                                  file.GetBuffer());
        }

        /// <summary>
        /// 匯出金流手續費統計資訊報表
        /// FileFormat : Excel
        /// </summary>
        public void ExportFeeStatisticsDetail(HttpResponseBase response, FeeStatisticsDetailQueryModel model)
        {
            model.PageNo = 1;
            model.PageSize = 65535;

            // 取得金流手續費統計資訊
            var list = _paymentCenterService.ListFeeStatisticsDetail(model);
            if (list == null)
                return;

            string functionName = "金流手續費統計表-明細";

            string dateRange = $"查詢日期：{model.StartDate.ToString("yyyy-MM-dd")} ~ {model.EndDate.ToString("yyyy-MM-dd")}";

            string[] header = new string[]
            {
                "撥款日期", "電支帳號", "交易序號", "交易/儲值日期", "交易/儲值金額", "手續費/佣金費率( %數 / $筆)", "手續費/佣金", "退款金額"
            };

            Func<FeeStatisticsDetailModel, string[]> arryDataGenerator = t =>
            {
                var values = new string[]
                {
                    t.AllocateDate.ToShortDateString(),
                    t.MerchantID.ToString(),
                    t.TradeNo,
                    t.TradeDate.ToShortDateString(),
                    t.Amount.ToString("N0"),
                    t.FeeRate,
                    t.Fee.ToString("N0"),
                    t.RefundAmount.ToString("N0")
                };

                return values;
            };

            Func<List<FeeStatisticsDetailModel>, string[]> dataCounter = t =>
            {
                var values = new string[]
                {
                    $"小計：共{t[0].TotalCount.ToString("N0")}筆",
                    string.Empty,
                    string.Empty,
                    string.Empty,
                    t[0].TotalAmount.ToString("N0"),
                    "-",
                    t[0].TotalFee.ToString("N0"),
                    t[0].TotalRefundAmount.ToString("N0")
                };

                return values;
            };

            // 產出報表內容
            var file = _exportDataService.GetXlsStreamExt(header, list, arryDataGenerator, functionName, dateRange, dataCounter);
            if (file == null)
                return;

            // 取得匯出檔名
            var fileName = _paymentCenterService.GetExportFeeStatisticsDetailFileName();

            // 匯出報表
            _paymentCenterService.ExportExcelFile(response,
                                                  "Content-Disposition",
                                                  $"attachment;filename={fileName}",
                                                  file.GetBuffer());
        }
        #endregion
    }
}
