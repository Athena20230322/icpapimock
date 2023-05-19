using ICP.Infrastructure.Core.Extensions;
using ICP.Infrastructure.Core.Models;
using ICP.Modules.Mvc.Admin.Models.PaymentStatistics;
using ICP.Modules.Mvc.Admin.Models.PaymentStatistics.IncomeMonitor;
using ICP.Modules.Mvc.Admin.Models.PaymentStatistics.PaymentMonitor;
using ICP.Modules.Mvc.Admin.Models.PaymentStatistics.TimingMonitor;
using ICP.Modules.Mvc.Admin.Services;
using NPOI.HSSF.UserModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.Mvc;

namespace ICP.Modules.Mvc.Admin.Commands
{
    public class PaymentStatisticsCommand
    {
        private PaymentStatisticsService _paymentStatisticsService = null;
        private readonly ExportDataService _exportDataService = null;

        public PaymentStatisticsCommand(
            PaymentStatisticsService paymentStatisticsService,
            ExportDataService exportDataService
        )
        {
            _paymentStatisticsService = paymentStatisticsService;
            _exportDataService = exportDataService;
        }

        #region 每日提領金額監控

        #region 下拉選單設定
        /// <summary>
        /// 查核類型選單
        /// </summary>
        /// <returns>1:提領金額 2:30天累計提領金額</returns>
        public IEnumerable<SelectListItem> ListWithdrawTradeTypeItem()
        {
            var selectList = new List<SelectListItem>();

            selectList.Add(new SelectListItem { Value = "1", Text = "選擇日期提領金額" });
            selectList.Add(new SelectListItem { Value = "2", Text = "30天累計提領金額" });

            return selectList;
        }

        /// <summary>
        /// 排序方式選單
        /// </summary>
        /// <returns>1:選擇日期提領百分比 2:選擇日期提領金額 3:30天累計提領金額</returns>
        public IEnumerable<SelectListItem> ListWithdrawSortTypeItem()
        {
            var selectList = new List<SelectListItem>();

            selectList.Add(new SelectListItem { Value = "1", Text = "選擇日期提領百分比" });
            selectList.Add(new SelectListItem { Value = "2", Text = "選擇日期提領金額" });
            selectList.Add(new SelectListItem { Value = "3", Text = "30天累計提領金額" });

            return selectList;
        }

        /// <summary>
        /// 監控類型選單
        /// </summary>
        /// <returns></returns>
        public IEnumerable<SelectListItem> ListWithdrawSelectTypeItem()
        {
            var selectList = new List<SelectListItem>();

            selectList.Add(new SelectListItem { Value = "20", Text = "全部" });
            selectList.Add(new SelectListItem { Value = "3", Text = "已檢視" }); //提領檢視
            selectList.Add(new SelectListItem { Value = "2", Text = "備註" }); //提領備註

            return selectList;
        }
        #endregion

        #region 提領監控清單
        /// <summary>
        /// QueryWithdrawVM轉換ListWithdrawReq
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public DataResult<ListWithdrawDbReq> MappingToListWithdrawReq(QueryWithdrawVM model)
        {
            return _paymentStatisticsService.MappingToListWithdrawReq(model);
        }

        /// <summary>
        /// 每日提領金額監控清單
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public DataResult<List<ListWithdrawDbRes>> ListWithdraw(ListWithdrawDbReq req)
        {
            List<ListWithdrawDbRes> list = new List<ListWithdrawDbRes>();
            var result = new DataResult<List<ListWithdrawDbRes>>();

            #region 提領金額監控清單
            var listResult = _paymentStatisticsService.ListWithdraw(req);
            if (!listResult.IsSuccess)
            {
                return listResult;
            }
            list = listResult.RtnData;
            #endregion

            #region 提領排程清單
            foreach (var item in list)
            {
                List<BankTransferScheduleDbRes> bankTransferSchedule = _paymentStatisticsService.ListBankTransferSchedule(item.MID).RtnData;
                item.ListBankTransferSchedule = bankTransferSchedule.Where(w => w.Status == 1).ToList();
            }
            #endregion

            result.SetSuccess(list);

            return result;
        }
        #endregion

