using ICP.Infrastructure.Core.Models;
using ICP.Modules.Mvc.Admin.Models.Finance;
using ICP.Modules.Mvc.Admin.Models.Finance.MerchantTradeDetail;
using ICP.Modules.Mvc.Admin.Models.Finance.TradeDetail;
using ICP.Modules.Mvc.Admin.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.Mvc;

namespace ICP.Modules.Mvc.Admin.Commands
{
    public class FinanceCommand
    {
        private FinanceService _financeService = null;
        private ExportDataService _exportDataService = null;

        public FinanceCommand(
            FinanceService financeService, 
            ExportDataService exportDataService
            )
        {
            _financeService = financeService;
            _exportDataService = exportDataService;
        }

        #region 實質交易明細查詢

        /// <summary>
        /// 實質交易明細查詢 - 日期類型選單
        /// </summary>
        public IEnumerable<SelectListItem> ListTradeDetailDateTypeItem()
        {
            var selectList = new List<SelectListItem>
            {
                new SelectListItem { Value = "1", Text = "訂單日期" },
                new SelectListItem { Value = "2", Text = "付款日期" },
                new SelectListItem { Value = "3", Text = "傳輸日期" },
                new SelectListItem { Value = "4", Text = "撥款日期" },
                new SelectListItem { Value = "5", Text = "退款日期" }
            };

            return selectList;
        }

        /// <summary>
        /// 實質交易明細查詢 - 訂單類別選單
        /// </summary>
        public IEnumerable<SelectListItem> ListTradeDetailTradeNoTypeItem()
        {
            var selectList = new List<SelectListItem>
            {
                new SelectListItem { Value = "1", Text = "icashpay訂單編號" },
                new SelectListItem { Value = "2", Text = "特店訂單編號" }
            };

            return selectList;
        }

        /// <summary>
        /// 實質交易明細查詢 - 付款狀態選單
        /// </summary>
        public IEnumerable<SelectListItem> ListTradeDetailPaymentStatusItem()
        {
            var selectList = new List<SelectListItem>
            {
                new SelectListItem { Value = "0", Text = "全部" },
                new SelectListItem { Value = "1", Text = "未付款" },
                new SelectListItem { Value = "2", Text = "已付款" },
                new SelectListItem { Value = "3", Text = "已退款" },
                new SelectListItem { Value = "4", Text = "付款失敗" },
                new SelectListItem { Value = "5", Text = "退款失敗" }
            };

            return selectList;
        }

        /// <summary>
        /// 實質交易明細查詢 - 交易類型選單
        /// </summary>
        public IEnumerable<SelectListItem> ListTradeDetailTradeStatusItem()
        {
            var selectList = new List<SelectListItem>
            {
                new SelectListItem { Value = "0", Text = "全部" },
                new SelectListItem { Value = "1", Text = "銷售(交易)" },
                new SelectListItem { Value = "2", Text = "銷退(退款)" },
                new SelectListItem { Value = "3", Text = "銷售/銷退" },
                new SelectListItem { Value = "4", Text = "沖正" }
            };

            return selectList;
        }

        /// <summary>
        /// 實質交易明細查詢 - 電支帳號類型選單
        /// </summary>
        public IEnumerable<SelectListItem> ListTradeDetailICPMIDTypeItem()
        {
            var selectList = new List<SelectListItem>
            {
                new SelectListItem { Value = "1", Text = "收款方" },
                new SelectListItem { Value = "2", Text = "付款方" }
            };

            return selectList;
        }

        /// <summary>
        /// 實質交易明細查詢 - 撥款狀態選單
        /// </summary>
        public IEnumerable<SelectListItem> ListTradeDetailAllocateStatusItem()
        {
            var selectList = new List<SelectListItem>
            {
                new SelectListItem { Value = "0", Text = "全部" },
                new SelectListItem { Value = "1", Text = "未撥款" },
                new SelectListItem { Value = "2", Text = "已撥款" }
            };

            return selectList;
        }

