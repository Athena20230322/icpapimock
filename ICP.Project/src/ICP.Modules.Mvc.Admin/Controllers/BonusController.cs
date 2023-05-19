using ICP.Infrastructure.Core.Models;
using ICP.Modules.Mvc.Admin.Attributes;
using ICP.Modules.Mvc.Admin.Commands;
using ICP.Modules.Mvc.Admin.Enums;
using ICP.Modules.Mvc.Admin.Models;
using ICP.Modules.Mvc.Admin.Models.ViewModels.Bonus;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace ICP.Modules.Mvc.Admin.Controllers
{
    public class BonusController : BaseAdminController
    {
        private readonly BonusCommand _bonusCommand = null;

        public BonusController(
            BonusCommand bonusCommand
        )
        {
            _bonusCommand = bonusCommand;
        }

        /// <summary>
        /// 紅利交易明細查詢 - 查詢頁
        /// </summary>
        /// <returns></returns>
        [UserLoginAuth(MappingMethod = "Index", Action = MappingMethodAction.Query)]
        public ActionResult Index()
        {
            QryBonusReq model = new QryBonusReq();

            return View(model);
        }

        /// <summary>
        /// 紅利交易明細查詢 - 結果頁
        /// </summary>
        /// <param name="topUpReportQueryCondition"></param>
        /// <returns></returns>        
        [HttpPost]
        [UserLoginAuth(MappingMethod = "Index", Action = MappingMethodAction.Query)]
        public ActionResult Query(QryBonusReq qryBonusReq)
        {
            DataResult<List<QryBonusRes>> result = _bonusCommand.ListFinanceBonusDetail(qryBonusReq);

            if (!result.IsSuccess)
            {
                return RedirectAndAlert(Url.Action("Index"), result.RtnMsg);
            }

            List<QryBonusRes> list = result.RtnData;

            if (result.RtnData.Count > 0)
            {
                ViewBag.IsLastPage = (int)Math.Ceiling((decimal)result.RtnData[0].TotalCount / qryBonusReq.PageSize) == qryBonusReq.PageNo;
            }

            return PagedListView(list, qryBonusReq);
        }

        /// <summary>
        /// 匯出紅利交易Excel報表
        /// </summary>
        /// <param name="qryBonusReq"></param>
        /// <returns></returns>
        [UserLoginAuth(MappingMethod = "Index", Action = MappingMethodAction.Export)]
        public ActionResult ExportExcel(QryBonusReq qryBonusReq)
        {
            DataResult<List<QryBonusRes>> result = _bonusCommand.ListFinanceBonusDetail(qryBonusReq);

            if (!result.IsSuccess)
            {
                return null;
            }

            var file = _bonusCommand.ExportBonusDetailExcel(result.RtnData, qryBonusReq.StartDate.ToString("yyyy/MM/dd"), qryBonusReq.EndDate.ToString("yyyy/MM/dd"));

            file.Flush();
            file.Position = 0;

            return File(file, "application/ms-excel", $"紅利交易明細_{DateTime.Now.ToString("yyyyMMdd")}_{DateTime.Now.ToString("HHmmss")}.xls");
        }
    }
}
