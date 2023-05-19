using ICP.Modules.Mvc.Admin.Models.PaymentCenter;
using ICP.Modules.Mvc.Admin.Repositories;
using ICP.Infrastructure.Core.Models.Consts;
using System.Collections.Generic;
using System;
using System.Web;

namespace ICP.Modules.Mvc.Admin.Services
{
    /// <summary>
    /// 金流中心
    /// </summary>
    public class PaymentCenterService
    {
        PaymentCenterRepository _paymentCenterRepository;

        private const string _TRADESTATISTICSREPORTTITLE = "PaymentFlow_";
        private const string _DAILYREVENUEREPORTTITLE = "DailyRevenue_";
        private const string _FEESTATISTICSREPORTTITLE = "PaymentFeeReport_";
        private const string _FEESTATISTICSDETAILREPORTTITLE = "PaymentFeeDetailList_";
        private const string _XLS = ".xls";

        public PaymentCenterService(PaymentCenterRepository paymentCenterRepository)
        {
            _paymentCenterRepository = paymentCenterRepository;
        }

        #region 金流中心統計資訊
        /// <summary>
        /// 金流中心統計資訊查詢
        /// </summary>
        public List<TradeStatisticsModel> ListTradeStatistics(TradeStatisticsQueryModel model)
        {
            return _paymentCenterRepository.ListTradeStatistics(model);
        }

        /// <summary>
        /// 金流中心統計資訊明細查詢
        /// </summary>
        public List<TradeStatisticsDetailModel> ListTradeStatisticsDetail(TradeStatisticsDetailQueryModel model)
        {
            return _paymentCenterRepository.ListTradeStatisticsDetail(model);
        }

        /// <summary>
        /// 取得匯出金流中心統計資訊/明細報表檔名
        /// </summary>
        public string GetExportTradeStatisticsFileName()
        {
            string fileName = string.Empty;
            fileName = $"{_TRADESTATISTICSREPORTTITLE}{DateTime.Now.ToString("yyyyMMddHHmmdd")}{_XLS}";
            return fileName;
        }
        #endregion

        #region 每日營收統計
        /// <summary>
        /// 每日營收統計資訊查詢
        /// </summary>
        public List<DailyRevenueStatisticsModel> ListDailyRevenueStatistics(DailyRevenueStatisticsQueryModel model)
        {
            return _paymentCenterRepository.ListDailyRevenueStatistics(model);
        }

        /// <summary>
        /// 取得匯出每日營收統計報表檔名
        /// </summary>
        public string GetExportDailyRevenueStatisticsFileName()
        {
            string fileName = string.Empty;
            fileName = $"{_DAILYREVENUEREPORTTITLE}{DateTime.Now.ToString("yyyyMMddHHmmdd")}{_XLS}";
            return fileName;
        }
        #endregion

        #region 金流手續費統計(月結)
        /// <summary>
        /// 金流手續費統計資訊查詢
        /// </summary>
        public List<FeeStatisticsModel> ListFeeStatistics(FeeStatisticsQueryModel model)
        {
            if (model.StatisticsType == 2)
            {
                model.StartDate = new DateTime(model.Year, model.Month, 1);
                model.EndDate = model.StartDate.AddDays(DateTime.DaysInMonth(model.Year, model.Month));
            }
            model.MerchantQueryValue = model.MerchantQueryValue?.Trim();

            return _paymentCenterRepository.ListFeeStatistics(model);
        }

        /// <summary>
        /// 金流手續費統計資訊明細查詢
        /// </summary>
        public List<FeeStatisticsDetailModel> ListFeeStatisticsDetail(FeeStatisticsDetailQueryModel model)
        {
            model.StartDate = model.EndDate = model.AllocateDate;

            if (model.StatisticsType == 2)
            {
                model.StartDate = new DateTime(model.AllocateDate.Year, model.AllocateDate.Month, 1);
                model.EndDate = model.StartDate.AddDays(DateTime.DaysInMonth(model.AllocateDate.Year, model.AllocateDate.Month));
            }
            return _paymentCenterRepository.ListFeeStatisticsDetail(model);
        }

        /// <summary>
        /// 取得匯出金流手續費統計報表檔名
        /// </summary>
        public string GetExportFeeStatisticsFileName()
        {
            string fileName = string.Empty;
            fileName = $"{_FEESTATISTICSREPORTTITLE}{DateTime.Now.ToString("yyyyMMddHHmmdd")}{_XLS}";
            return fileName;
        }

        /// <summary>
        /// 取得匯出金流手續費統計明細報表檔名
        /// </summary>
        public string GetExportFeeStatisticsDetailFileName()
        {
            string fileName = string.Empty;
            fileName = $"{_FEESTATISTICSDETAILREPORTTITLE}{DateTime.Now.ToString("yyyyMMddHHmmdd")}{_XLS}";
            return fileName;
        }
        #endregion

        #region 共用
        /// <summary>
        /// 匯出報表
        /// FileFormat : Excel
        /// </summary>
        public void ExportExcelFile(HttpResponseBase response, string headerName, string headerValue, byte[] file)
        {
            response.Clear();
            response.AddHeader(headerName, headerValue);
            response.ContentType = MimeTypes.ApplicationOctetStream;
            response.OutputStream.Write(file, 0, file.Length);
            response.OutputStream.Flush();
            response.OutputStream.Close();
            response.Flush();
            response.End();
        }
        #endregion
    }
}
