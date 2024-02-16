using ICP.Infrastructure.Core.Models;
using ICP.Modules.Mvc.Admin.Models;
using ICP.Modules.Mvc.Admin.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICP.Modules.Mvc.Admin.Services
{
    public class FunctionSwitchService
    {
        FunctionSwitchRepository _functionSwitchRepository;

        public FunctionSwitchService(
            FunctionSwitchRepository functionSwitchRepository
            )
        {
            _functionSwitchRepository = functionSwitchRepository;
        }

        /// <summary>
        /// 取得內部後台功能開關
        /// </summary>
        /// <param name="FunctionName"></param>
        /// <returns></returns>
        public List<FunctionCatagory> ListFunctionCategory(string FunctionName)
        {
            return _functionSwitchRepository.ListFunctionCategory(FunctionName);
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
            return _functionSwitchRepository.UpdateFunctionCategoryStatus(FunctionID, Status, Modifier, RealIP, ProxyIP);
        }

        /// <summary>
        /// 取得內部後台預約開關列表
        /// </summary>
        /// <param name="FunctionID"></param>
        /// <returns></returns>
        public List<FunctionCategoryStatusRev> ListFunctionCategoryStatusRev(int FunctionID)
        {
            return _functionSwitchRepository.ListFunctionCategoryStatusRev(FunctionID);
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
            return _functionSwitchRepository.UpdateAppFunctionSwitch(model, Modifier, RealIP, ProxyIP);
        }

        /// <summary>
        /// 取得內部後台預約開關
        /// </summary>
        /// <param name="RevID"></param>
        /// <returns></returns>
        public FunctionCategoryStatusRev GetFunctionCategoryStatusRev(int RevID)
        {
            return _functionSwitchRepository.GetFunctionCategoryStatusRev(RevID);
        }

        /// <summary>
        /// 新增內部後台預約開關
        /// </summary>
        /// <param name="model"></param>
        /// <param name="Modifier"></param>
        /// <param name="RealIP"></param>
        /// <param name="ProxyIP"></param>
        /// <returns></returns>
        public BaseResult AddFunctionCategoryStatusRev(FunctionCategoryStatusRev model, string Modifier, long RealIP, long ProxyIP)
        {
            return _functionSwitchRepository.AddFunctionCategoryStatusRev(model, Modifier, RealIP, ProxyIP);
        }

        /// <summary>
        /// 取得權限設定歷程
        /// </summary>
        /// <param name="FunctionID"></param>
        /// <returns></returns>
        public List<FunctionCategoryLog> ListFunctionCategoryLog(int FunctionID)
        {
            return _functionSwitchRepository.ListFunctionCategoryLog(FunctionID);
        }

        /// <summary>
        /// 刪除內部後台預約開關
        /// </summary>
        /// <param name="RevID"></param>
        /// <returns></returns>
        public BaseResult DeleteFunctionCategoryStatusRev(int RevID, string Modifier, long RealIP, long ProxyIP)
        {
            return _functionSwitchRepository.DeleteFunctionCategoryStatusRev(RevID, Modifier, RealIP, ProxyIP);
        }

        /// <summary>
        /// 取得APP預約開關列表
        /// </summary>
        /// <param name="AppName"></param>
        /// <param name="FunctionID"></param>
        /// <returns></returns>
        public List<AppFunctionSwitchRev> ListAppFunctionSwitchRev(string AppName, int FunctionID)
        {
            return _functionSwitchRepository.ListAppFunctionSwitchRev(AppName, FunctionID);
        }

        /// <summary>
        /// 取得App預約開關
        /// </summary>
        /// <param name="RevID"></param>
        /// <returns></returns>
        public AppFunctionSwitchRev GetAppFunctionSwitchRev(int RevID)
        {
            return _functionSwitchRepository.GetAppFunctionSwitchRev(RevID);
        }

        /// <summary>
        /// 新增App預約開關
        /// </summary>
        /// <param name="model"></param>
        /// <param name="Modifier"></param>
        /// <param name="RealIP"></param>
        /// <param name="ProxyIP"></param>
        /// <returns></returns>
        public BaseResult AddAppFunctionSwitchRev(AppFunctionSwitchRev model, string Modifier, long RealIP, long ProxyIP)
        {
            return _functionSwitchRepository.AddAppFunctionSwitchRev(model, Modifier, RealIP, ProxyIP);
        }

        /// <summary>
        /// App開關歷程
        /// </summary>
        /// <param name="AppName"></param>
        /// <param name="FunctionID"></param>
        /// <returns></returns>
        public List<AppFunctionSwitchLog> ListAppFunctionSwitchLog(string AppName, int FunctionID)
        {
            return _functionSwitchRepository.ListAppFunctionSwitchLog(AppName, FunctionID);
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
            return _functionSwitchRepository.DeleteAppFunctionSwitchRev(RevID, Modifier, RealIP, ProxyIP);
        }
    }
}
