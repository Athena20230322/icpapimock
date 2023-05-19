using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICP.Library.Models.MemberModels
{
    /// <summary>
    /// 查詢結果
    /// </summary>
    public class MemberNotifyMessageResult
    {
        /// <summary>
        /// 分頁內容
        /// </summary>
        public List<MemberNotifyMessage> Items { get; set; }

        /// <summary>
        /// 總筆數
        /// </summary>
        public int TotalCount { get; set; }

        /// <summary>
        /// 總頁數
        /// </summary>
        public int PageCount { get; set; }
    }
}