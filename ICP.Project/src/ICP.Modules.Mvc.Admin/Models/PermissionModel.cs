using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICP.Modules.Mvc.Admin.Models
{
    public class PermissionModel
    {
        /// <summary>
        /// 功能編號
        /// </summary>
        public int FunctionID { get; set; }

        /// <summary>
        /// 功能群組編號
        /// </summary>
        public int FunctionGroupID { get; set; }

        /// <summary>
        /// 功能層級
        /// </summary>
        public byte FunctionLevel { get; set; }

        /// <summary>
        /// 功能名稱
        /// </summary>
        public String FunctionName { get; set; }

        /// <summary>
        /// 排序編號
        /// </summary>
        public short OrderID { get; set; }

        /// <summary>
        /// 查詢
        /// </summary>
        public bool Query { get; set; }

        /// <summary>
        /// 查詢停用
        /// </summary>
        public bool QueryDisable { get; set; }

        /// <summary>
        /// 新增
        /// </summary>
        public bool Add { get; set; }

        /// <summary>
        /// 新增停用
        /// </summary>
        public bool AddDisable { get; set; }

        /// <summary>
        /// 編輯
        /// </summary>
        public bool Edit { get; set; }

        /// <summary>
        /// 編輯停用
        /// </summary>
        public bool EditDisable { get; set; }

        /// <summary>
        /// 刪除
        /// </summary>
        public bool Delete { get; set; }

        /// <summary>
        /// 刪除停用
        /// </summary>
        public bool DeleteDisable { get; set; }

        /// <summary>
        /// 審核
        /// </summary>
        public bool Check { get; set; }

        /// <summary>
        /// 審核停用
        /// </summary>
        public bool CheckDisable { get; set; }

        /// <summary>
        /// 匯入
        /// </summary>
        public bool Import { get; set; }

        /// <summary>
        /// 匯入停用
        /// </summary>
        public bool ImportDisable { get; set; }

        /// <summary>
        /// 匯出
        /// </summary>
        public bool Export { get; set; }

        /// <summary>
        /// 匯出停用
        /// </summary>
        public bool ExportDisable { get; set; }
    }
}
