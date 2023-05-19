using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ICP.Modules.API.TinyURL.Services;

namespace ICP.Modules.API.TinyURL.Commands
{
    public class TinyURLCommand
    {

        private TinyURLService _tinyURLService;

        public TinyURLCommand(TinyURLService TinyURLService) {
            _tinyURLService = TinyURLService;
        }

        #region 產生短網址

        public string GenerateTinyURL(string URL) {

            string sWebSiteURL = System.Web.HttpUtility.UrlDecode(URL, Encoding.UTF8).Replace(" ", "+");
            
            // 解密傳入參數並驗證
            var DeCrypyURL = _tinyURLService.AesDecrypt(sWebSiteURL);

            if (string.IsNullOrEmpty(DeCrypyURL)) {
                return "-1";
            }

            // 產生短網址
           return _tinyURLService.AddTinyURL(DeCrypyURL);

        }

        #endregion

        #region 產生加密網址

        public string EnTinyURL(string URL)
        {

            
            // 解密傳入參數並驗證
            var EnCrypyURL = _tinyURLService.AesEncrypt(URL);

            // 產生短網址
            return EnCrypyURL;

        }

        #endregion

        #region 取得原始網址

        public string GetWebSiteURL(string TinyURL)
        {
            return _tinyURLService.GetWebSiteURL(TinyURL);
        }

        #endregion
    }
}
