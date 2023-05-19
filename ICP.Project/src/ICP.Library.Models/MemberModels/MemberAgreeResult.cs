using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICP.Library.Models.MemberModels
{
    public class MemberAgreeResult
    {
        /// <summary>
        /// 同意事項編號
        /// </summary>
        public int AgreeType { get; set; }

        /// <summary>
        /// 同意事項名稱
        /// </summary>
        public string AgreeName { get; set; }

        /// <summary>
        /// 同意事項狀態 0:停用, 1啟用
        /// </summary>
        public int AgreeStatus { get; set; }
    }
}
