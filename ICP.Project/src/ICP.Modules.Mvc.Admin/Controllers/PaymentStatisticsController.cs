using ICP.Infrastructure.Core.Models;
using ICP.Modules.Mvc.Admin.Attributes;
using ICP.Modules.Mvc.Admin.Commands;
using ICP.Modules.Mvc.Admin.Enums;
using ICP.Modules.Mvc.Admin.Models.Finance.TradeDetail;
using ICP.Modules.Mvc.Admin.Models.PaymentStatistics;
using ICP.Modules.Mvc.Admin.Models.PaymentStatistics.IncomeMonitor;
using ICP.Modules.Mvc.Admin.Models.PaymentStatistics.PaymentMonitor;
using ICP.Modules.Mvc.Admin.Models.PaymentStatistics.TimingMonitor;
using System;
using System.Collections.Generic;
using System.IO;
using System.Web.Mvc;

namespace ICP.Modules.Mvc.Admin.Controllers
{
    public class PaymentStatisticsController : BaseAdminController
    {
        private PaymentStatisticsCommand _paymentStatisticsCommand = null;
        private FinanceCommand _financeCommand = null;

        public PaymentStatisticsController(
            PaymentStatisticsCommand paymentStatisticsCommand,
            FinanceCommand financeCommand
        )
        {
            _paymentStatisticsCommand = paymentStatisticsCommand;
            _financeCommand = financeCommand;
        }

        #region 每日提領金額監控

        #region 提領監控清單
        /// <summary>
        /// 提領監控主頁
        /// </summary>
        /// <returns></returns>
        [UserLoginAuth(MappingMethod = "WithdrawMonitor", Action = MappingMethodAction.Query)]
        public ActionResult WithdrawMonitor()
        {
            QueryWithdrawVM model = new QueryWithdrawVM();
            model.TradeTypeList = _paymentStatisticsCommand.ListWithdrawTradeTypeItem();
            model.SortTypeList = _paymentStatisticsCommand.ListWithdrawSortTypeItem();

            return View(model);
        }

        /// <summary>
        /// 提領監控清單
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        //[HttpPost]
        [UserLoginAuth(MappingMethod = "WithdrawMonitor", Action = MappingMethodAction.Query)]
        public ActionResult ListWithdraw(QueryWithdrawVM model)
        {
            if (!ModelState.IsValid)
            {
                return RedirectAndAlert(Url.Action("WithdrawMonitor"), "資料錯誤");
            }

            ListWithdrawDbReq req = new ListWithdrawDbReq();
            List<ListWithdrawDbRes> list = new List<ListWithdrawDbRes>();

            var mappingResult = _paymentStatisticsCommand.MappingToListWithdrawReq(model);
            if (!mappingResult.IsSuccess)
            {
                return RedirectAndAlert(Url.Action("WithdrawMonitor"), mappingResult.RtnMsg);
            }
            req = mappingResult.RtnData;

            var listResult = _paymentStatisticsCommand.ListWithdraw(req);
            if (!listResult.IsSuccess)
            {
                return RedirectAndAlert(Url.Action("WithdrawInspect"), listResult.RtnMsg);
            }
            list = listResult.RtnData;

            ViewBag.SelectDate = model.StartDate.Replace("-", "/");

            return PagedListView(list, req);
        }
        #endregion

        #region 提領監控歷程

        /// <summary>
        /// 歷程清單
        /// </summary>
        /// <param name="id"></param>
        /// <param name="icpMid"></param>
        /// <param name="merchantName"></param>
        /// <returns></returns>
        public ActionResult ListWithdrawLog(int id, string icpMid, string merchantName)
        {
            WithdrawLogVM model = new WithdrawLogVM();
            model.MID = id;
            model.ICPMID = icpMid;
            model.MerchantName = merchantName;
            model.SelectTypeList = _paymentStatisticsCommand.ListWithdrawSelectTypeItem();

            var result = _paymentStatisticsCommand.GetMonitorLogList(id, 20);
            if (!result.IsSuccess)
            {
                return HttpNotFound();
            }

            model.LogList = result.RtnData;

            return View(model);
        }

