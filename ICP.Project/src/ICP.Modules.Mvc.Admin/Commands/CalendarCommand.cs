using ICP.Infrastructure.Core.Models;
using ICP.Modules.Mvc.Admin.Models;
using ICP.Modules.Mvc.Admin.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICP.Modules.Mvc.Admin.Commands
{
    public class CalendarCommand
    {
        private readonly CalendarService _calendarService = null;

        public CalendarCommand(CalendarService calendarService)
        {
            _calendarService = calendarService;
        }

        /// <summary>
        /// 取得行事曆清單
        /// </summary>
        /// <param name="calendarQuery"></param>
        /// <returns></returns>
        public List<HolidayWorkingDayModel> ListHolidayWorkingDay(CalendarQuery calendarQuery)
        {
            return _calendarService.ListHolidayWorkingDay(calendarQuery);
        }

        /// <summary>
        /// 新增行事曆資料
        /// </summary>
        /// <param name="holidayWorkingDayModel"></param>
        /// <returns></returns>
        public BaseResult AddHolidayWorkingDay(HolidayWorkingDayModel holidayWorkingDayModel)
        {
            return _calendarService.AddHolidayWorkingDay(holidayWorkingDayModel);
        }

        /// <summary>
        /// 更新行事曆資料
        /// </summary>
        /// <param name="holidayWorkingDayModel"></param>
        /// <returns></returns>
        public BaseResult UpdateHolidayWorkingDay(HolidayWorkingDayModel holidayWorkingDayModel)
        {
            return _calendarService.UpdateHolidayWorkingDay(holidayWorkingDayModel);
        }

        /// <summary>
        /// 移除行事曆資料
        /// </summary>
        /// <param name="dayID"></param>
        /// <returns></returns>
        public BaseResult RemoveHolidayWorkingDay(int dayID)
        {
            return _calendarService.RemoveHolidayWorkingDay(dayID);
        }

        /// <summary>
        /// 取得行事曆資料(單筆)
        /// </summary>
        /// <param name="dayID"></param>
        /// <returns></returns>
        public HolidayWorkingDayModel GetHolidayWorkingDay(int dayID)
        {
            return _calendarService.GetHolidayWorkingDay(dayID);
        }
    }
}