        #endregion

        #region 每日收款交易金額監控

        /// <summary>
        /// 每日收款交易金額監控 - 金流類型選單
        /// </summary>
        public IEnumerable<SelectListItem> ListIncomeTradeTypeItem()
        {
            var selectList = new List<SelectListItem>
            {
                new SelectListItem { Value = "1", Text = "帳戶餘額" },
                new SelectListItem { Value = "2", Text = "連結銀行帳號" },
                new SelectListItem { Value = "3", Text = "1日總收款額" },
                new SelectListItem { Value = "4", Text = "10天帳戶餘額收款" },
                new SelectListItem { Value = "5", Text = "30天帳戶餘額收款" }
            };

            return selectList;
        }

        /// <summary>
        /// 每日收款交易金額監控 - 排序方式選單
        /// </summary>
        public IEnumerable<SelectListItem> ListIncomeSortTypeItem()
        {
            var selectList = new List<SelectListItem>
            {
                new SelectListItem { Value = "1", Text = "帳戶餘額" },
                new SelectListItem { Value = "2", Text = "連結銀行帳號" },
                new SelectListItem { Value = "3", Text = "1日總收款額" },
                new SelectListItem { Value = "4", Text = "10天帳戶餘額收款" },
                new SelectListItem { Value = "5", Text = "30天帳戶餘額收款" }
            };

            return selectList;
        }

        /// <summary>
        /// 每日收款交易金額監控 - 監控類型選單
        /// </summary>
        public IEnumerable<SelectListItem> ListIncomeMonitorTypeItem()
        {
            var selectList = new List<SelectListItem>
            {
                new SelectListItem { Value = "40", Text = "全部" },
                new SelectListItem { Value = "5", Text = "已檢視" },
                new SelectListItem { Value = "4", Text = "備註" }
            };

            return selectList;
        }

        /// <summary>
        /// 每日收款交易金額監控 - 查詢監控記錄
        /// </summary>
        /// <param name="request">查詢條件</param>
        /// <returns></returns>
        public DataResult<List<QryIncomeMonitorRes>> ListIncomeMonitor(QryIncomeMonitorReq request)
        {
            int isPersonal = request.MerchantTypeChkBox.Personal ? 1 : 0;
            int isLegalPerson = request.MerchantTypeChkBox.LegalPerson ? 2 : 0;
            int merchantType = isPersonal + isLegalPerson;

            //商戶類型:全選或全不選都是全選
            request.MerchantType = merchantType == 3 ? 0 : merchantType;

            var result = _paymentStatisticsService.ListIncomeMonitor(request);

            return result;
        }

        /// <summary>
        /// 每日收款交易金額監控 - 匯出
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public MemoryStream ExportIncomeMonitor(QryIncomeMonitorReq request)
        {
            var list = _paymentStatisticsService.ListIncomeMonitor(request);

            if (!list.RtnData.Any()) return null;

            string functionName = "每日收款交易金額監控";

            string dateRange = $"查詢日期：{request.Date}";

            #region 標題

            string[] header = new string[]
            {
                "電支帳號","商戶名稱","商店名稱","註冊時間","MCC Code","商品類別","個人/法人","帳戶餘額","連結銀行帳戶","1天總收款額","10天帳戶餘額收款","30天帳戶餘額收款"
            };

            #endregion

            string[] arryDataGenerator(QryIncomeMonitorRes t)
            {
                var values = new string[]
                {
                    t.ICPMID,
                    t.MerchantName,
                    t.WebSiteName,
                    t.RegDate.ToString("yyyy/MM/dd HH:mm:ss"),
                    t.MCCCode.ToString(),
                    t.CommoditiyTypeName,
                    t.MerchantTypeName,
                    t.ICashAmt.ToString("N0"),
                    t.ACLinkAmt.ToString("N0"),
                    t.Total1DayAmt.ToString("N0"),
                    t.ICash10DaysAmt.ToString("N0"),
                    t.ICash30DaysAmt.ToString("N0")
                };

                return values;
            }

            MemoryStream ms= _exportDataService.GetXlsStream(header, list.RtnData, arryDataGenerator, functionName, dateRange);

            return ms;
        }