        /// <summary>
        /// 歷程清單POST
        /// </summary>
        /// <param name="model"></param>
        /// <param name="postType">Add:新增備註, Query:歷程清單篩選</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult ListWithdrawLog(WithdrawLogVM model, string postType)
        {
            model.SelectTypeList = _paymentStatisticsCommand.ListWithdrawSelectTypeItem();
            model.LogList = new List<ListMonitorLogDbRes>();

            if (postType == "Add")
            {
                var addResult = _paymentStatisticsCommand.AddMonitorRemarkLog(CurrentUser.Account, model);
                if (!addResult.IsSuccess)
                {
                    TempData["RtnMsg"] = addResult.RtnMsg;
                    return View(model);
                }
            }

            var getResult = _paymentStatisticsCommand.GetMonitorLogList(model.MID, model.SelectType);
            if (!getResult.IsSuccess)
            {
                TempData["RtnMsg"] = getResult.RtnMsg;
                return View(model);
            }

            model.LogList = getResult.RtnData;

            return View(model);
        }

        /// <summary>
        /// 新增歷程檢視
        /// </summary>
        /// <param name="listMID"></param>
        /// <param name="startDate"></param>
        /// <param name="monitorType"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult AddWithdrawInspectLog(string listMID, string startDate)
        {
            var result = _paymentStatisticsCommand.AddMonitorInspectLog(CurrentUser.Account, listMID, startDate, 3);
            return Json(result);
        }
        #endregion

        #endregion

        #region 每日收款交易金額監控

        /// <summary>
        /// 每日收款交易金額監控 - 初始
        /// </summary>
        /// <returns></returns>
        [UserLoginAuth(MappingMethod = "IncomeMonitor", Action = MappingMethodAction.Query)]
        public ActionResult IncomeMonitor()
        {
            QryIncomeMonitorReq model = new QryIncomeMonitorReq()
            {
                //金流類型選單
                TradeTypeList = _paymentStatisticsCommand.ListIncomeTradeTypeItem(),
                //排序方式選單
                SortTypeList = _paymentStatisticsCommand.ListIncomeSortTypeItem()
            };

            return View(model);
        }

        /// <summary>
        /// 每日收款交易金額監控 - 查詢
        /// </summary>
        /// <param name="request">查詢條件</param>
        /// <returns></returns>
        [HttpPost]
        [UserLoginAuth(MappingMethod = "IncomeMonitor", Action = MappingMethodAction.Query)]
        public ActionResult ListIncomeMonitor(QryIncomeMonitorReq request)
        {
            //查詢結果
            var result = _paymentStatisticsCommand.ListIncomeMonitor(request);

            if (!result.IsSuccess)
            {
                return RedirectAndAlert(Url.Action("IncomeMonitor"), result.RtnMsg);
            }

            ViewBag.SelectDate = request.Date.Replace("-", "/");

            return PagedListView(result.RtnData, request);
        }

        /// <summary>
        /// 每日收款交易金額監控 - 匯出Excel
        /// </summary>
        /// <param name="request">查詢條件</param>
        /// <returns></returns>
        [UserLoginAuth(MappingMethod = "IncomeMonitor", Action = MappingMethodAction.Export)]
        public ActionResult IncomeMonitorExportExcel(QryIncomeMonitorReq request)
        {
            request.PageSize = 0;//撈全部

            MemoryStream ms = _paymentStatisticsCommand.ExportIncomeMonitor(request);

            if (ms == null)
            {
                return RedirectAndAlert(Url.Action("IncomeMonitor"), "查無資料");
            }

            string fileName = Server.UrlPathEncode($"每日收款交易金額監控_{DateTime.Now:yyyyMMddHHmmss}.xls");

            ms.Flush();
            ms.Position = 0;

            return File(ms, "application/ms-excel", fileName);
        }

