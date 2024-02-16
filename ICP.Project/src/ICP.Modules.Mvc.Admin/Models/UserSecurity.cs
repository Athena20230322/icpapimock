using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICP.Modules.Mvc.Admin.Models
{
    public class UserSecurity
    {
        /// <summary>
        /// 使用者編號
        /// </summary>
        public int UserID { get; set; }

        /// <summary>
        /// 最後修改密碼時間
        /// </summary>
        public DateTime? LastChangePwdAt { get; set; }

        /// <summary>
        /// 認證碼
        /// </summary>
        public string AuthCode { get; set; }

        /// <summary>
        /// 認證時間
        /// </summary>
        public DateTime? AuthTime { get; set; }

        /// <summary>
        /// 忘記密碼Token
        /// </summary>
        public string ForgetPwdToken { get; set; }
    }
}
