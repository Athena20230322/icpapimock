using ICP.Modules.Mvc.Admin.Attributes;
using ICP.Modules.Mvc.Admin.Commands;
using ICP.Modules.Mvc.Admin.Enums;
using ICP.Modules.Mvc.Admin.Models.Finance;
using ICP.Modules.Mvc.Admin.Models.Finance.MerchantTradeDetail;
using ICP.Modules.Mvc.Admin.Models.Finance.TradeDetail;
using ICP.Modules.Mvc.Admin.Models.PaymentCenter;
using System;
using System.Collections.Generic;
using System.IO;
using System.Web.Mvc;

namespace ICP.Modules.Mvc.Admin.Controllers
{
    public class FinanceController : BaseAdminController
    {
        private FinanceCommand _financeCommand = null;
        PaymentCenterCommand _paymentCenterCommand;

        public FinanceController(FinanceCommand financeCommand, PaymentCenterCommand paymentCenterCommand)
        {
            _financeCommand = financeCommand;
            _paymentCenterCommand = paymentCenterCommand;
        }

        #region 實質交易明細查詢

        /// <summary>
        /// 實質交易明細查詢 - 初始
        /// </summary>
        /// <returns></returns>
        [UserLoginAuth(MappingMethod = "TradeDetail", Action = MappingMethodAction.Query)]
        public ActionResult TradeDetail(string id,string date)
        {
            QryTradeDetailReq model = _financeCommand.SetTradeDetailDefaultSetting();

            //依參數條件初始
            if (!string.IsNullOrWhiteSpace(id) && !string.IsNullOrWhiteSpace(date)) {

                DateTime dt = DateTime.TryParse(date, out DateTime dateStart) ? dateStart : DateTime.Now;

                model.DateEnd = dt.ToString("yyyy-MM-dd");
                model.DateStart = dt.AddDays(-3).ToString("yyyy-MM-dd");
                model.ICPMIDType = 1;
                model.ICPMID = id;
            }

            return View(model);
        }

        /// <summary>
        /// 實質交易明細查詢 - 查詢
        /// </summary>
        /// <param name="request">查詢條件</param>
        /// <returns></returns>
        [HttpPost]
        [UserLoginAuth(MappingMethod = "TradeDetail", Action = MappingMethodAction.Query)]
        public ActionResult ListTradeDetail(QryTradeDetailReq request)
        {
            //查詢結果
            var result = _financeCommand.ListTradeDetail(request);

            if (!result.IsSuccess)
            {
                return RedirectAndAlert(Url.Action("TradeDetail"), result.RtnMsg);
            }

            if (result.RtnData.Count > 0)
            {
                ViewBag.IsLastPage = (int)Math.Ceiling((decimal)result.RtnData[0].TotalCount / request.PageSize) == request.PageNo;
            }

            return PagedListView(result.RtnData, request);
        }

        /// <summary>
        /// 實質交易明細查詢 - 匯出Excel
        /// </summary>
        /// <param name="request">查詢條件</param>
        /// <returns></returns>
        [UserLoginAuth(MappingMethod = "TradeDetail", Action = MappingMethodAction.Export)]
        public ActionResult TradeDetailExportExcel(QryTradeDetailReq request)
        {
            request.PageSize = 0;//撈全部

            MemoryStream ms = _financeCommand.ExportTradeDetail(request);

            if (ms == null)
            {
                return RedirectAndAlert(Url.Action("TradeDetail"), "查無資料");
            }

            string fileName = Server.UrlPathEncode($"TradeDetail_{DateTime.Now:yyyyMMddHHmmss}.xls");

            ms.Flush();
            ms.Position = 0;

            return File(ms, "application/ms-excel", fileName);
        }

        #endregion

        #region 特店帳務進出明細

        /// <summary>
        /// 特店帳務進出明細 - 初始
        /// </summary>
        /// <returns></returns>
        [UserLoginAuth(MappingMethod = "MerchantTradeDetail", Action = MappingMethodAction.Query)]
        public ActionResult MerchantTradeDetail()
        {
            QryMerchantTradeDetailReq model = new QryMerchantTradeDetailReq()
            {
                UserTypeList = _financeCommand.ListMerchantTradeDetailUserTypeItem(),
                TradeModeList = _financeCommand.ListMerchantTradeDetailTradeModeItem(),
                PaymentTypeList = _financeCommand.ListMerchantTradeDetailPaymentTypeItem(),
                PaymentSubTypeList = _financeCommand.ListMerchantTradeDetailPaymentSubTypeItem()
            };

            return View(model);
        }