        /// <summary>
        /// 每日收款交易金額監控 - 新增檢視記錄
        /// </summary>
        /// <param name="listMID">會員編號</param>
        /// <param name="request">查詢條件</param>
        /// <returns></returns>
        [HttpPost]
        [UserLoginAuth(MappingMethod = "IncomeMonitor", Action = MappingMethodAction.Edit)]
        public JsonResult AddIncomeInspectLog(string listMID, QryIncomeMonitorReq request)
        {
            int monitorType = 5;//收款檢視固定5

            //新增記錄
            var logResult = _paymentStatisticsCommand.AddMonitorInspectLog(CurrentUser.Account, listMID, request.Date.Replace("-", "/"), monitorType);

            var result = new DataResult<object>()
            {
                RtnCode = logResult.RtnCode,
                RtnMsg = logResult.RtnMsg
            };

            if (logResult.RtnCode == 1)
            {
                result.RtnData = request;
            }
            else
            {
                result.RtnData = logResult.RtnData;
            }

            return Json(result);
        }

        /// <summary>
        /// 每日收款交易金額監控(備註歷程) - Dialog
        /// </summary>
        /// <param name="mid">會員編號</param>
        /// <param name="icpMid">電支帳號</param>
        /// <param name="merchantName">商戶名稱/個人名稱</param>
        /// <returns></returns>
        [UserLoginAuth(MappingMethod = "IncomeMonitor", Action = MappingMethodAction.Query)]
        public ActionResult IncomeMonitorLog(int id, string icpMid, string merchantName)
        {
            int monitorType = 40;//預設

            var result = _paymentStatisticsCommand.GetMonitorLogList(id, monitorType);

            if (!result.IsSuccess)
            {
                return RedirectAndAlert(Url.Action("IncomeMonitor"), result.RtnMsg);
            }

            QryIncomeMonitorLogReq model = new QryIncomeMonitorLogReq()
            {
                MID = id,
                ICPMID = icpMid,
                MerchantName = merchantName,
                //監控類型
                MonitorTypeList = _paymentStatisticsCommand.ListIncomeMonitorTypeItem(),
                //歷程記錄
                LogList = result.RtnData
            };

            return View(model);
        }

        /// <summary>
        /// 每日收款交易金額監控(備註歷程) - 查詢
        /// </summary>
        /// <param name="request">查詢條件</param>
        /// <returns></returns>
        [HttpPost]
        [UserLoginAuth(MappingMethod = "IncomeMonitor", Action = MappingMethodAction.Query)]
        public ActionResult ListIncomeMonitorLog(QryIncomeMonitorLogReq request)
        {
            //查詢
            var result = _paymentStatisticsCommand.GetMonitorLogList(request.MID, request.MonitorType);

            if (!result.IsSuccess)
            {
                return JavaScript($"window.alert('{result.RtnMsg}');");
            }

            request.MonitorTypeList = _paymentStatisticsCommand.ListIncomeMonitorTypeItem();
            request.LogList = result.RtnData;

            return View("IncomeMonitorLog", request);

        }

        /// <summary>
        /// 每日收款交易金額監控(備註歷程) - 新增備註
        /// </summary>
        /// <param name="request">更新條件</param>
        /// <returns></returns>
        [HttpPost]
        [UserLoginAuth(MappingMethod = "IncomeMonitor", Action = MappingMethodAction.Edit)]
        public ActionResult AddIncomeMonitorRemark(QryIncomeMonitorLogReq request)
        {
            request.MonitorType = 4;//收款備註固定4

            //新增備註
            var addResult = _paymentStatisticsCommand.AddMonitorRemarkLog(CurrentUser.Account, request);
            if (!addResult.IsSuccess)
            {
                return JavaScript($"window.alert('{addResult.RtnMsg}');");
            }

            //重新查詢
            var result = _paymentStatisticsCommand.GetMonitorLogList(request.MID, request.MonitorType);

            if (!result.IsSuccess)
            {
                return JavaScript($"window.alert('{result.RtnMsg}');");
            }

            request.MonitorTypeList = _paymentStatisticsCommand.ListIncomeMonitorTypeItem();
            request.LogList = result.RtnData;

            return View("IncomeMonitorLog", request);
        }

        #endregion

        #region 每日付款交易金額監控

