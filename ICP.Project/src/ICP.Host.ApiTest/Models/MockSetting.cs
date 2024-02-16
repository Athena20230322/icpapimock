using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ICP.Host.ApiTest.Models
{
    public class MockSetting
    {
        public string UserCode { get; set; }

        public long MID { get; set; }

        /// <summary>
        /// 客戶端RSA私鑰
        /// </summary>
        public string ClientPrivateKey { get; set; }

        /// <summary>
        /// 客戶端RSA公鑰
        /// </summary>
        public string ClientPublicKey { get; set; }

        /// <summary>
        /// 伺服器端匹配RSA金鑰唯一識別碼
        /// </summary>
        public long ServerPubCertID { get; set; }

        /// <summary>
        /// 伺服器端匹配RSA公鑰
        /// </summary>
        public string ServerPubCert { get; set; }

        /// <summary>
        /// 伺服器端匹配AES加密金鑰唯一識別碼
        /// </summary>
        public long AES_CertId { get; set; }

        /// <summary>
        /// AES加密金鑰
        /// </summary>
        public string AES_Key { get; set; }

        /// <summary>
        /// AES金鑰向量
        /// </summary>
        public string AES_IV { get; set; }

        /// <summary>
        /// 到期日時間 2019/01/01 00:00:00
        /// </summary>
        public DateTime ExpireDate { get; set; }
    }
}