        /// <summary>
        /// 交易類型選單
        /// </summary>
        /// <param name="tradeModeID">帳務類型</param>
        /// <returns></returns>
        [HttpPost]
        [UserLoginAuth(MappingMethod = "MerchantTradeDetail", Action = MappingMethodAction.Query)]
        public JsonResult BindPaymentTypeList(int tradeModeID)
        {
            var result = new
            {
                PaymentTypeList = _financeCommand.ListMerchantTradeDetailPaymentTypeItem(tradeModeID),
                PaymentSubTypeList = _financeCommand.ListMerchantTradeDetailPaymentSubTypeItem()
            };

            return Json(result);
        }

        /// <summary>
        /// 交易子類型選單
        /// </summary>
        /// <param name="paymentTypeID">交易類型</param>
        /// <returns></returns>
        [HttpPost]
        [UserLoginAuth(MappingMethod = "MerchantTradeDetail", Action = MappingMethodAction.Query)]
        public JsonResult BindPaymentSubTypeList(int paymentTypeID)
        {
            var result = new
            {
                PaymentSubTypeList = _financeCommand.ListMerchantTradeDetailPaymentSubTypeItem(paymentTypeID)
            };

            return Json(result);
        }

        /// <summary>
        /// 特店帳務進出明細 - 查詢
        /// </summary>
        /// <param name="request">查詢條件</param>
        /// <returns></returns>
        [HttpPost]
        [UserLoginAuth(MappingMethod = "MerchantTradeDetail", Action = MappingMethodAction.Query)]
        public ActionResult ListMerchantTradeDetail(QryMerchantTradeDetailReq request)
        {
            //查詢結果
            var result = _financeCommand.ListMerchantTradeDetail(request);

            if (!result.IsSuccess)
            {
                return RedirectAndAlert(Url.Action("MerchantTradeDetail"), result.RtnMsg);
            }

            if (result.RtnData.Count > 0)
            {
                ViewBag.IsLastPage = (int)Math.Ceiling((decimal)result.RtnData[0].TotalCount / request.PageSize) == request.PageNo;
            }

            return PagedListView(result.RtnData, request);
        }

        /// <summary>
        /// 特店帳務進出明細 - 匯出Excel
        /// </summary>
        /// <param name="request">查詢條件</param>
        /// <returns></returns>
        [UserLoginAuth(MappingMethod = "MerchantTradeDetail", Action = MappingMethodAction.Export)]
        public ActionResult MerchantTradeDetailExportExcel(QryMerchantTradeDetailReq request)
        {
            request.PageSize = 0;//撈全部

            MemoryStream ms = _financeCommand.ExportMerchantTradeDetail(request);

            if (ms == null)
            {
                return RedirectAndAlert(Url.Action("MerchantTradeDetail"), "查無資料");
            }

            string fileName = Server.UrlPathEncode($"AccountDetail_{DateTime.Now:yyyyMMddHHmmss}.xls");

            ms.Flush();
            ms.Position = 0;

            return File(ms, "application/ms-excel", fileName);
        }

        #endregion

        #region 金流中心統計資訊
        /// <summary>
        /// 金流中心統計資訊查詢條件輸入
        /// </summary>
        [UserLoginAuth(MappingMethod = "TradeStatistics", Action = MappingMethodAction.Query)]
        public ActionResult TradeStatistics()
        {
            var viewModel = new TradeStatisticsQueryModel
            {
                StartDate = DateTime.Now.AddDays(-1),
                EndDate = DateTime.Now
            };

            return View(viewModel);
        }

        /// <summary>
        /// 金流中心統計資訊查詢
        /// </summary>
        [HttpPost]
        [UserLoginAuth(MappingMethod = "TradeStatistics", Action = MappingMethodAction.Query)]
        public ActionResult TradeStatisticsQuery(TradeStatisticsQueryModel model)
        {
            var list = _paymentCenterCommand.ListTradeStatistics(model);

            if (list.Count > 0)
            {
                ViewBag.IsLastPage = (int)Math.Ceiling((decimal)list[0].TotalCount / model.PageSize) == model.PageNo;
            }

            return PagedListView(list, model);
        }

        /// <summary>
        /// 金流中心統計資訊明細查詢
        /// </summary>
        //[HttpPost]
        [UserLoginAuth(MappingMethod = "TradeStatistics", Action = MappingMethodAction.Query)]
        public ActionResult TradeStatisticsDetailQuery(TradeStatisticsDetailQueryModel model)
        {
            var detail = _paymentCenterCommand.ListTradeStatisticsDetail(model);

            ViewBag.QueryModel = model;

            return PagedListView(detail, model);
        }

        /// <summary>
        /// 匯出金流中心統計資訊報表
        /// </summary>
        public ActionResult ExportTradeStatistics(TradeStatisticsQueryModel model)
        {
            _paymentCenterCommand.ExportTradeStatistics(Response, model);
            return new EmptyResult();
        }