        /// <summary>
        /// 每日付款交易金額監控 - 初始
        /// </summary>
        /// <returns></returns>
        [UserLoginAuth(MappingMethod = "PaymentMonitor", Action = MappingMethodAction.Query)]
        public ActionResult PaymentMonitor()
        {
            QryPaymentMonitorReq model = new QryPaymentMonitorReq()
            {
                //金流類型選單
                TradeTypeList = _paymentStatisticsCommand.ListPaymentTradeTypeItem(),
                //排序方式選單
                SortTypeList = _paymentStatisticsCommand.ListPaymentSortTypeItem()
            };

            return View(model);
        }

        /// <summary>
        /// 每日付款交易金額監控 - 查詢
        /// </summary>
        /// <param name="request">查詢條件</param>
        /// <returns></returns>
        [HttpPost]
        [UserLoginAuth(MappingMethod = "PaymentMonitor", Action = MappingMethodAction.Query)]
        public ActionResult ListPaymentMonitor(QryPaymentMonitorReq request)
        {
            //查詢結果
            var result = _paymentStatisticsCommand.ListPaymentMonitor(request);

            if (!result.IsSuccess)
            {
                return RedirectAndAlert(Url.Action("PaymentMonitor"), result.RtnMsg);
            }

            ViewBag.SelectDate = request.Date.Replace("-", "/");

            return PagedListView(result.RtnData, request);
        }

        /// <summary>
        /// 每日付款交易金額監控 - 匯出Excel
        /// </summary>
        /// <param name="request">查詢條件</param>
        /// <returns></returns>
        [UserLoginAuth(MappingMethod = "PaymentMonitor", Action = MappingMethodAction.Export)]
        public ActionResult PaymentMonitorExportExcel(QryPaymentMonitorReq request)
        {
            request.PageSize = 0;//撈全部

            MemoryStream ms = _paymentStatisticsCommand.ExportPaymentMonitor(request);

            if (ms == null)
            {
                return RedirectAndAlert(Url.Action("PaymentMonitor"), "查無資料");
            }

            string fileName = Server.UrlPathEncode($"每日付款交易金額監控_{DateTime.Now:yyyyMMddHHmmss}.xls");

            ms.Flush();
            ms.Position = 0;

            return File(ms, "application/ms-excel", fileName);
        }

        /// <summary>
        /// 每日付款交易金額監控 - 新增檢視記錄
        /// </summary>
        /// <param name="listMID">會員編號</param>
        /// <param name="request">查詢條件</param>
        /// <returns></returns>
        [HttpPost]
        [UserLoginAuth(MappingMethod = "PaymentMonitor", Action = MappingMethodAction.Edit)]
        public JsonResult AddPaymentInspectLog(string listMID, QryPaymentMonitorReq request)
        {
            int monitorType = 7;//付款檢視固定7

            //新增記錄
            var logResult = _paymentStatisticsCommand.AddMonitorInspectLog(CurrentUser.Account, listMID, request.Date, monitorType);

            var result = new DataResult<object>()
            {
                RtnCode = logResult.RtnCode,
                RtnMsg = logResult.RtnMsg
            };

            if (logResult.RtnCode == 1)
            {
                result.RtnData = request;
            }
            else
            {
                result.RtnData = logResult.RtnData;
            }

            return Json(result);
        }

        /// <summary>
        /// 每日付款交易金額監控(備註歷程) - Dialog
        /// </summary>
        /// <param name="mid">會員編號</param>
        /// <param name="icpMid">電支帳號</param>
        /// <param name="merchantName">商戶名稱/個人名稱</param>
        /// <returns></returns>
        [UserLoginAuth(MappingMethod = "PaymentMonitor", Action = MappingMethodAction.Query)]
        public ActionResult PaymentMonitorLog(int id, string icpMid, string merchantName)
        {
            int monitorType = 6;//預設

            var result = _paymentStatisticsCommand.GetMonitorLogList(id, monitorType);

            if (!result.IsSuccess)
            {
                return RedirectAndAlert(Url.Action("PaymentMonitor"), result.RtnMsg);
            }

            QryPaymentMonitorLogReq model = new QryPaymentMonitorLogReq()
            {
                MID = id,
                ICPMID = icpMid,
                MerchantName = merchantName,
                //監控類型
                MonitorTypeList = _paymentStatisticsCommand.ListPaymentMonitorTypeItem(),
                //歷程記錄
                LogList = result.RtnData
            };

            return View(model);
        }

