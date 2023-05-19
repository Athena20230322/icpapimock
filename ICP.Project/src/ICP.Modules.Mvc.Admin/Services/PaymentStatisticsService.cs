using AutoMapper;
using ICP.Infrastructure.Abstractions.Logging;
using ICP.Infrastructure.Core.Extensions;
using ICP.Infrastructure.Core.Models;
using ICP.Modules.Mvc.Admin.Models.PaymentStatistics;
using ICP.Modules.Mvc.Admin.Models.PaymentStatistics.IncomeMonitor;
using ICP.Modules.Mvc.Admin.Models.PaymentStatistics.PaymentMonitor;
using ICP.Modules.Mvc.Admin.Models.PaymentStatistics.TimingMonitor;
using ICP.Modules.Mvc.Admin.Repositories;
using NPOI.HSSF.UserModel;
using System;
using System.Collections.Generic;
using System.IO;

namespace ICP.Modules.Mvc.Admin.Services
{
    public class PaymentStatisticsService
    {
        private PaymentStatisticsRepository _paymentStatisticsRepository = null;
        private readonly ILogger _logger = null;

        public PaymentStatisticsService(
            PaymentStatisticsRepository paymentStatisticsRepository,
            ILogger<PaymentStatisticsService> logger
        )
        {
            _paymentStatisticsRepository = paymentStatisticsRepository;
            _logger = logger;
        }

        #region 每日提領金額監控

        #region 提領監控清單
        /// <summary>
        /// QueryWithdrawVM轉換ListWithdrawReq
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public DataResult<ListWithdrawDbReq> MappingToListWithdrawReq(QueryWithdrawVM model)
        {
            var result = new DataResult<ListWithdrawDbReq>();

            try
            {
                var config = new MapperConfiguration(cfg => cfg.CreateMap<QueryWithdrawVM, ListWithdrawDbReq>());
                var mapper = config.CreateMapper();
                ListWithdrawDbReq req = mapper.Map<ListWithdrawDbReq>(model);

                #region 僅顯示觀察廠商
                if (model.SelectTypeMode != null)
                {
                    if (!model.SelectTypeMode.SelectMode && model.SelectTypeMode.MonitorStaus)
                    {
                        req.MonitorStaus = 1;
                    }
                }
                #endregion

                if (model.RuleMode == 1)
                {
                    #region 規則一
                    #region 排序方式
                    req.SortType = model.SortType1;
                    #endregion

                    #region 提領設定
                    int transferTypeMode = 0;
                    if (model.TransferTypeMode1.Manually)
                    {
                        transferTypeMode += 1;
                    }
                    if (model.TransferTypeMode1.Auto)
                    {
                        transferTypeMode += 2;
                    }
                    req.TransferType = transferTypeMode == 3 ? 0 : transferTypeMode;
                    #endregion

                    #region 商品類別
                    int commoditiyTypeMode = 0;

                    if (model.CommoditiyTypeMode1.Real)
                    {
                        commoditiyTypeMode += 1;
                    }
                    if (model.CommoditiyTypeMode1.Virtual)
                    {
                        commoditiyTypeMode += 2;
                    }
                    if (model.CommoditiyTypeMode1.Extend)
                    {
                        commoditiyTypeMode += 4;
                    }
                    if (model.CommoditiyTypeMode1.Other)
                    {
                        commoditiyTypeMode += 8;
                    }

                    req.TransferType = commoditiyTypeMode == 15 ? 0 : commoditiyTypeMode;
                    #endregion
                    #endregion
                }
                else
                {
                    #region 規則二
                    #region 排序方式
                    req.SortType = model.SortType2;
                    #endregion

                    #region 提領設定
                    int transferTypeMode = 0;

                    if (model.TransferTypeMode2.Manually)
                    {
                        transferTypeMode += 1;
                    }
                    if (model.TransferTypeMode2.Auto)
                    {
                        transferTypeMode += 2;
                    }

                    req.TransferType = transferTypeMode == 3 ? 0 : transferTypeMode;
                    #endregion

                    #region 商品類別
                    int commoditiyTypeMode = 0;

                    if (model.CommoditiyTypeMode2.Real)
                    {
                        commoditiyTypeMode += 1;
                    }
                    if (model.CommoditiyTypeMode2.Virtual)
                    {
                        commoditiyTypeMode += 2;
                    }
                    if (model.CommoditiyTypeMode2.Extend)
                    {
                        commoditiyTypeMode += 4;
                    }
                    if (model.CommoditiyTypeMode2.Other)
                    {
                        commoditiyTypeMode += 8;
                    }

                    req.TransferType = commoditiyTypeMode == 15 ? 0 : commoditiyTypeMode;
                    #endregion
                    #endregion
                }

                result.SetSuccess(req);
            }
            catch
            {
                result.SetError();
            }

            return result;
        }

