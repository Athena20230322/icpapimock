using ICP.Infrastructure.Core.Models;
using ICP.Modules.Mvc.Admin.Models.ViewModels.Bonus;
using ICP.Modules.Mvc.Admin.Services;
using System;
using System.Collections.Generic;
using System.IO;

namespace ICP.Modules.Mvc.Admin.Commands
{
    public class BonusCommand
    {
        private readonly BonusService _bonusService = null;
        private readonly ExportDataService _exportDataService = null;

        public BonusCommand(
            BonusService bonusService,
            ExportDataService exportDataService
        )
        {
            _bonusService = bonusService;
            _exportDataService = exportDataService;
        }

        /// <summary>
        /// 取得紅利交易明細
        /// </summary>
        /// <param name="topUpReportQueryCondition"></param>
        /// <returns></returns>
        public DataResult<List<QryBonusRes>> ListFinanceBonusDetail(QryBonusReq qryBonusReq)
        {
            return _bonusService.ListFinanceBonusDetail(qryBonusReq);
        }

        /// <summary>
        /// 匯出紅利交易明細Excel報表
        /// </summary>
        /// <param name="qryBonusRes"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        public MemoryStream ExportBonusDetailExcel(List<QryBonusRes> qryBonusRes,string startDate, string endDate)
        {
            string functionName = "紅利交易明細";

            string dateRange = $"查詢起迄：{startDate} ~ {endDate}";

            #region 標題
            string[] header = new string[]
            {
                "紅利類型", "訂單日期", "付款日期", "退款日期", "icashpay 訂單編號", "特店訂單編號", "收款方電支帳號", "收款方名稱",
                "付款方電支帳號", "付款方名稱", "交易金額", "折抵點數", "點數折抵金額", "實付/退金額", "付款方式", "退款狀態"
            };
            #endregion

            return _exportDataService.GetXlsStream(header, qryBonusRes, _bonusService.GetExcelDateil(), functionName, dateRange);
        }
    }
}