        /// <summary>
        /// 每日付款交易金額監控(備註歷程) - 查詢
        /// </summary>
        /// <param name="request">查詢條件</param>
        /// <returns></returns>
        [HttpPost]
        [UserLoginAuth(MappingMethod = "PaymentMonitor", Action = MappingMethodAction.Query)]
        public ActionResult ListPaymentMonitorLog(QryPaymentMonitorLogReq request)
        {
            //查詢
            var result = _paymentStatisticsCommand.GetMonitorLogList(request.MID, request.MonitorType);

            if (!result.IsSuccess)
            {
                return JavaScript($"window.alert('{result.RtnMsg}');");
            }

            request.MonitorTypeList = _paymentStatisticsCommand.ListPaymentMonitorTypeItem();
            request.LogList = result.RtnData;

            return View("PaymentMonitorLog", request);

        }

        /// <summary>
        /// 每日付款交易金額監控(備註歷程) - 新增備註
        /// </summary>
        /// <param name="request">更新條件</param>
        /// <returns></returns>
        [HttpPost]
        [UserLoginAuth(MappingMethod = "PaymentMonitor", Action = MappingMethodAction.Edit)]
        public ActionResult AddPaymentMonitorRemark(QryPaymentMonitorLogReq request)
        {
            request.MonitorType = 6;//付款觀察名單固定6

            //新增備註
            var addResult = _paymentStatisticsCommand.AddMonitorRemarkLog(CurrentUser.Account, request);
            if (!addResult.IsSuccess)
            {
                return JavaScript($"window.alert('{addResult.RtnMsg}');");
            }

            //重新查詢
            var result = _paymentStatisticsCommand.GetMonitorLogList(request.MID, request.MonitorType);

            if (!result.IsSuccess)
            {
                return JavaScript($"window.alert('{result.RtnMsg}');");
            }

            request.MonitorTypeList = _paymentStatisticsCommand.ListPaymentMonitorTypeItem();
            request.LogList = result.RtnData;

            return View("PaymentMonitorLog", request);
        }

        #endregion


        #region 定時監控
        #region 首頁        
        /// <summary>
        /// 定時監控首頁
        /// </summary>
        /// <returns></returns>
        [UserLoginAuth(MappingMethod = "TimingMonitor", Action = MappingMethodAction.Query)]
        public ActionResult TimingMonitor()
        {
            return View(new QryTimingMonitorVM());
        }
        #endregion
    
        /// <summary>
        /// 定時監控查詢結果
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [HttpPost]
        [UserLoginAuth(MappingMethod = "TimingMonitor", Action = MappingMethodAction.Query)]
        public ActionResult ListTimingMonitorResult(QryTimingMonitorVM query)
        {           
            ViewBag.StartDate = query.StartDate.ToString("yyyy/MM/dd");
           
            List<TimingMonitorRes> list = new List<TimingMonitorRes>();

            var result = _paymentStatisticsCommand.ListTimingMonitor(query);
            if (!result.IsSuccess)
            {
                return RedirectAndAlert(Url.Action("TimingMonitor"), result.RtnMsg);
            }
           
            list = result.RtnData;

            return PagedListView(list, query);
        }

        /// <summary>
        /// 新增定時監控歷程檢視
        /// </summary>
        /// <param name="listMID"></param>
        /// <param name="startDate"></param>
        /// <param name="monitorType"></param>
        /// <returns></returns>
        [HttpPost]
        [UserLoginAuth(MappingMethod = "TimingMonitor", Action = MappingMethodAction.Add)]
        public JsonResult AddTimingMonitorLog(string listMID, string startDate)
        {
            var result = _paymentStatisticsCommand.AddMonitorInspectLog(CurrentUser.Account, listMID, startDate.Replace('-', '/'), 8);
            return Json(result);
        }

