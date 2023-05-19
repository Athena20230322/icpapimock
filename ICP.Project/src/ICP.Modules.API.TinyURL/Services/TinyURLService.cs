using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using ICP.Modules.API.TinyURL.Repositories;

namespace ICP.Modules.API.TinyURL.Services
{
    public class TinyURLService
    {
        private TinyURLRepository _tinyURLRepository;
        private CyptRepository _cyptRepository;

        public TinyURLService(TinyURLRepository TinyURLRepository, CyptRepository CyptRepository)
        {
            _tinyURLRepository = TinyURLRepository;
            _cyptRepository = CyptRepository;
        }

        #region AES 解密
        public string AesDecrypt(string URL) {
            return _cyptRepository.TinyURL_AesDecrypt(URL);
        }
        #endregion

        #region AES 加密
        public string AesEncrypt(string URL)
        {
            return _cyptRepository.TinyURL_AesEncrypt(URL);
        }
        #endregion

        #region 轉短網址
        /// <summary>
        /// 新增 TinyURL
        /// </summary>
        /// <param name="webSiteURL">實際網址</param>
        /// <returns>TinyURL</returns>
        public string AddTinyURL(string webSiteURL) {

            return HttpContext.Current.Request.Url.Scheme + "://" + HttpContext.Current.Request.ServerVariables["HTTP_HOST"] + "/" + _tinyURLRepository.AddTinyURL(webSiteURL);
        }
        #endregion

        #region 取得原始網址

        public string GetWebSiteURL(string TinyURL)
        {
            return _tinyURLRepository.GetWebSiteURL(TinyURL);
        }

        #endregion
    }
}