        /// <summary>
        /// 實質交易明細查詢 - 付款方式選單
        /// </summary>
        public IEnumerable<SelectListItem> ListTradeDetailPaymentTypeItem()
        {
            var selectList = new List<SelectListItem>
            {
                new SelectListItem { Value = "0", Text = "全部" },
                new SelectListItem { Value = "1", Text = "icashpay帳戶" },
                new SelectListItem { Value = "2", Text = "連結扣款帳戶" }
            };

            return selectList;
        }

        /// <summary>
        /// 實質交易明細查詢 - 初始條件
        /// </summary>
        /// <returns></returns>
        public QryTradeDetailReq SetTradeDetailDefaultSetting()
        {
            QryTradeDetailReq model = new QryTradeDetailReq()
            {
                //日期類型選單
                DateTypeList = ListTradeDetailDateTypeItem(),
                //訂單類別選單
                TradeNoTypeList = ListTradeDetailTradeNoTypeItem(),
                //付款狀態選單
                PaymentStatusList = ListTradeDetailPaymentStatusItem(),
                //交易類型選單
                TradeStatusList = ListTradeDetailTradeStatusItem(),
                //電支帳號類型選單
                ICPMIDTypeList = ListTradeDetailICPMIDTypeItem(),
                //撥款狀態選單
                AllocateStatusList = ListTradeDetailAllocateStatusItem(),
                //付款方式選單
                PaymentTypeList = ListTradeDetailPaymentTypeItem()
            };

            return model;
        }

        /// <summary>
        /// 實質交易明細查詢 - 查詢交易明細
        /// </summary>
        /// <param name="request">查詢條件</param>
        /// <returns></returns>
        public DataResult<List<QryTradeDetailRes>> ListTradeDetail(QryTradeDetailReq request)
        {
            var result = _financeService.ListTradeDetail(request);

            return result;
        }

        /// <summary>
        /// 實質交易明細查詢 - 匯出
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public MemoryStream ExportTradeDetail(QryTradeDetailReq request)
        {
            var list = _financeService.ListTradeDetail(request);

            if (!list.RtnData.Any()) return null;

            string functionName = "實質交易明細查詢";

            string dateRange = $"查詢日期:{request.DateStart.Replace('-', '/')}~{request.DateEnd.Replace('-', '/')}";

            #region 標題

            string[] header = new string[]
            {
                "交易類型","訂單日期","付款日期","傳輸日期","撥款日期","退款日期","收款方電支帳號","付款方電支帳號","平台商編號","icashpay訂單編號",
                "特店訂單編號","付款方式","款項來源(銀行)","原始訂單金額","信託金額","實際收到金額","點數折抵金額","交易手續費(%數/$筆)","交易手續費金額","應撥/退款項(淨額)",
                "付款狀態","撥款狀態"
            };

            #endregion

            string[] arryDataGenerator(QryTradeDetailRes t)
            {
                var values = new string[]
                {
                    t.TradeStatusName,
                    t.CreateDate.ToString("yyyy/MM/dd HH:mm:ss"),
                    t.PaymentDate?.ToString("yyyy/MM/dd HH:mm:ss")??"-",
                    t.TransmittalDate?.ToString("yyyy/MM/dd HH:mm:ss")??"-",
                    t.AllocateDate?.ToString("yyyy/MM/dd HH:mm:ss")??"-",
                    t.RefundDate?.ToString("yyyy/MM/dd HH:mm:ss")??"-",
                    t.PayeeICPMID,
                    t.PayerICPMID,
                    t.PlatformID==0?"-":t.PlatformID.ToString(),
                    t.TradeNo,

                    t.MerchantTradeNo,
                    t.PaymentTypeName,
                    t.PaymentSource,
                    t.TotalAmount.ToString("N0"),
                    t.TrustAmt.ToString("N0"),
                    t.RealAmt.ToString("N0"),
                    t.BonusAmt.ToString("N0"),
                    t.ChargeFee,
                    t.ChargeFeeAmt.ToString("N0"),
                    t.AllocateAmt.ToString("N0"),

                    t.PaymentStatusName,
                    t.AllocateStatusName
                };

                return values;
            }

            MemoryStream ms = _exportDataService.GetXlsStream(header, list.RtnData, arryDataGenerator, functionName, dateRange);

            return ms;
        }

