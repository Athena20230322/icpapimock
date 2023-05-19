using ICP.Infrastructure.Core.Extensions;
using ICP.Infrastructure.Core.Models;
using ICP.Library.Repositories.MemberRepositories;
using ICP.Modules.Api.Member.Models.MemberInfo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace ICP.Modules.Api.Member.Services
{
    public class AppManagementService: Library.Services.MemberApi.AppManagementApiService
    {
        MemberConfigRepository _configRepository;

        public AppManagementService(
            MemberConfigRepository configRepository
            )
        {
            _configRepository = configRepository;
        }

        public DataResult<GetAppXmlSettingResult> GetAppXmlSetting(long MID, GetAppXmlSettingRequest model,string APPXMLPath)
        {
            var result = new DataResult<GetAppXmlSettingResult>();
            result.SetError();

            if (string.IsNullOrWhiteSpace(model.XmlVersion)) { model.XmlVersion = "1"; }
           
            string XMLTESTMIDName = GetXMLTESTMIDName(model.XmlVersion);
            string TestMIDPath = GetXMLPath(APPXMLPath, "TestMID", XMLTESTMIDName);

            string XMLFileName = GetXMLFileName(model.XmlVersion);
            string PublishXMLPath = GetXMLPath(APPXMLPath, "XMLData", XMLFileName);
            string TestXmlPath = GetXMLPath(APPXMLPath, "TestXML", XMLFileName);

            //是否有設定測試 MID
            bool isTestMID = false;

            //是否符合測試 MID
            bool isMatchMID = false;

            string TestXML = null;
            string PublishXML = null;
            string RtnXMLXML = null;

            #region 讀取 XML
            try
            {
                //讀取 測試 mid 判斷是否在測試 MID 名單內
                if (isTestMID && System.IO.File.Exists(TestMIDPath))
                {
                    string TestMID = System.IO.File.ReadAllText(TestMIDPath);
                    if (!string.IsNullOrWhiteSpace(TestMID))
                    {
                        isTestMID = true;
                        var TestMIDArray = TestMID.Split('#');
                        isMatchMID = TestMIDArray.Contains(MID.ToString());
                    }
                }

                //讀取 正式 XML
                if (System.IO.File.Exists(PublishXMLPath))
                {
                    var XMLData = new XmlDocument();
                    XMLData.Load(PublishXMLPath);
                    PublishXML = XMLData.OuterXml;
                }

                //讀取 測試 XML
                if (System.IO.File.Exists(TestXmlPath))
                {
                    TestXML = System.IO.File.ReadAllText(TestXmlPath);
                    if (!string.IsNullOrWhiteSpace(TestXML))
                    {
                        var TestXMLData = new XmlDocument();
                        TestXMLData.Load(TestXmlPath);
                        TestXML = TestXMLData.OuterXml;
                    }
                }
            }
            catch //(Exception ex)
            {
                result.RtnMsg = "System Error";
                return result;
            }
            #endregion

            #region XML 選擇
            //"取得XML選單API：
            //1.撈取傳入版號的XML，若有傳入MID則先看是否於測試MID名單內，若有則回傳測試XML，無則回傳正式XML。
            //2.新增的XML：「正式XML」為空值，若未設定「測試MID」時，則表示全部通用，則一律回傳測試XML。若有設定「測試MID」但API傳入的MID未在名單內，則回傳ERROR。"

            //測試 XML 為空時使用 正式 XML
            if (string.IsNullOrWhiteSpace(TestXML))
            {
                TestXML = PublishXML;
            }

            //在測試名單內
            if (isMatchMID)
            {
                RtnXMLXML = TestXML;
            }
            //有設定測試 MID, 但不在名單中, 只能使用正式 XML
            else if (isTestMID)
            {
                RtnXMLXML = PublishXML;
            }
            //未設定測試 MID, 優先使用正式, 為空再用測試
            else
            {
                if (!string.IsNullOrWhiteSpace(PublishXML))
                {
                    RtnXMLXML = PublishXML;
                }
                else
                {
                    RtnXMLXML = TestXML;
                }
            }

            if (string.IsNullOrWhiteSpace(RtnXMLXML))
            {
                result.RtnMsg = "ERROR";
                return result;
            }
            #endregion

            var RtnData = new GetAppXmlSettingResult
            {
               xml = RtnXMLXML
            };

            result.SetSuccess(RtnData);
            return result;
        }
    }
}
