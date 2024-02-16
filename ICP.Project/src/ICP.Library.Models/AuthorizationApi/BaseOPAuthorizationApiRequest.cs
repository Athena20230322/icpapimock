using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICP.Library.Models.AuthorizationApi
{
    public class BaseOPAuthorizationApiRequest : BaseAuthorizationApiRequest
    {
        /// <summary>
        /// 註冊TokenID (帶入M0001回傳LoginTokenID)
        /// </summary>
        [Required]
        public string LoginTokenID { get; set; }
    }
}
