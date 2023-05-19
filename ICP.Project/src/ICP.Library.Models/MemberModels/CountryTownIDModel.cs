using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICP.Library.Models.MemberModels
{
    public class CountryTownIDModel
    {
        /// <summary>
        /// 縣市代碼
        /// </summary>
        public string CountyID { get; set; }

        /// <summary>
        /// 縣市名稱
        /// </summary>
        public string CountyName { get; set; }

        /// <summary>
        /// 鄉鎮市區代碼
        /// </summary>
        public string TownID { get; set; }

        /// <summary>
        /// 鄉鎮市區名稱
        /// </summary>
        public string TownName { get; set; }

        /// <summary>
        /// 郵遞區號
        /// </summary>
        public string ZipCode { get; set; }
    }
}