        #endregion

        #region 每日付款交易金額監控

        /// <summary>
        /// 每日付款交易金額監控 - 金流類型選單
        /// </summary>
        public IEnumerable<SelectListItem> ListPaymentTradeTypeItem()
        {
            var selectList = new List<SelectListItem>
            {
                new SelectListItem { Value = "1", Text = "帳戶餘額" },
                new SelectListItem { Value = "2", Text = "連結銀行帳號" },
                new SelectListItem { Value = "3", Text = "1天總付款額" },
                new SelectListItem { Value = "4", Text = "10天總付款額" },
                new SelectListItem { Value = "5", Text = "30天總付款額" },
                new SelectListItem { Value = "6", Text = "10天帳戶餘額付款" },
                new SelectListItem { Value = "7", Text = "30天帳戶餘額付款" },
                new SelectListItem { Value = "8", Text = "10天儲值總額" }
            };

            return selectList;
        }

        /// <summary>
        /// 每日付款交易金額監控 - 排序方式選單
        /// </summary>
        public IEnumerable<SelectListItem> ListPaymentSortTypeItem()
        {
            var selectList = new List<SelectListItem>
            {
                new SelectListItem { Value = "1", Text = "帳戶餘額" },
                new SelectListItem { Value = "2", Text = "連結銀行帳號" },
                new SelectListItem { Value = "3", Text = "1天總付款額" },
                new SelectListItem { Value = "4", Text = "10天總付款額" },
                new SelectListItem { Value = "5", Text = "30天總付款額" },
                new SelectListItem { Value = "6", Text = "10天帳戶餘額付款" },
                new SelectListItem { Value = "7", Text = "30天帳戶餘額付款" },
                new SelectListItem { Value = "8", Text = "10天儲值總額" }
            };

            return selectList;
        }

        /// <summary>
        /// 每日付款交易金額監控 - 監控類型選單
        /// </summary>
        public IEnumerable<SelectListItem> ListPaymentMonitorTypeItem()
        {
            var selectList = new List<SelectListItem>
            {
                new SelectListItem { Value = "6", Text = "觀察名單狀態" },
                new SelectListItem { Value = "7", Text = "已檢視" }
            };

            return selectList;
        }

        /// <summary>
        /// 每日付款交易金額監控 - 查詢監控記錄
        /// </summary>
        /// <param name="request">查詢條件</param>
        /// <returns></returns>
        public DataResult<List<QryPaymentMonitorRes>> ListPaymentMonitor(QryPaymentMonitorReq request)
        {
            var result = _paymentStatisticsService.ListPaymentMonitor(request);

            return result;
        }

        /// <summary>
        /// 每日付款交易金額監控 - 匯出
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public MemoryStream ExportPaymentMonitor(QryPaymentMonitorReq request)
        {
            var list = _paymentStatisticsService.ListPaymentMonitor(request);

            if (!list.RtnData.Any()) return null;

            string functionName = "每日付款交易金額監控";

            string dateRange = $"查詢日期：{request.Date}";

            #region 標題

            string[] header = new string[]
            {
                "電支帳號","商戶名稱","商店名稱","註冊時間","MCC Code","商品類別","個人/法人","帳戶餘額","連結銀行帳戶","1天總付款額","10天總付款額","30天總付款額","10天帳戶餘額付款","30天帳戶餘額付款","10天儲值總額"
            };

            #endregion

            string[] arryDataGenerator(QryPaymentMonitorRes t)
            {
                var values = new string[]
                {
                    t.ICPMID,
                    t.MerchantName,
                    t.WebSiteName,
                    t.RegDate.ToString("yyyy/MM/dd HH:mm:ss"),
                    t.MCCCode.ToString(),
                    t.CommoditiyTypeName,
                    t.MerchantTypeName,
                    t.ICashAmt.ToString("N0"),
                    t.ACLinkAmt.ToString("N0"),
                    t.Total1DayAmt.ToString("N0"),
                    t.Total10DaysAmt.ToString("N0"),
                    t.Total30DaysAmt.ToString("N0"),
                    t.ICash10DaysAmt.ToString("N0"),
                    t.ICash30DaysAmt.ToString("N0"),
                    t.ACLink10DaysAmt.ToString("N0")
                };

                return values;
            }

            MemoryStream ms = _exportDataService.GetXlsStream(header, list.RtnData, arryDataGenerator, functionName, dateRange);

            return ms;
        }

