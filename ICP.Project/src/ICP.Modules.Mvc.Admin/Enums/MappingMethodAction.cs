using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICP.Modules.Mvc.Admin.Enums
{
    public enum MappingMethodAction: int
    {
        /// <summary>
        /// None
        /// </summary>
        None = 0,
        /// <summary>
        /// 查詢
        /// </summary>
        Query = 1,
        /// <summary>
        /// 新增
        /// </summary>
        Add = 2,
        /// <summary>
        /// 編輯
        /// </summary>
        Edit = 4,
        /// <summary>
        /// 刪除
        /// </summary>
        Delete = 8,
        /// <summary>
        /// 審核
        /// </summary>
        Check = 16,
        /// <summary>
        /// 匯入
        /// </summary>
        Import = 32,
        /// <summary>
        /// 匯出
        /// </summary>
        Export = 64
    }
}