using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICP.Modules.Mvc.Admin.Models
{
    public class FunctionCategoryLog : FunctionCatagory
    {
        /// <summary>
        /// 流水號
        /// </summary>
        public long LogID { get; set; }

        /// <summary>
        /// 建立時間
        /// </summary>
        public DateTime CreateDate { get; set; }

        /// <summary>
        /// 建立人員: 電支後台為登入帳號,會員為電支帳號
        /// </summary>
        public string CreateUser { get; set; }

        /// <summary>
        /// RealIP
        /// </summary>
        public long RealIP { get; set; }

        /// <summary>
        /// ProxyIP
        /// </summary>
        public long ProxyIP { get; set; }
    }
}
