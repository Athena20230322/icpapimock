using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICP.Modules.Mvc.Admin.Models.ViewModels
{
    /// <summary>
    /// APP 設定 查詢結果
    /// </summary>
    public class APPSettingQueryResult
    {
        /// <summary>
        /// XML版本號
        /// </summary>
        public byte VersionNo { get; set; }

        /// <summary>
        /// 上線時間
        /// </summary>
        public DateTime? ReleaseTime { get; set; }

        /// <summary>
        /// 更新說明
        /// </summary>
        public string ReleaseNote { get; set; }

        /// <summary>
        /// 最後修改人
        /// </summary>
        public string Modifier { get; set; }

        /// <summary>
        /// 最後修改時間
        /// </summary>
        public DateTime ModifyDate { get; set; }

        /// <summary>
        /// 測試MID
        /// </summary>
        public string TestMID { get; set; }

        /// <summary>
        /// 建立日期
        /// </summary>
        public DateTime CreateDate { get; set; }

        /// <summary>
        /// 建立者
        /// </summary>
        public string Creator { get; set; }
    }
}