        /// <summary>
        /// 每日提領金額監控清單
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public DataResult<List<ListWithdrawDbRes>> ListWithdraw(ListWithdrawDbReq req)
        {
            var result = new DataResult<List<ListWithdrawDbRes>>();

            try
            {
                result.SetSuccess(_paymentStatisticsRepository.ListWithdraw(req));
            }
            catch
            {
                result.SetError();
            }

            return result;
        }

        /// <summary>
        /// 提領排程清單
        /// </summary>
        /// <returns></returns>
        public DataResult<List<BankTransferScheduleDbRes>> ListBankTransferSchedule(long mid)
        {
            var result = new DataResult<List<BankTransferScheduleDbRes>>();

            try
            {
                var args = new
                {
                    MID = mid
                };

                result.SetSuccess(_paymentStatisticsRepository.ListBankTransferSchedule(args));
            }
            catch
            {
                result.SetError();
            }

            return result;
        }
        #endregion

        #endregion

        #region 每日收款交易金額監控

        /// <summary>
        /// 每日收款交易金額監控 - 查詢監控記錄
        /// </summary>
        /// <param name="request">查詢條件</param>
        /// <returns></returns>
        public DataResult<List<QryIncomeMonitorRes>> ListIncomeMonitor(QryIncomeMonitorReq request)
        {
            var result = new DataResult<List<QryIncomeMonitorRes>>();

            QryIncomeMonitorDbReq dbReq = Mapper.Map<QryIncomeMonitorDbReq>(request);

            var dbResult = _paymentStatisticsRepository.ListDailyIncomeMonitor(dbReq);

            if (dbResult == null)
            {
                result.SetError();
                return result;
            }

            List<QryIncomeMonitorRes> qryRes = Mapper.Map<List<QryIncomeMonitorRes>>(dbResult);

            result.SetSuccess(qryRes);

            return result;
        }

        #endregion

        #region 每日收款交易金額監控

        /// <summary>
        /// 每日收款交易金額監控 - 查詢監控記錄
        /// </summary>
        /// <param name="request">查詢條件</param>
        /// <returns></returns>
        public DataResult<List<QryPaymentMonitorRes>> ListPaymentMonitor(QryPaymentMonitorReq request)
        {
            var result = new DataResult<List<QryPaymentMonitorRes>>();

            QryPaymentMonitorDbReq dbReq = Mapper.Map<QryPaymentMonitorDbReq>(request);

            var dbResult = _paymentStatisticsRepository.ListDailyPaymentMonitor(dbReq);

            if (dbResult == null)
            {
                result.SetError();
                return result;
            }

            List<QryPaymentMonitorRes> qryRes = Mapper.Map<List<QryPaymentMonitorRes>>(dbResult);

            result.SetSuccess(qryRes);

            return result;
        }

        #endregion


        #region 歷程
        /// <summary>
        /// 歷程清單
        /// </summary>
        /// <param name="mid">會員編號</param>
        /// <param name="monitorType">監控類型(1:風管定時監控,2:提領備註,3:提領檢視,4:收款備註,5:收款檢視,6:付款監控,7:付款檢視,8:定時監控檢視)</param>
        /// <returns></returns>
        public DataResult<List<ListMonitorLogDbRes>> GetMonitorLogList(long mid, int monitorType)
        {
            var result = new DataResult<List<ListMonitorLogDbRes>>();

            try
            {
                var args = new
                {
                    MID = mid,
                    MonitorType = monitorType
                };

                result.SetSuccess(_paymentStatisticsRepository.GetMonitorLogList(args));
            }
            catch(Exception ex)
            {
                _logger.Error($"GetMonitorLogList Error, Msg= {ex.ToString()}");
                result.SetError();
            }

            return result;
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
            var result = new BaseResult();

            try
            {
                var config = new MapperConfiguration(cfg => cfg.CreateMap<T, AddMonitorLogDbReq>());
                var mapper = config.CreateMapper();

                AddMonitorLogDbReq req = mapper.Map<AddMonitorLogDbReq>(model);
                req.Operator = Operator;

                result = _paymentStatisticsRepository.AddMonitorLog(req);
            }
            catch(Exception ex)
            {
                _logger.Error($"AddMonitorRemarkLog Error, Msg= {ex.ToString()}");
                result.SetError();
            }

            return result;
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
            string errorMID = string.Empty;
            string[] aryMID = listMID.Split('|');
            var result = new DataResult<string>();
            result.SetSuccess();

            foreach (string mID in aryMID)
            {
                try
                {
                    AddMonitorLogDbReq req = new AddMonitorLogDbReq()
                    {
                        MID = long.Parse(mID),
                        Status = 1,
                        Remark = remark,
                        Operator = Operator,
                        MonitorType = monitorType
                    };

                    var addResult = _paymentStatisticsRepository.AddMonitorLog(req);
                    if (!addResult.IsSuccess)
                    {
                        errorMID += mID + ",";
                    }
                }
                catch(Exception ex)
                {
                    _logger.Error($"AddMonitorInspectLog Error, Msg= {ex.ToString()}");
                    result.SetError();
                }
            }

            if (!string.IsNullOrWhiteSpace(errorMID))
            {
                result.SetError();
                result.RtnData = errorMID;
            }

            return result;
        }
        #endregion


