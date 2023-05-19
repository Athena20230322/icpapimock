using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICP.Library.Models.OpenWalletApi.WebUIApi
{
    /// <summary>
    /// 同意未成年註冊
    /// </summary>
    public class AgreeRegisterWebUIRequest: BaseAuthWebUIApiRequest
    {
        /// <summary>
        /// 上傳身分証
        /// </summary>
        public class identityCard
        {
            public string ImageFile1 { get; set; }

            public string ImageFile2 { get; set; }
        }

        /// <summary>
        /// 上傳戶籍黱本/戶口名簿
        /// </summary>
        public class householdRegistration
        {
            public string ImageFile1 { get; set; }

            public string ImageFile2 { get; set; }

            public string ImageFile3 { get; set; }

            public string ImageFile4 { get; set; }

            public string ImageFile5 { get; set; }

            public string ImageFile6 { get; set; }
        }

        /// <summary>
        /// 未成年註冊電支帳戶
        /// </summary>
        public string MID { get; set; }

        /// <summary>
        /// 未成年身分証字號
        /// </summary>
        public string UserID { get; set; }

        /// <summary>
        /// 上傳身分証
        /// </summary>
        public identityCard IdentityCard { get; set; }

        /// <summary>
        /// 上傳戶籍黱本/戶口名簿
        /// </summary>
        public householdRegistration HouseholdRegistration { get; set; }
    }
}
