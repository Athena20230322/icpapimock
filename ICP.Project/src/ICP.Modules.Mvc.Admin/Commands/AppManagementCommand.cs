using ICP.Infrastructure.Core.Extensions;
using ICP.Infrastructure.Core.Models;
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
    public class AppManagementCommand
    {
        AppManagementService _appManagementService;

        public AppManagementCommand(AppManagementService appManagementService)
        {
            _appManagementService = appManagementService;
        }

        /// <summary>
        /// 新增APP設定
        /// </summary>
        /// <param name="model">APP設定</param>
        /// <param name="APPXMLPath">APP設定 XML路徑</param>
        /// <param name="Creator">建立者</param>
        /// <returns></returns>
        public BaseResult AddAPPSetting(APPSetting model, string APPXMLPath, string Creator)
        {
            var addResult = _appManagementService.AddAPPSetting(model, Creator);
            if (!addResult.IsSuccess)
            {
                return addResult;
            }

            var saveResult = _appManagementService.SaveAPPSettingXml(APPXMLPath, model);
            if (!saveResult.IsSuccess)
            {
                return saveResult;
            }

            var result = new BaseResult();
            result.SetSuccess();
            return result;
        }

        /// <summary>
        /// 取得APP設定
        /// </summary>
        /// <param name="VersionNo">版本號</param>
        /// <returns></returns>
        public DataResult<APPSetting> GetAPPSetting(byte VersionNo)
        {
            return _appManagementService.GetAPPSetting(VersionNo);
        }

        /// <summary>
        /// 取得目前最大版本號
        /// </summary>
        /// <returns></returns>
        public byte GetAPPSettingMaxVersion()
        {
            return _appManagementService.GetAPPSettingMaxVersion();
        }

        /// <summary>
        /// 取得APP設定清單
        /// </summary>
        /// <returns></returns>
        public List<APPSettingQueryResult> ListAPPSetting()
        {
            return _appManagementService.ListAPPSetting();
        }

        /// <summary>
        /// 發佈APP設定
        /// </summary>
        /// <param name="VersionNo">版本號</param>
        /// <param name="APPXMLPath">APP設定 XML路徑</param>
        /// <param name="Modifier">發佈者</param>
        /// <returns></returns>
        public BaseResult PublishAPPSetting(byte VersionNo, string APPXMLPath, string Modifier)
        {
            var publishResult = _appManagementService.PublishAPPSetting(VersionNo, Modifier);
            if (!publishResult.IsSuccess)
            {
                return publishResult;
            }

            var getResult = GetAPPSetting(VersionNo);
            var model = getResult.RtnData;

            var saveResult = _appManagementService.SaveAPPSettingXml(APPXMLPath, model, IsPublish: true);
            if (!saveResult.IsSuccess)
            {
                return saveResult;
            }

            var result = new BaseResult();
            result.SetSuccess();
            return result;
        }

        /// <summary>
        /// 更新APP設定
        /// </summary>
        /// <param name="model">APP設定</param>
        /// <param name="APPXMLPath">APP設定 XML路徑</param>
        /// <param name="Modifier">修改者</param>
        /// <returns></returns>
        public BaseResult UpdateAPPSetting(APPSetting model, string APPXMLPath, string Modifier)
        {
            var updateResult = _appManagementService.UpdateAPPSetting(model, Modifier);
            if (!updateResult.IsSuccess)
            {
                return updateResult;
            }

            var saveResult = _appManagementService.SaveAPPSettingXml(APPXMLPath, model);
            if (!saveResult.IsSuccess)
            {
                return saveResult;
            }

            var result = new BaseResult();
            result.SetSuccess();
            return result;
        }

        /// <summary>
        /// 取得修改歷程
        /// </summary>
        /// <param name="VersionNo">版本號</param>
        /// <returns></returns>
        public List<APPSettingLog> ListAPPXMLSettingLog(byte VersionNo)
        {
            return _appManagementService.ListAPPXMLSettingLog(VersionNo);
        }
    }
}
