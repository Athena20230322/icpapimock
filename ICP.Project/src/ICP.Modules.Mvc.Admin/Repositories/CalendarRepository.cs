using ICP.Infrastructure.Abstractions.DbUtil;
using ICP.Infrastructure.Core.Models;
using ICP.Modules.Mvc.Admin.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICP.Modules.Mvc.Admin.Repositories
{
    public class CalendarRepository
    {
        private readonly IDbConnectionPool _dbConnectionPool = null;
        private readonly IDbConnection db;

        public CalendarRepository(IDbConnectionPool dbConnectionPool)
        {
            _dbConnectionPool = dbConnectionPool;
            db = _dbConnectionPool.Create(DatabaseName.ICP_Payment);
        }

        /// <summary>
        /// 取得行事曆清單
        /// </summary>
        /// <param name="calendarQuery"></param>
        /// <returns></returns>
        public List<HolidayWorkingDayModel> ListHolidayWorkingDay(CalendarQuery calendarQuery)
        {
            string sql = "EXEC ausp_Payment_Calendar_ListHolidayWorkingDay_S";
            var args = new
            {
                calendarQuery.DayYear
            };
            sql += db.GenerateParameter(args);
            return db.Query<HolidayWorkingDayModel>(sql, args);
        }

        /// <summary>
        /// 新增行事曆資料
        /// </summary>
        /// <param name="holidayWorkingDayModel"></param>
        /// <returns></returns>
        public BaseResult AddHolidayWorkingDay(HolidayWorkingDayModel holidayWorkingDayModel)
        {
            string sql = "EXEC ausp_Payment_Calendar_AddHolidayWorkingDay_I";
            var args = new
            {
                holidayWorkingDayModel.DayDescription,
                holidayWorkingDayModel.DayDate,
                holidayWorkingDayModel.DayType,
                holidayWorkingDayModel.Creator
            };
            sql += db.GenerateParameter(args);
            return db.QuerySingleOrDefault<BaseResult>(sql, args);
        }

        /// <summary>
        /// 更新行事曆資料
        /// </summary>
        /// <param name="holidayWorkingDayModel"></param>
        /// <returns></returns>
        public BaseResult UpdateHolidayWorkingDay(HolidayWorkingDayModel holidayWorkingDayModel)
        {
            string sql = "EXEC ausp_Payment_Calendar_UpdateHolidayWorkingDay_U";
            var args = new
            {
                holidayWorkingDayModel.DayID,
                holidayWorkingDayModel.DayDescription,
                holidayWorkingDayModel.DayDate,
                holidayWorkingDayModel.DayType,
                holidayWorkingDayModel.Modifier
            };
            sql += db.GenerateParameter(args);
            return db.QuerySingleOrDefault<BaseResult>(sql, args);
        }

        /// <summary>
        /// 移除行事曆資料
        /// </summary>
        /// <param name="dayID"></param>
        /// <returns></returns>
        public BaseResult RemoveHolidayWorkingDay(int dayID)
        {
            string sql = "EXEC ausp_Payment_Calendar_DeleteHolidayWorkingDay_D";
            var args = new
            {
                DayID = dayID,
            };
            sql += db.GenerateParameter(args);
            return db.QuerySingleOrDefault<BaseResult>(sql, args);
        }

        /// <summary>
        /// 取得行事曆資料(單筆)
        /// </summary>
        /// <param name="dayID"></param>
        /// <returns></returns>
        public HolidayWorkingDayModel GetHolidayWorkingDay(int dayID)
        {
            string sql = "EXEC ausp_Payment_Calendar_GetHolidayWorkingDay_S";
            var args = new
            {
                DayID = dayID
            };
            sql += db.GenerateParameter(args);
            return db.QuerySingleOrDefault<HolidayWorkingDayModel>(sql, args);
        }
    }
}
