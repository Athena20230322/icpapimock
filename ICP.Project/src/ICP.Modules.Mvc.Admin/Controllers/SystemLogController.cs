using ICP.Infrastructure.Core.Models;
using ICP.Modules.Mvc.Admin.Attributes;
using ICP.Modules.Mvc.Admin.Commands;
using ICP.Modules.Mvc.Admin.Enums;
using ICP.Modules.Mvc.Admin.Models.SystemLog.SystemError;
using System.Collections.Generic;
using System.Web.Mvc;

namespace ICP.Modules.Mvc.Admin.Controllers
{
    public class SystemLogController : BaseAdminController
    {
        private readonly SystemLogCommand _systemLogCommand = null;

        public SystemLogController
        (
            SystemLogCommand systemLogCommand
        )
        {
            _systemLogCommand = systemLogCommand;
        }

        #region 後台系統管理 LOG查詢 系統異常記錄查詢
        /// <summary>
        /// 後台系統管理 LOG查詢 系統異常記錄查詢 - 查詢頁
        /// </summary>
        /// <returns></returns>
        [UserLoginAuth(MappingMethod = "SystemErrorLog", Action = MappingMethodAction.Query)]
        public ActionResult SystemErrorLog()
        {
            QrySystemErrorReq model = new QrySystemErrorReq();

            return View(model);
        }

        /// <summary>
        /// 後台系統管理 LOG查詢 系統異常記錄查詢 - 結果頁
        /// </summary>
        /// <param name="qrySystemErrorReq"></param>
        /// <returns></returns>
        [HttpPost]
        [UserLoginAuth(MappingMethod = "SystemErrorLog", Action = MappingMethodAction.Query)]
        public ActionResult SystemErrorLogResult(QrySystemErrorReq qrySystemErrorReq)
        {
            DataResult<List<QrySystemErrorRes>> result = _systemLogCommand.ListSystemErrorLogResult(qrySystemErrorReq);

            if (!result.IsSuccess)
            {
                return RedirectAndAlert(Url.Action("Index"), result.RtnMsg);
            }

            List<QrySystemErrorRes> list = result.RtnData;

            return PagedListView(list, qrySystemErrorReq);
        }

        /// <summary>
        /// 後台系統管理 LOG查詢 系統異常記錄查詢 - 錯誤訊息Detail
        /// </summary>
        /// <param name="LogId"></param>
        /// <param name="SiteType"></param>
        /// <returns></returns>
        [UserLoginAuth(MappingMethod = "SystemErrorLog", Action = MappingMethodAction.Query)]
        public ActionResult SystemErrorLogDetail(long LogId, int SiteType)
        {
            DataResult<SystemErrorDetailRes> result = _systemLogCommand.GetSystemErrorDetail(LogId, SiteType);
            if (!result.IsSuccess)
            {
                return HttpNotFound();
            }
            
            return View(result.RtnData);
        }
        #endregion


    }
}