        #endregion

        #region 歷程
        /// <summary>
        /// 歷程清單
        /// </summary>
        /// <param name="mid">會員編號</param>
        /// <param name="type">監控類型(1:風管定時監控,2:提領備註,3:提領檢視,4:收款備註,5:收款檢視,6:付款監控,7:付款檢視,8:定時監控檢視)</param>
        /// <returns></returns>
        public DataResult<List<ListMonitorLogDbRes>> GetMonitorLogList(long mid, int monitorType)
        {
            return _paymentStatisticsService.GetMonitorLogList(mid, monitorType);
        }

        /// <summary>
        /// 新增歷程備註
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="Operator"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        public BaseResult AddMonitorRemarkLog<T>(string Operator, T model)
        {
            return _paymentStatisticsService.AddMonitorRemarkLog(Operator, model);
        }

        /// <summary>
        /// 新增歷程檢視
        /// </summary>
        /// <param name="Operator"></param>
        /// <param name="listMID"></param>
        /// <param name="remark"></param>
        /// <param name="monitorType"></param>
        /// <returns></returns>
        public DataResult<string> AddMonitorInspectLog(string Operator, string listMID, string remark, int monitorType)
        {
            return _paymentStatisticsService.AddMonitorInspectLog(Operator, listMID, remark, monitorType);
        }
        #endregion


        /// <summary>
        /// QueryWithdrawVM轉換ListWithdrawReq
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public DataResult<List<TimingMonitorRes>> ListTimingMonitor(QryTimingMonitorVM qryTimingMonitorVM)
        {
            return _paymentStatisticsService.ListTimingMonitor(qryTimingMonitorVM);
        }

        /// <summary>
        /// 新增定時監控歷程檢視
        /// </summary>
        /// <param name="Operator"></param>
        /// <param name="timingMonitorLogVM"></param>
        /// <returns></returns>
        public BaseResult AddTimingMonitorRemarkLog(string Operator, TimingMonitorLogVM timingMonitorLogVM)
        {
            return AddMonitorRemarkLog<AddMonitorLogDbReq>(Operator, new AddMonitorLogDbReq()
            {
                MID = timingMonitorLogVM.MerchantID,
                MonitorType = timingMonitorLogVM.Type,
                Status = timingMonitorLogVM.Status,
                Operator = Operator,
                Remark = timingMonitorLogVM.Remark
            });
        }

        /// <summary>
        /// 匯出紅利交易明細Excel報表
        /// </summary>
        /// <param name="qryBonusRes"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        public MemoryStream ExportTimingMonitorDetailExcel(List<TimingMonitorRes> qryBonusRes, string startDate)
        {
            string functionName = "定時監控";

            string dateRange = $"查詢日期：{startDate}";

            string sheetName = "定時監控";

            #region 標題
            string[] header = new string[]
            {
                "電支帳號", "商戶名稱/個人名稱", "商店名稱", "註冊時間", "MCC Code", "交易選擇日期", "交易選擇日期與前10天平均額",
                "交易連續10天交易金額與過去30天總金額","交易連續30天交易金額與過去90天總金額", "前7天退款金額與筆數"
            };
            #endregion

            HSSFWorkbook workbook = _exportDataService.GetXlsHeader(header, functionName, sheetName);

            var arryDataGenerator = _paymentStatisticsService.GetExcelTimingMonitorDateil();

            return _paymentStatisticsService.GetXlsStream<TimingMonitorRes>(header, qryBonusRes, arryDataGenerator, workbook, dateRange, sheetName, true);        
        }
    }
}
