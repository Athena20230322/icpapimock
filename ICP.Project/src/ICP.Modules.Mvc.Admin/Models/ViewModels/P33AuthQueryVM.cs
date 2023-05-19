using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICP.Modules.Mvc.Admin.Models.ViewModels
{
    /// <summary>
    /// P33驗證結果
    /// </summary>
    public class P33AuthQueryVM
    {
        /// <summary>
        /// 驗證成功與否
        /// 0:未驗證
        /// 1:成功
        /// 2:身份證重覆
        /// 3:失敗
        /// </summary>
        public short IsPass { get; set; }
        /// <summary>
        /// 通報案件筆數
        /// </summary>
        public int DataCount { get; set; }
    }
}
