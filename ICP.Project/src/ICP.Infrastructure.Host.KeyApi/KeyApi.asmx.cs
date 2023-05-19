using ICP.Infrastructure.Host.KeyApi.Services;
using System.Configuration;
using System.Web.Services;

namespace ICP.Infrastructure.Host.KeyApi
{
    /// <summary>
    ///KeyApi 的摘要描述
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // 若要允許使用 ASP.NET AJAX 從指令碼呼叫此 Web 服務，請取消註解下列一行。
    // [System.Web.Script.Services.ScriptService]
    public class KeyApi : System.Web.Services.WebService
    {

        [WebMethod]
        public string GetValue(string key)
        {
            var config = ConfigurationManager.AppSettings[key];
            return (config != null) ? config.ToString() : string.Empty;
        }

        #region 3DES

        /// <summary>
        /// 3DES加密
        /// </summary>
        /// <param name="rawData">欲加密的字串</param>
        /// <param name="keyLabel">key標籤</param>
        /// <returns></returns>
        [WebMethod]
        public string TripleDESEncrypt(string rawData, string keyLabel)
        {
            string result = new TripleDESService(keyLabel).Encrypt(rawData);

            return result;
        }

        /// <summary>
        /// 3DES解密
        /// </summary>
        /// <param name="encryptedHex">欲解密的16進位字串</param>
        /// <param name="keyLabel">key標籤</param>
        /// <returns></returns>
        [WebMethod]
        public string TripleDESDecrypt(string encryptedHex, string keyLabel)
        {
            string result = new TripleDESService(keyLabel).Decrypt(encryptedHex);

            return result;
        }

        /// <summary>
        /// 3DES加密
        /// </summary>
        /// <param name="rawData">欲加密的字串</param>
        /// <param name="keyHex">16進位的key</param>
        /// <returns></returns>
        [WebMethod]
        public string TripleDESEncryptByKey(string rawData, string keyHex)
        {
            string result = new TripleDESService("").Encrypt(rawData, keyHex);

            return result;
        }

        /// <summary>
        /// 3DES解密
        /// </summary>
        /// <param name="encryptedHex">欲解密的16進位字串</param>
        /// <param name="keyHex">16進位的key</param>
        /// <returns></returns>
        [WebMethod]
        public string TripleDESDecryptByKey(string encryptedHex, string keyHex)
        {
            string result = new TripleDESService("").Decrypt(encryptedHex, keyHex);

            return result;
        }

        #endregion

        #region sign

        /// <summary>
        /// 簽章
        /// </summary>
        /// <param name="bankCode">銀行代碼</param>
        /// <param name="data">欲簽章的字串</param>
        /// <param name="keyLabel">key標籤</param>
        /// <param name="certId">憑證編號</param>
        /// <returns></returns>
        [WebMethod]
        public string Sign(string bankCode, string data, string keyLabel, string certId)
        {
            string result = new KmsP7SignService(bankCode, keyLabel, certId).Sign(data);

            return result;
        }

        /// <summary>
        /// 驗證簽章
        /// </summary>
        /// <param name="bankCode">銀行代碼</param>
        /// <param name="signDataHex">16進位的簽章字串</param>
        /// <param name="keyLabel">key標籤</param>
        /// <param name="certId">憑證編號</param>
        /// <returns></returns>
        [WebMethod]
        public bool VerifySign(string bankCode, string signDataHex, string keyLabel, string certId)
        {
            bool result = new KmsP7SignService(bankCode, keyLabel, certId).VerifySign(signDataHex);

            return result;
        }

        #endregion

    }
}
