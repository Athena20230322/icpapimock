using ICP.Infrastructure.Core.Models;
using ICP.Modules.Mvc.Admin.Attributes;
using ICP.Modules.Mvc.Admin.Commands;
using ICP.Modules.Mvc.Admin.Enums;
using ICP.Modules.Mvc.Admin.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace ICP.Modules.Mvc.Admin.Controllers
{
    public class TopUpReportController : BaseAdminController
    {
        private readonly TopUpReportCommand _topUpReportCommand = null;

        public TopUpReportController(TopUpReportCommand topUpReportCommand)
        {
            _topUpReportCommand = topUpReportCommand;
        }

        /// <summary>
        /// 內部後台：儲值報表 → 查詢表單頁
        /// </summary>
        /// <returns></returns>
        [UserLoginAuth(MappingMethod = "Index", Action = MappingMethodAction.Query)]
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// 內部後台：儲值報表 → 查詢結果頁
        /// </summary>
        /// <param name="topUpReportQueryCondition"></param>
        /// <returns></returns>
        [UserLoginAuth(MappingMethod = "Index", Action = MappingMethodAction.Query)]
        [HttpPost]
        public ActionResult Query(TopUpReportQueryCondition topUpReportQueryCondition)
        {
            List<TopUpReportQueryResult> topUpReportQueryResultList = _topUpReportCommand.ListTopUpDetails(topUpReportQueryCondition);
            return PagedListView(topUpReportQueryResultList, topUpReportQueryCondition);
        }

        /// <summary>
        /// 匯出儲值報表 Excel
        /// </summary>
        /// <param name="topUpReportQueryCondition"></param>
        /// <returns></returns>
        public ActionResult ExportTopUpReport(TopUpReportQueryCondition topUpReportQueryCondition)
        {
            topUpReportQueryCondition.PageNo = 1;
            topUpReportQueryCondition.PageSize = 65536;

            var xlsStream = _topUpReportCommand.ExportTopUpReport(topUpReportQueryCondition);
            if (xlsStream == null)
                return Content("<script>alert('查無資料');history.back();</script>");

            string fileName = "TopUpList_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xls";
            xlsStream.Flush();
            xlsStream.Position = 0;
            return File(xlsStream, "application/ms-excel", fileName);
        }
    }
}
