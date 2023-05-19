using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICP.Library.Models.MemberModels
{
    /// <summary>
    /// 郵遞區號
    /// </summary>
    public class ZipCodeModel
    {
        /// <summary>
        /// 區域代碼
        /// 二位數: 縣市編號 , 四位數:鄉鎮市區編號
        /// </summary>
        public string AreaID { get; set; }

        /// <summary>
        /// 區域名稱
        /// </summary>
        public string AreaName { get; set; }

        /// <summary>
        /// 郵遞區號
        /// </summary>
        public string ZipCode { get; set; }

        /// <summary>
        /// 英文縣市鄉鎮區
        /// </summary>
        public string AreaName_EN { get; set; }
    }
}
