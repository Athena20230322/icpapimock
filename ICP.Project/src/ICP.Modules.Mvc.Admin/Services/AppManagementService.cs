using ICP.Infrastructure.Abstractions.Logging;
using ICP.Infrastructure.Core.Extensions;
using ICP.Infrastructure.Core.Models;
using ICP.Modules.Mvc.Admin.Models;
using ICP.Modules.Mvc.Admin.Models.ViewModels;
using ICP.Modules.Mvc.Admin.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace ICP.Modules.Mvc.Admin.Services
{
    public class AppManagementService: Library.Services.MemberApi.AppManagementApiService
    {
        ILogger<AppManagementRepository> _logger;
        AppManagementRepository _appManagementRepository;

        public AppManagementService(
            ILogger<AppManagementRepository> logger,
            AppManagementRepository appManagementRepository)
        {
            _logger = logger;
            _appManagementRepository = appManagementRepository;
        }

        /// <summary>
        /// 新增APP設定
        /// </summary>
        /// <param name="model">APP設定</param>
        /// <param name="Creator">建立者</param>
        /// <returns></returns>
        public BaseResult AddAPPSetting(APPSetting model, string Creator)
        {
            var result = new BaseResult();
            result.SetError();

            if (!model.IsValid())
            {
                result.SetFormatError(model.GetFirstErrorMessage());
                return result;
            }

            return _appManagementRepository.AddAPPSetting(model, Creator);
        }

        /// <summary>
        /// 取得APP設定
        /// </summary>
        /// <param name="VersionNo">版本號</param>
        /// <returns></returns>
        public DataResult<APPSetting> GetAPPSetting(byte VersionNo)
        {
            var result = new DataResult<APPSetting>();
            result.SetError();

            var rtnData = _appManagementRepository.GetAPPSetting(VersionNo);
            if (rtnData == null) return result;

            result.SetSuccess(rtnData);
            return result;
        }

        /// <summary>
        /// 取得目前最大版本號
        /// </summary>
        /// <returns></returns>
        public byte GetAPPSettingMaxVersion()
        {
            return _appManagementRepository.GetAPPSettingMaxVersion();
        }

        /// <summary>
        /// 取得APP設定清單
        /// </summary>
        /// <returns></returns>
        public List<APPSettingQueryResult> ListAPPSetting()
        {
            return _appManagementRepository.ListAPPSetting();
        }

        /// <summary>
        /// 發佈APP設定
        /// </summary>
        /// <param name="VersionNo">版本號</param>
        /// <param name="Modifier">發佈者</param>
        /// <returns></returns>
        public BaseResult PublishAPPSetting(byte VersionNo, string Modifier)
        {
            return _appManagementRepository.PublishAPPSetting(VersionNo, Modifier);
        }

        /// <summary>
        /// 更新APP設定
        /// </summary>
        /// <param name="model">APP設定</param>
        /// <param name="Modifier">修改者</param>
        /// <returns></returns>
        public BaseResult UpdateAPPSetting(APPSetting model, string Modifier)
        {
            var result = new BaseResult();
            result.SetError();

            if (!model.IsValid())
            {
                result.SetFormatError(model.GetFirstErrorMessage());
                return result;
            }

            return _appManagementRepository.UpdateAPPSetting(model, Modifier);
        }

        /// <summary>
        /// 儲存 APP設定 XML
        /// </summary>
        /// <param name="APPXMLPath">XML根路徑</param>
        /// <param name="model">APP設定</param>
        /// <param name="IsPublish">是否為發佈產檔</param>
        /// <returns></returns>
        public BaseResult SaveAPPSettingXml(string APPXMLPath, APPSetting model, bool IsPublish = false)
        {
            var result = new BaseResult();
            result.SetError();

            var xmlDoc = new XmlDocument();

            string XMLFileName = GetXMLFileName(model.VersionNo.ToString());
            string XMLTESTMIDName = GetXMLTESTMIDName(model.VersionNo.ToString());

            try
            {
                string XMLTESTMIDPath = GetXMLPath(APPXMLPath, "TestMID", XMLTESTMIDName);
                string XMLFilePath;

                //發佈
                if (IsPublish)
                {
                    xmlDoc.LoadXml(model.XMLData);
                    XMLFilePath = GetXMLPath(APPXMLPath, "XMLData", XMLFileName);
                }
                else
                {
                    xmlDoc.LoadXml(model.TestXMLData);
                    XMLFilePath = GetXMLPath(APPXMLPath, "TestXML", XMLFileName);
                }

                //建立目錄
                string dirXMLFilePath = System.IO.Path.GetDirectoryName(XMLFilePath);
                if (!System.IO.File.Exists(dirXMLFilePath)) System.IO.Directory.CreateDirectory(dirXMLFilePath);

                //儲存測試 XML
                xmlDoc.Save(XMLFilePath);

                //測試 MID
                if (!string.IsNullOrWhiteSpace(model.TestMID))
                {
                    //建立目錄
                    string dirXMLTESTMIDPath = System.IO.Path.GetDirectoryName(XMLTESTMIDPath);
                    if (!System.IO.File.Exists(dirXMLTESTMIDPath)) System.IO.Directory.CreateDirectory(dirXMLTESTMIDPath);

                    //儲存測試MID
                    System.IO.File.WriteAllText(XMLTESTMIDPath, model.TestMID);
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "APP設定產檔更新失敗");
                return result;
            }

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
            return _appManagementRepository.ListAPPXMLSettingLog(VersionNo);
        }
    }
}