        #endregion

        #region 特店帳務進出明細

        /// <summary>
        /// 特店帳務進出明細 - 電支使用者選單
        /// </summary>
        public IEnumerable<SelectListItem> ListMerchantTradeDetailUserTypeItem()
        {
            var selectList = new List<SelectListItem>
            {
                new SelectListItem { Value = "1", Text = "電支帳號" },
                new SelectListItem { Value = "2", Text = "名稱" }
            };

            return selectList;
        }

        /// <summary>
        /// 特店帳務進出明細 - 帳務類型選單
        /// </summary>
        public IEnumerable<SelectListItem> ListMerchantTradeDetailTradeModeItem(int type = 1)
        {
            var selectList = _financeService.ListTradeMode(type);
            selectList.Insert(0, new SelectListItem { Value = "0", Text = "全部" });

            return selectList;
        }

        /// <summary>
        /// 特店帳務進出明細 - 交易類型選單
        /// </summary>
        /// <param name="tradeMode">帳務類型</param>
        /// <returns></returns>
        public IEnumerable<SelectListItem> ListMerchantTradeDetailPaymentTypeItem(int tradeMode = 0)
        {
            var selectList = new List<SelectListItem>();

            if (tradeMode != 0)
            {
                selectList = _financeService.ListTradeType(tradeMode);
            }

            selectList.Insert(0, new SelectListItem { Value = "0", Text = "全部" });

            return selectList;
        }

        /// <summary>
        /// 特店帳務進出明細 - 交易子類型選單
        /// </summary>
        /// <param name="tradeTypeID">交易類型</param>
        /// <returns></returns>
        public IEnumerable<SelectListItem> ListMerchantTradeDetailPaymentSubTypeItem(int tradeTypeID = 0)
        {
            var selectList = new List<SelectListItem>();

            if (tradeTypeID != 0)
            {
                selectList = _financeService.ListTradeSubType(tradeTypeID);
            }

            selectList.Insert(0, new SelectListItem { Value = "0", Text = "全部" });

            return selectList;
        }

        /// <summary>
        /// 特店帳務進出明細 - 查詢明細
        /// </summary>
        /// <param name="request">查詢條件</param>
        /// <returns></returns>
        public DataResult<List<QryMerchantTradeDetailRes>> ListMerchantTradeDetail(QryMerchantTradeDetailReq request)
        {
            var result = _financeService.ListMerchantTradeDetail(request);

            return result;
        }

        /// <summary>
        /// 特店帳務進出明細 - 匯出
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public MemoryStream ExportMerchantTradeDetail(QryMerchantTradeDetailReq request)
        {
            var list = _financeService.ListMerchantTradeDetail(request);

            if (!list.RtnData.Any()) return null;

            string functionName = "特店帳務進出明細";

            string dateRange = $"查詢日期:{request.DateStart.Replace('-', '/')}~{request.DateEnd.Replace('-', '/')}";

            #region 標題

            string[] header = new string[]
            {
                "帳務進出日期","電支帳號","名稱","交易時間","交易編號","帳務類型","交易類型","交易子類型","交易收入","交易支出","交易後餘額"
            };

            #endregion

            string[] arryDataGenerator(QryMerchantTradeDetailRes t)
            {
                var values = new string[]
                {
                    t.CreateDate.ToString("yyyy/MM/dd HH:mm:ss"),
                    t.ICPMID,
                    t.UserName,
                    t.PaymentDate.ToString("yyyy/MM/dd HH:mm:ss"),
                    t.TradeNo.ToString(),
                    t.TradeModeCName,
                    t.PaymentTypeName,
                    t.PaymentSubTypeName,
                    t.Income.ToString("N0"),
                    t.Payment.ToString("N0"),
                    t.NewCash.ToString("N0")
                };

                return values;
            }

            MemoryStream ms = _exportDataService.GetXlsStream(header, list.RtnData, arryDataGenerator, functionName, dateRange);

            return ms;
        }

        #endregion

    }
}
