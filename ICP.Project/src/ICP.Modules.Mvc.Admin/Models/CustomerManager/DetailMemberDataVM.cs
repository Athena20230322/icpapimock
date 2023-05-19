using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICP.Modules.Mvc.Admin.Models.CustomerManager
{
    public class DetailMemberDataVM
    {
        /// <summary>
        /// 電支帳號
        /// </summary>
        public string ICPMID { get; set; }

        /// <summary>
        /// 登入帳號
        /// </summary>
        public string Account { get; set; }
        
        /// <summary>
        /// 會員姓名
        /// </summary>
        public string CName { get; set; }


        /// <summary>
        /// 會員生日
        /// </summary>
        public string Birthday { get; set; }

        /// <summary>
        /// 手機號碼
        /// </summary>
        public string CellPhone { get; set; }

        /// <summary>
        /// 身份證字號
        /// </summary>
        public string IDNO { get; set; }

        /// <summary>
        /// 居留證號
        /// </summary>
        public string UniformID { get; set; }



    }
}
