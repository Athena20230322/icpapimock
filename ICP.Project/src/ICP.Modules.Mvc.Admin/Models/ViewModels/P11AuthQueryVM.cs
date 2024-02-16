using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICP.Modules.Mvc.Admin.Models.ViewModels
{
    /// <summary>
    /// P11驗證結果
    /// </summary>
    public class P11AuthQueryVM
    {
        /// <summary>
        /// 是否通過驗證
        /// 0:預設值(待驗證)
        /// 1:驗證成功
        /// 2:驗證不通過
        /// </summary>
        public short IsPass { get; set; }
        /// <summary>
        /// 驗證結果代號(Y/N)
        /// </summary>
        public string Answer { get; set; }
        /// <summary>
        /// 驗證結果說明
        /// </summary>
        public string Result { get; set; }
    }
}
