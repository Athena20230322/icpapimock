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
    public class RefundReportController : BaseAdminController
    {
        private readonly RefundReportCommand _refundReportCommand = null;

        public RefundReportController(RefundReportCommand refundReportCommand)
        {
            _refundReportCommand = refundReportCommand;
        }

        /// <summary>
        /// 內部後台：退款報表 → 查詢表單頁
        /// </summary>
        /// <returns></returns>
        [UserLoginAuth(MappingMethod = "Index", Action = MappingMethodAction.Query)]
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// 內部後台：退款報表 → 查詢結果頁
        /// </summary>
        /// <param name="refundReportQueryCondition"></param>
        /// <returns></returns>
        [UserLoginAuth(MappingMethod = "Index", Action = MappingMethodAction.Query)]
        [HttpPost]
        public ActionResult Query(RefundReportQueryCondition refundReportQueryCondition)
        {
            List<RefundReportQueryResult> refundReportQueryResultList = _refundReportCommand.ListRefundDetail(refundReportQueryCondition);

            return PagedListView(refundReportQueryResultList, refundReportQueryCondition);
        }

        /// <summary>
        /// 匯出退款報表 Excel
        /// </summary>
        /// <param name="refundReportQueryCondition"></param>
        /// <returns></returns>
        [UserLoginAuth(MappingMethod = "Index", Action = MappingMethodAction.Export)]
        public ActionResult ExportRefundReport(RefundReportQueryCondition refundReportQueryCondition)
        {
            refundReportQueryCondition.PageNo = 1;
            refundReportQueryCondition.PageSize = 65536;

            var xlsStream = _refundReportCommand.ExportRefundReport(refundReportQueryCondition);
            if (xlsStream == null)
                return Content("<script>alert('查無資料');history.back();</script>");

            string fileName = "RefundDetail_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xls";
            xlsStream.Flush();
            xlsStream.Position = 0;
            return File(xlsStream, "application/ms-excel", fileName);
        }
    }
}
