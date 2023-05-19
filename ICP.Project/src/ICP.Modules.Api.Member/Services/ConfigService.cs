using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ICP.Modules.Api.Member.Services
{
    public class ConfigService
    {
        /// <summary>
        /// 取得KeyApi站台的相關AppSetting設定值
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        /// <summary>
        private static string GetAppSettingValue(string key)
        {
            string keyValue = "";
            if (string.IsNullOrWhiteSpace(key))
                return "";

            string requestUrl = ConfigurationManager.AppSettings["KeyApiDomain"].ToString() + "/KeyApi/GetValue";

            HttpWebRequest httpWebRequest = HttpWebRequest.Create(requestUrl) as HttpWebRequest;
            httpWebRequest.Method = "POST";
            httpWebRequest.Timeout = 60000;
            httpWebRequest.ReadWriteTimeout = 60000;
            httpWebRequest.UserAgent = "Mozilla/4.0 (compatible; MSIE 5.5; Windows NT 5.0)";
            httpWebRequest.ContentType = "application/x-www-form-urlencoded";

            using (System.IO.StreamWriter sw = new System.IO.StreamWriter(httpWebRequest.GetRequestStream()))
            {
                sw.Write("KeyName=" + key);
                sw.Flush();
                sw.Close();
            }
            HttpWebResponse httpWebResponse = httpWebRequest.GetResponse() as HttpWebResponse;
            using (System.IO.StreamReader sr = new System.IO.StreamReader(httpWebResponse.GetResponseStream()))
            {
                keyValue = sr.ReadToEnd();
                sr.Close();
            }
            return keyValue;
        }

        //#region API_【APP】新增指紋辨識鎖功能，提供API產生Hash密碼

        ///// <summary>
        ///// FingerPrintPassword KEY
        ///// </summary>
        //public static string FingerPrintPasswordHashKey
        //{
        //    get
        //    {
        //        string retVal = GetAppSettingValue("FingerPrintPasswordHashKey");

        //        if (retVal == null)
        //        {
        //            return "";
        //        }

        //        return retVal;
        //    }
        //}

        ///// <summary>
        ///// FingerPrintPassword KEY
        ///// </summary>
        //public static string FingerPrintPasswordHashIV
        //{
        //    get
        //    {
        //        string retVal = GetAppSettingValue("FingerPrintPasswordHashIV");

        //        if (retVal == null)
        //        {
        //            return "";
        //        }

        //        return retVal;
        //    }
        //}

        //#endregion
    }
}
