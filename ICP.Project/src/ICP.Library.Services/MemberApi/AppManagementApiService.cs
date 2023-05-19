using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace ICP.Library.Services.MemberApi
{
    public class AppManagementApiService
    {
        /// <summary>
        /// 取得XML檔名
        /// </summary>
        /// <param name="VersionNo">版號</param>
        /// <returns></returns>
        protected string GetXMLFileName(string XmlVersion)
        {
            return string.Format("SettingList_v{0}.xml", XmlVersion);
        }

        /// <summary>
        /// 取得測試MID檔名
        /// </summary>
        /// <param name="VersionNo"></param>
        /// <returns></returns>
        protected string GetXMLTESTMIDName(string XmlVersion)
        {
            return string.Format("TestMIDList_v{0}.txt", XmlVersion);
        }

        /// <summary>
        /// 取得 APP設定 XML 檔案路徑
        /// </summary>
        /// <param name="APPXMLPath">XML根路徑</param>
        /// <param name="XMLType">XML類型</param>
        /// <param name="XMLFileName">XML檔名</param>
        /// <returns></returns>
        protected string GetXMLPath(string APPXMLPath, string XMLType, string XMLFileName)
        {
            string dir = string.Format("{0}\\{1}", APPXMLPath.TrimEnd('\\'), XMLType);

            return string.Format("{0}\\{1}", dir, XMLFileName);
        }
    }
}
