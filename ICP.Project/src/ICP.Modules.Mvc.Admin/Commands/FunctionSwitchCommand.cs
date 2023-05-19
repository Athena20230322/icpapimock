using ICP.Infrastructure.Core.Extensions;
using ICP.Infrastructure.Core.Models;
using ICP.Library.Models.AdminModels;
using ICP.Library.Services.AdminServices;
using ICP.Modules.Mvc.Admin.Models;
using ICP.Modules.Mvc.Admin.Models.ViewModels;
using ICP.Modules.Mvc.Admin.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICP.Modules.Mvc.Admin.Commands
{
    public class FunctionSwitchCommand
    {
        FunctionSwitchService _functionSwitchService;
        LibAdminService _libAdminService;

        public FunctionSwitchCommand(
            FunctionSwitchService functionSwitchService,
            LibAdminService libAdminService
            )
        {
            _functionSwitchService = functionSwitchService;
            _libAdminService = libAdminService;
        }

        /// <summary>
        /// 取得內部後台功能開關
        /// </summary>
        /// <param name="functionName"></param>
        /// <returns></returns>
        public List<FunctionCatagory> ListFunctionCategory(string functionName)
        {
            return _functionSwitchService.ListFunctionCategory(functionName);
        }

        /// <summary>
        /// 更新內部後台功能開關
        /// </summary>
        /// <param name="FunctionID"></param>
        /// <param name="Status"></param>
        /// <param name="Modifier"></param>
        /// <returns></returns>
        public BaseResult UpdateFunctionCategoryStatus(int FunctionID, byte Status, string Modifier, long RealIP, long ProxyIP)
        {
            if (FunctionID == 1041)
            {
                return new BaseResult
                {
                    RtnCode = 0,
                    RtnMsg = "功能維護設定無法永久啟用維護"
                };
            }

            return _functionSwitchService.UpdateFunctionCategoryStatus(FunctionID, Status, Modifier, RealIP, ProxyIP);
        }

        /// <summary>
        /// 取得內部後台預約開關列表
        /// </summary>
        /// <param name="FunctionID"></param>
        /// <returns></returns>
        public List<FunctionCategoryStatusRev> ListFunctionCategoryStatusRev(int FunctionID)
        {
            return _functionSwitchService.ListFunctionCategoryStatusRev(FunctionID);
        }

        /// <summary>
        /// 取得App 開關列表
        /// </summary>
        /// <returns></returns>
        public List<AppFunctionSwitch> ListAppFunctionSwitch(string FunctionName)
        {
            string AppName = "icp";

            return _libAdminService.ListAppFunctionSwitch(AppName: AppName, FunctionName: FunctionName);
        }

        /// <summary>
        /// 更新 App 功能開關
        /// </summary>
        /// <param name="model"></param>
        /// <param name="Modifier"></param>
        /// <param name="RealIP"></param>
        /// <param name="ProxyIP"></param>
        /// <returns></returns>
        public BaseResult UpdateAppFunctionSwitch(UpdateAppFunctionSwitch model, string Modifier, long RealIP, long ProxyIP)
        {
            return _functionSwitchService.UpdateAppFunctionSwitch(model, Modifier, RealIP, ProxyIP);
        }

        /// <summary>
        /// 取得內部後台預約開關
        /// </summary>
        /// <param name="RevID"></param>
        /// <returns></returns>
        public FunctionCategoryStatusRev GetFunctionCategoryStatusRev(int RevID)
        {
            return _functionSwitchService.GetFunctionCategoryStatusRev(RevID);
        }

        /// <summary>
        /// 新增內部後台預約開關
        /// </summary>
        /// <param name="model"></param>
        /// <param name="Modifier"></param>
        /// <param name="RealIP"></param>
        /// <param name="ProxyIP"></param>
        /// <returns></returns>
        public BaseResult AddFunctionCategoryStatusRev(AddFunctionSwitchRevVM model, string Modifier, long RealIP, long ProxyIP)
        {
            var result = new BaseResult();
            result.SetError();

            var switchStatusRev = model.FunctionCategoryStatusRev;
            if (switchStatusRev == null)
            {
                result.RtnMsg = "請填寫預設資訊";
                return result;
            }

            DateTime startDate;
            if (!DateTime.TryParse($"{model.SwitchStartDate} {model.SwitchStartTime}", out startDate))
            {
                result.RtnMsg = "請選擇開關預設開始時間";
                return result;
            }

            DateTime endDate;
            if (!DateTime.TryParse($"{model.SwitchEndDate} {model.SwitchEndTime}", out endDate))
            {
                result.RtnMsg = "請選擇開關預設關閉時間，關閉時間到了後，功能狀態將依照主開關狀態變動";
                return result;
            }

            if (startDate <= DateTime.Now || startDate >= endDate)
            {
                result.RtnMsg = "請確認開啟/關閉維護時間(不可晚於當下時間)";
                return result;
            }

            switchStatusRev.StartDate = startDate;
            switchStatusRev.EndDate = endDate;

            return _functionSwitchService.AddFunctionCategoryStatusRev(switchStatusRev, Modifier, RealIP, ProxyIP);
        }

        /// <summary>
        /// 取得權限設定歷程
        /// </summary>
        /// <param name="FunctionID"></param>
        /// <returns></returns>
        public List<FunctionCategoryLog> ListFunctionCategoryLog(int FunctionID)
        {
            return _functionSwitchService.ListFunctionCategoryLog(FunctionID);
        }

        /// <summary>
        /// 刪除內部後台預約開關
        /// </summary>
        /// <param name="RevID"></param>
        /// <returns></returns>
        public BaseResult DeleteFunctionCategoryStatusRev(int RevID, string Modifier, long RealIP, long ProxyIP)
        {
            return _functionSwitchService.DeleteFunctionCategoryStatusRev(RevID, Modifier, RealIP, ProxyIP);
        }

        /// <summary>
        /// 取得APP預約開關列表
        /// </summary>
        /// <param name="AppName"></param>
        /// <param name="FunctionID"></param>
        /// <returns></returns>
        public List<AppFunctionSwitchRev> ListAppFunctionSwitchRev(string AppName, int FunctionID)
        {
            return _functionSwitchService.ListAppFunctionSwitchRev(AppName, FunctionID);
        }

        /// <summary>
        /// 取得App預約開關
        /// </summary>
        /// <param name="RevID"></param>
        /// <returns></returns>
        public AppFunctionSwitchRev GetAppFunctionSwitchRev(int RevID)
        {
            return _functionSwitchService.GetAppFunctionSwitchRev(RevID);
        }

        /// <summary>
        /// 新增內部後台預約開關
        /// </summary>
        /// <param name="model"></param>
        /// <param name="Modifier"></param>
        /// <param name="RealIP"></param>
        /// <param name="ProxyIP"></param>
        /// <returns></returns>
        public BaseResult AddAppFunctionSwitchRev(AddAppFunctionSwitchRevVM model, string Modifier, long RealIP, long ProxyIP)
        {
            var result = new BaseResult();
            result.SetError();

            var switchStatusRev = model.AppFunctionSwitchRev;
            if (switchStatusRev == null)
            {
                result.RtnMsg = "請填寫預設資訊";
                return result;
            }

            DateTime startDate;
            if (!DateTime.TryParse($"{model.SwitchStartDate} {model.SwitchStartTime}", out startDate))
            {
                result.RtnMsg = "請選擇開關預設開始時間";
                return result;
            }

            DateTime endDate;
            if (!DateTime.TryParse($"{model.SwitchEndDate} {model.SwitchEndTime}", out endDate))
            {
                result.RtnMsg = "請選擇開關預設關閉時間，關閉時間到了後，功能狀態將依照主開關狀態變動";
                return result;
            }

            if (startDate <= DateTime.Now || startDate >= endDate)
            {
                result.RtnMsg = "請確認開啟/關閉維護時間(不可晚於當下時間)";
                return result;
            }

            switchStatusRev.StartDate = startDate;
            switchStatusRev.EndDate = endDate;

            return _functionSwitchService.AddAppFunctionSwitchRev(switchStatusRev, Modifier, RealIP, ProxyIP);
        }

        /// <summary>
        /// App開關歷程
        /// </summary>
        /// <param name="AppName"></param>
        /// <param name="FunctionID"></param>
        /// <returns></returns>
        public List<AppFunctionSwitchLog> ListAppFunctionSwitchLog(string AppName, int FunctionID)
        {
            return _functionSwitchService.ListAppFunctionSwitchLog(AppName, FunctionID);
        }

        /// <summary>
        /// 刪除App預約開關
        /// </summary>
        /// <param name="RevID"></param>
        /// <param name="Modifier"></param>
        /// <param name="RealIP"></param>
        /// <param name="ProxyIP"></param>
        /// <returns></returns>
        public BaseResult DeleteAppFunctionSwitchRev(int RevID, string Modifier, long RealIP, long ProxyIP)
        {
            return _functionSwitchService.DeleteAppFunctionSwitchRev(RevID, Modifier, RealIP, ProxyIP);
        }
    }
}
