using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICP.Modules.Mvc.Admin.Models.ViewModels
{
    public class APPSettingLog
    {
        /// <summary>
        /// 記錄編號
        /// </summary>
        public long LogID { get; set; }

        /// <summary>
        /// XML版本號
        /// </summary>
        public byte VersionNo { get; set; }

        /// <summary>
        /// 正式XML
        /// </summary>
        public string XMLData { get; set; }

        /// <summary>
        /// 測試XML
        /// </summary>
        public string TestXMLData { get; set; }

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
        /// 測試MID
        /// </summary>
        public string TestMID { get; set; }

        /// <summary>
        /// 最後修改時間
        /// </summary>
        public DateTime CreateDate { get; set; }

        /// <summary>
        /// 動作類型 1:新增 2:修改 3:發佈
        /// </summary>
        public byte ActionType { get; set; }
    }
}