        /// <summary>
        /// 定時監控歷程清單
        /// </summary>
        /// <param name="id"></param>
        /// <param name="merchantName"></param>
        /// <returns></returns>
        [UserLoginAuth(MappingMethod = "TimingMonitor", Action = MappingMethodAction.Query)]
        public ActionResult ListTimingMonitorLog(long merchantid, string icpMID, string merchantName, int status)
        {
            TimingMonitorLogVM model = new TimingMonitorLogVM();
            model.MerchantID = merchantid;
            model.ICPMID = icpMID;
            model.MerchantName = merchantName;
            model.Status = status;

            var result = _paymentStatisticsCommand.GetMonitorLogList(merchantid, 1/* 預設列出觀察名單 */);
            if (!result.IsSuccess)
            {
                return HttpNotFound();
            }

            model.LogList = result.RtnData;

            return View(model);
        }


        /// <summary>
        /// 定時監控歷程清單POST
        /// </summary>
        /// <param name="model"></param>
        /// <param name="postType">Add:新增備註, Query:歷程清單篩選</param>
        /// <returns></returns>
        [HttpPost]
        [UserLoginAuth(MappingMethod = "TimingMonitor", Action = MappingMethodAction.Add)]
        public ActionResult ListTimingMonitorLog(TimingMonitorLogVM model, string postType)
        {            
            model.LogList = new List<ListMonitorLogDbRes>();
            ViewBag.RtnMsg = "OK";

            if (postType == "Add")
            {
                var addResult = _paymentStatisticsCommand.AddTimingMonitorRemarkLog(CurrentUser.Account, model);
                if (!addResult.IsSuccess)
                {
                    ViewBag.RtnMsg = addResult.RtnMsg;
                    return View(model);
                }
            }

            var getResult = _paymentStatisticsCommand.GetMonitorLogList(model.MerchantID, model.SelectType);
            if (!getResult.IsSuccess)
            {
                ViewBag.RtnMsg = getResult.RtnMsg;
                return View(model);
            }

            model.LogList = getResult.RtnData;
            ViewBag.SelectType = model.SelectType;

            return View(model);
        }

        #region 匯出Excel檔
        /// <summary>
        /// 匯出定時監控Excel報表
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [UserLoginAuth(MappingMethod = "TimingMonitor", Action = MappingMethodAction.Export)]
        public ActionResult ExportTimingMonitorExcel(QryTimingMonitorVM query)
        {
            var result = _paymentStatisticsCommand.ListTimingMonitor(query);

            if (!result.IsSuccess)
            {
                return null;
            }

            var file = _paymentStatisticsCommand.ExportTimingMonitorDetailExcel(result.RtnData, query.StartDate.ToString("yyyy/MM/dd"));

            file.Flush();
            file.Position = 0;

            return File(file, "application/ms-excel", $"定時監控_{DateTime.Now.ToString("yyyyMMdd")}_{DateTime.Now.ToString("HHmmss")}.xls");
        }

        #endregion

        /// <summary>
        /// 實質交易明細查詢 - 初始
        /// </summary>
        /// <returns></returns>
        [UserLoginAuth(MappingMethod = "TimingMonitor", Action = MappingMethodAction.Query)]
        public ActionResult TimingMonitorTradeDetail(string icpMID, string startDate, string endDate)
        {
            QryTradeDetailReq model = _financeCommand.SetTradeDetailDefaultSetting();

            //依參數條件初始           
            DateTime eDt = DateTime.Now;
            DateTime.TryParse(endDate, out eDt);

            DateTime sDt = DateTime.Now.AddMonths(-1);
            DateTime.TryParse(startDate, out sDt);

            model.DateStart = sDt.ToString("yyyy-MM-dd");
            model.DateEnd = eDt.ToString("yyyy-MM-dd");

            model.ICPMIDType = 1;
            model.ICPMID = !string.IsNullOrWhiteSpace(icpMID) ? icpMID : "";

            return View("../Finance/TradeDetail", model);
        }

        #endregion
    }
}
