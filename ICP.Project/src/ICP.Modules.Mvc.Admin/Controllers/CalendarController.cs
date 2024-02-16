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
    public class CalendarController : BaseAdminController
    {
        private readonly CalendarCommand _calendarCommand = null;

        public CalendarController(CalendarCommand calendarCommand)
        {
            _calendarCommand = calendarCommand;
        }

        /// <summary>
        /// 行事曆首頁
        /// </summary>
        /// <returns></returns>
        [UserLoginAuth(MappingMethod = "HolidayWorkingDaySetting", Action = MappingMethodAction.Query)]
        public ActionResult HolidayWorkingDaySetting()
        {
            return View();
        }

        /// <summary>
        /// 查詢結果頁
        /// </summary>
        /// <param name="calendarQuery"></param>
        /// <returns></returns>
        [UserLoginAuth(MappingMethod = "HolidayWorkingDaySetting", Action = MappingMethodAction.Query)]
        public ActionResult Query(CalendarQuery calendarQuery)
        {
            List<HolidayWorkingDayModel> holidayWorkingDayModelList = _calendarCommand.ListHolidayWorkingDay(calendarQuery);
            return PagedListView(holidayWorkingDayModelList, calendarQuery);
        }

        /// <summary>
        /// 新增頁
        /// </summary>
        /// <returns></returns>
        [UserLoginAuth(MappingMethod = "HolidayWorkingDaySetting", Action = MappingMethodAction.Add)]
        public ActionResult Add()
        {
            return View();
        }

        /// <summary>
        /// 新增頁處理
        /// </summary>
        /// <param name="holidayWorkingDayModel"></param>
        /// <returns></returns>
        [UserLoginAuth(MappingMethod = "HolidayWorkingDaySetting", Action = MappingMethodAction.Add)]
        [HttpPost]
        public ActionResult Add(HolidayWorkingDayModel holidayWorkingDayModel)
        {
            if (!holidayWorkingDayModel.IsValid())
            {
                return HttpNotFound();
            }
            holidayWorkingDayModel.Creator = CurrentUser.Account;

            BaseResult addResult = _calendarCommand.AddHolidayWorkingDay(holidayWorkingDayModel);
            if (!addResult.IsSuccess)
            {
                ModelState.AddModelError("", addResult.RtnMsg);
                return View(holidayWorkingDayModel);
            }

            if (Request.IsAjaxRequest())
            {
                return Json(addResult);
            }
            else
            {
                return RedirectAndAlert(Url.Action("Add"), "成功");
            }
        }

        /// <summary>
        /// 編輯頁
        /// </summary>
        /// <param name="dayID"></param>
        /// <returns></returns>
        [UserLoginAuth(MappingMethod = "HolidayWorkingDaySetting", Action = MappingMethodAction.Edit)]
        public ActionResult Edit(int dayID)
        {
            if (dayID == 0)
                return HttpNotFound();

            HolidayWorkingDayModel holidayWorkingDayModel = _calendarCommand.GetHolidayWorkingDay(dayID);
            if (holidayWorkingDayModel == null)
                return HttpNotFound();

            return View(holidayWorkingDayModel);
        }

        /// <summary>
        /// 編輯頁處理
        /// </summary>
        /// <param name="holidayWorkingDayModel"></param>
        /// <returns></returns>
        [UserLoginAuth(MappingMethod = "HolidayWorkingDaySetting", Action = MappingMethodAction.Edit)]
        [HttpPost]
        public ActionResult Edit(HolidayWorkingDayModel holidayWorkingDayModel)
        {
            if (!holidayWorkingDayModel.IsValid())
            {
                return HttpNotFound();
            }
            holidayWorkingDayModel.Modifier = CurrentUser.Account;

            BaseResult updateResult = _calendarCommand.UpdateHolidayWorkingDay(holidayWorkingDayModel);
            if (!updateResult.IsSuccess)
            {
                ModelState.AddModelError("", updateResult.RtnMsg);
                return View(holidayWorkingDayModel);
            }

            if (Request.IsAjaxRequest())
            {
                return Json(updateResult);
            }
            else
            {
                return RedirectAndAlert(Url.Action("Edit"), "成功");
            }
        }

        /// <summary>
        /// 刪除處理
        /// </summary>
        /// <param name="dayID"></param>
        /// <returns></returns>
        [UserLoginAuth(MappingMethod = "HolidayWorkingDaySetting", Action = MappingMethodAction.Delete)]
        [HttpPost]
        public ActionResult Remove(int dayID)
        {
            if (dayID == 0)
                return HttpNotFound();

            BaseResult delResult = _calendarCommand.RemoveHolidayWorkingDay(dayID);
            return Json(delResult);
        }
    }
}