        /// <summary>
        /// 匯出金流中心統計資訊明細報表
        /// </summary>
        public ActionResult ExportTradeStatisticsDetail(TradeStatisticsDetailQueryModel model)
        {
            _paymentCenterCommand.ExportTradeStatisticsDetail(Response, model);
            return new EmptyResult();
        }
        #endregion

        #region 每日營收統計
        /// <summary>
        /// 每日營收統計查詢條件輸入
        /// </summary>
        [UserLoginAuth(MappingMethod = "DailyRevenueStatistics", Action = MappingMethodAction.Query)]
        public ActionResult DailyRevenueStatistics()
        {
            var viewModel = new DailyRevenueStatisticsQueryModel
            {
                StartDate = DateTime.Now.AddDays(-1),
                EndDate = DateTime.Now
            };

            return View(viewModel);
        }

        /// <summary>
        /// 每日營收統計資訊查詢
        /// </summary>
        [HttpPost]
        [UserLoginAuth(MappingMethod = "DailyRevenueStatistics", Action = MappingMethodAction.Query)]
        public ActionResult DailyRevenueStatisticsQuery(DailyRevenueStatisticsQueryModel model)
        {
            var list = _paymentCenterCommand.ListDailyRevenueStatistics(model);

            if (list.Count > 0)
            {
                ViewBag.IsLastPage = (int)Math.Ceiling((decimal)list[0].TotalCount / model.PageSize) == model.PageNo;
            }

            return PagedListView(list, model);
        }

        /// <summary>
        /// 匯出金流中心統計資訊報表
        /// </summary>
        public ActionResult ExportDailyRevenueStatistics(DailyRevenueStatisticsQueryModel model)
        {
            _paymentCenterCommand.ExportDailyRevenueStatistics(Response, model);
            return new EmptyResult();
        }
        #endregion

        #region 金流手續費統計(月結)
        /// <summary>
        /// 金流手續費統計(月結)查詢條件輸入
        /// </summary>
        [UserLoginAuth(MappingMethod = "FeeStatistics", Action = MappingMethodAction.Query)]
        public ActionResult FeeStatistics()
        {
            var viewModel = new FeeStatisticsQueryModel
            {
                StatisticsType = 1,
                MerchantQueryType = 1,
                StartDate = DateTime.Now.AddDays(-1),
                EndDate = DateTime.Now,
                Year = DateTime.Now.Year,
                Month = DateTime.Now.Month,
                TradeModeID = 0,

                MerchantQueryList = _paymentCenterCommand.GetMerchantQueryList(),
                YearList = _paymentCenterCommand.GetYearList(),
                MonthList = _paymentCenterCommand.GetMonthList(),
                TradeModeList = _paymentCenterCommand.GetTradeModeList()
            };

            return View(viewModel);
        }

        /// <summary>
        /// 金流手續費統計(月結)資訊查詢
        /// </summary>
        [HttpPost]
        [UserLoginAuth(MappingMethod = "FeeStatistics", Action = MappingMethodAction.Query)]
        public ActionResult FeeStatisticsQuery(FeeStatisticsQueryModel model)
        {
            var list = _paymentCenterCommand.ListFeeStatistics(model);

            if (list.Count > 0)
            {
                ViewBag.IsLastPage = (int)Math.Ceiling((decimal)list[0].TotalCount / model.PageSize) == model.PageNo;
            }

            return PagedListView(list, model);
        }

        /// <summary>
        /// 金流手續費統計(月結)資訊查詢
        /// </summary>
        [UserLoginAuth(MappingMethod = "FeeStatistics", Action = MappingMethodAction.Query)]
        public ActionResult FeeStatisticsDetailQuery(FeeStatisticsDetailQueryModel model)
        {
            var detail = _paymentCenterCommand.ListFeeStatisticsDetail(model);

            ViewBag.QueryModel = model;

            return PagedListView(detail, model);
        }

        /// <summary>
        /// 匯出金流手續費統計資訊報表
        /// </summary>
        public ActionResult ExportFeeStatistics(FeeStatisticsQueryModel model)
        {
            _paymentCenterCommand.ExportFeeStatistics(Response, model);
            return new EmptyResult();
        }

        /// <summary>
        /// 匯出金流手續費統計資訊報表 - 明細
        /// </summary>
        public ActionResult ExportFeeStatisticsDetail(FeeStatisticsDetailQueryModel model)
        {
            _paymentCenterCommand.ExportFeeStatisticsDetail(Response, model);
            return new EmptyResult();
        }
        #endregion
    }
}
