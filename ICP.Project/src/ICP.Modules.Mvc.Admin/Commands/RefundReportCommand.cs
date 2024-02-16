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
    public class RefundReportCommand
    {
        private readonly RefundReportService _refundReportService = null;
        private readonly ExportDataService _exportDataService = null;

        public RefundReportCommand(RefundReportService refundReportService, ExportDataService exportDataService)
        {
            _refundReportService = refundReportService;
            _exportDataService = exportDataService;
        }

        /// <summary>
        /// 取得退款明細資料
        /// </summary>
        /// <param name="refundReportQueryCondition"></param>
        /// <returns></returns>
        public List<RefundReportQueryResult> ListRefundDetail(RefundReportQueryCondition refundReportQueryCondition)
        {
            return _refundReportService.ListRefundDetail(refundReportQueryCondition);
        }

        /// <summary>
        /// 匯出退款報表 Excel
        /// </summary>
        /// <param name="refundReportQueryCondition"></param>
        /// <returns></returns>
        public MemoryStream ExportRefundReport(RefundReportQueryCondition refundReportQueryCondition)
        {
            List<RefundReportQueryResult> refundReportQueryResultList = _refundReportService.ListRefundDetail(refundReportQueryCondition);

            string functionName = "退款報表";
            string dateRange = $"查詢日期：{refundReportQueryCondition.StartDate.ToString("yyyy-MM-dd")} ~ {refundReportQueryCondition.EndDate.ToString("yyyy-MM-dd")}";

            #region 報表欄位
            string[] header = new string[]
            {
                "訂單日期", "付款日期", "icashpay 訂單編號", "付款方電支帳號", "付款方名稱", "收款方電支帳號", "收款方名稱", "收款方統一編號",
                "繳費方式", "撥款狀態", "原始訂單金額", "實際收到金額", "金流手續費", "應撥款項", "退款日期", "退款金額", "返還手續費"
            };
            #endregion

            Func<RefundReportQueryResult, string[]> arryDataGenerator = t =>
            {
                var values = new string[]
                {
                    t.CreateDate.ToString("yyyy/MM/dd HH:mm:ss"),
                    t.PaymentDate.ToString("yyyy/MM/dd HH:mm:ss"),
                    t.TradeNo,
                    t.PaymentSideICPMID,
                    t.PaymentSideName,
                    t.ReceiptSideICPMID,
                    t.ReceiptSideName,
                    t.ReceiptSideUnifiedBusinessNo,
                    t.PaymentTypeMeaning,
                    t.AllocateStatusMeaning,
                    t.Amount.ToString("N0"),
                    t.RealAmount.ToString("N0"),
                    t.GoldFlowChargeFee.ToString("N2"),
                    t.ShouldAllocateAmount.ToString("N2"),
                    t.RefundDate.ToString("yyyy/MM/dd HH:mm:ss"),
                    t.RefundAMT.ToString("N0"),
                    t.BackChargeFee.ToString("N2")
                };

                return values;
            };

            return _exportDataService.GetXlsStream(header, refundReportQueryResultList, arryDataGenerator, functionName, dateRange);
        }
    }
}
