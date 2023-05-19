using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICP.Modules.Mvc.Admin.Models.MemberModels
{
    /// <summary>
    /// 更新法定代理人資料
    /// </summary>
    public class UpdateTeenagersLegalDetailModel
    {
        /// <summary>
        /// 法代資料審核身份證檔案1
        /// </summary>
        public string IDNOFile1 { get; set; }

        /// <summary>
        /// 法代資料審核身份證檔案2
        /// </summary>
        public string IDNOFile2 { get; set; }

        /// <summary>
        /// 法代資料審核文件檔案1
        /// </summary>
        public string FilePath1 { get; set; }

        /// <summary>
        /// 法代資料審核文件檔案2
        /// </summary>
        public string FilePath2 { get; set; }

        /// <summary>
        /// 法代資料審核文件檔案3
        /// </summary>
        public string FilePath3 { get; set; }

        /// <summary>
        /// 法代資料審核文件檔案4
        /// </summary>
        public string FilePath4 { get; set; }

        /// <summary>
        /// 法代資料審核文件檔案5
        /// </summary>
        public string FilePath5 { get; set; }

        /// <summary>
        /// 法代資料審核文件檔案6
        /// </summary>
        public string FilePath6 { get; set; }
    }
}
