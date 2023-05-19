using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICP.Library.Models.AccountLinkApi
{
    public class HSMSettingModel
    {
        /// <summary>
        /// 簽章的KeyLabel
        /// </summary>
        public string HSMKeyLabel { get; set; } = string.Empty;
        /// <summary>
        /// 3DES的KeyLabel
        /// </summary>
        public string HSMTripleDESKeyLabel { get; set; } = string.Empty;
        /// <summary>
        /// 憑證序號
        /// </summary>
        public string HSMCERTSN { get; set; } = string.Empty;
    }
}
