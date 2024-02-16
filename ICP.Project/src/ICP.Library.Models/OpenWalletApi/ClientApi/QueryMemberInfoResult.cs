using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICP.Library.Models.OpenWalletApi.ClientApi
{
    public class QueryMemberInfoResult: BaseClientApiResult
    {
        public string mid { get; set; }

        /// <summary>
        /// 會員手機
        /// </summary>
        public string phone { get; set; }

        /// <summary>
        /// 會員信箱
        /// </summary>
        public string email { get; set; }

        /// <summary>
        /// 會員姓名
        /// </summary>
        public string name { get; set; }

        /// <summary>
        /// 地址縣市
        /// </summary>
        public string homeCity { get; set; }

        /// <summary>
        /// 生日
        /// </summary>
        public string birthday { get; set; }

        /// <summary>
        /// 性別
        /// </summary>
        public string gender { get; set; }
    }
}
