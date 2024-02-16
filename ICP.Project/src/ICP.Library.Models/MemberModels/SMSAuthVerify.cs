using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.ComponentModel.DataAnnotations;

namespace ICP.Library.Models.MemberModels
{
    using Infrastructure.Core.Models;

    public class SMSAuthVerify: SMSAuth
    {
        /// <summary>
        /// 驗證碼
        /// </summary>
        [Required]
        public string AuthCode { get; set; }
    }
}