        /// <summary>
        /// 定時監控
        /// </summary>
        /// <returns></returns>
        public DataResult<List<TimingMonitorRes>> ListTimingMonitor(QryTimingMonitorVM qryTimingMonitorVM)
        {
            TimingMonitorDbReq timingMonitorDbReq = Mapper.Map<TimingMonitorDbReq>(qryTimingMonitorVM);

            var result = new DataResult<List<TimingMonitorRes>>();

            try
            {               
                result.SetSuccess(_paymentStatisticsRepository.ListTimingMonitor(timingMonitorDbReq));
            }
            catch(Exception ex)
            {
                _logger.Error($"ListTimingMonitor Error, Msg= {ex.ToString()}");
                result.SetError();
            }

            return result;
        }

        /// <summary>
        /// 取得要匯出的定時監控明細資訊
        /// </summary>
        /// <returns></returns>
        public Func<TimingMonitorRes, string[]> GetExcelTimingMonitorDateil()
        {
            return t =>
            {
                int refundDay1To7Amount = t.RefundDay1To7Amount;
                if (t.RefundDay1To7Amount < 0)
                {
                    refundDay1To7Amount = -refundDay1To7Amount;
                }

                var c = System.Environment.NewLine;

                var values = new string[]
                {
                    t.ICPMID,
                    t.MerchantName,
                    t.WebSiteName,
                    t.RegisterDate.ToString("yyyy/MM/dd HH:mm:ss"),
                    !string.IsNullOrWhiteSpace(t.MCCCode) && t.MCCCode != "0"? t.MCCCode : "-",
                    $"{t.Day1Amount.ToString("N0")}{c}{t.Day1Count.ToString("N0")}",
                    $"{t.Day1Amount.ToString("N0")}/{(t.Day2To11Amount > 0 ? t.Day2To11Amount.ToString("N0") : "1")}{c}{t.Day1Percent}%",
                    $"{t.Day1To10Amount.ToString("N0")}/{(t.Day11To40Amount > 0 ? t.Day11To40Amount.ToString("N0") : "1")}{c}{t.Day10Percent}%",
                    $"{t.Day1To30Amount.ToString("N0")}/{(t.Day31To120Amount > 0 ? t.Day31To120Amount.ToString("N0") : "1")}{c}{t.Day30Percent}%",
                    $"{refundDay1To7Amount.ToString("N0")}/{t.RefundDay7Count.ToString("N0")}",
                };

                return values;
            };
        }

        public MemoryStream GetXlsStream<T>(string[] header, List<T> list, Func<T, string[]> arryDataGenerator, HSSFWorkbook workbook, string dateRange = null, string sheetName = "Sheet1", bool wrapText = false)
        {
            if (workbook == null) return null;

            var sheet = workbook.GetSheet(sheetName);

            if(sheet == null)
            {
                sheet = (HSSFSheet)workbook.CreateSheet("Sheet1");
            }

            int rowIndex = sheet.PhysicalNumberOfRows;

            HSSFCellStyle css = (HSSFCellStyle)sheet.Workbook.CreateCellStyle();
            css.VerticalAlignment = NPOI.SS.UserModel.VerticalAlignment.Center;
            css.Alignment = NPOI.SS.UserModel.HorizontalAlignment.Center;
            css.WrapText = wrapText;

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
    }
}
