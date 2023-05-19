using ICP.Library.Models.AuthorizationApi;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICP.Modules.Api.Member.Models.MemberInfo
{
    public class DelAccountBindRequest : BaseAuthorizationApiRequest
    {
        /// <summary>
        /// 刪除帳戶類別
        /// 1 = 連結扣款帳戶
        /// 2 = 提領轉入帳戶
        /// </summary>
        [Required]
        [Range(1, 2)]
        public byte AccountType { get; set; }

        /// <summary>
        /// 銀行代碼
        /// </summary>
        [Required]
        public string BankCode { get; set; }

        /// <summary>
        /// 銀行帳戶編號
        /// </summary>
        [Required]
        public long AccountID { get; set; }
    }
}
