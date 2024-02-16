using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICP.Library.Models.MerchantModels
{
    public class MerchantCertificateModel
    {
        /// <summary>
        /// 會員/廠商編號
        /// </summary>
        public long MID { get; set; }

        /// <summary>
        /// 會員/廠商的AES_Key
        /// </summary>
        public string AES_Key { get; set; }

        /// <summary>
        /// 會員/廠商的AES_IV
        /// </summary>
        public string AES_IV { get; set; }

        /// <summary>
        /// iCash System對應會員/廠商的Private私鑰
        /// </summary>
        public string PrivateCert { get; set; }

        /// <summary>
        /// 會員/廠商的公鑰
        /// </summary>
        public string ClientPublicCert { get; set; }

        /// <summary>
        /// 金鑰編號
        /// </summary>
        public long ClientCertId { get; set; }
    }
}
