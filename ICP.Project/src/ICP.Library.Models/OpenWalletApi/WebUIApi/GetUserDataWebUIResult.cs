using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICP.Library.Models.OpenWalletApi.WebUIApi
{
    public class GetUserDataWebUIResult: BaseWebUIApiResult
    {
        public class userData
        {
            /// <summary>
            /// 未成年註冊電支帳戶
            /// </summary>
            public string MID { get; set; }

            /// <summary>
            /// 未成年註冊姓名
            /// </summary>
            public string UserName { get; set; }

            /// <summary>
            /// 未成年身分証字號
            /// </summary>
            public string UserID { get; set; }

            /// <summary>
            /// 未成年註冊手機
            /// </summary>
            public string UserPhone { get; set; }

            /// <summary>
            /// 未成年註冊時效 yyyy/MM/dd HH:mm:ss
            /// </summary>
            public string ValidDate { get; set; }

            /// <summary>
            /// 未成年是否滿成年
            /// 1.成年
            /// 0.未成年
            /// </summary>
            public string Adult { get; set; }

            /// <summary>
            /// 法定代理人身分別
            /// 1.父母
            /// 2.非父母
            /// 3.單一法定代理人
            /// </summary>
            public string ValidType { get; set; }
        }

        public List<userData> UserData { get; set; }
    }
}
