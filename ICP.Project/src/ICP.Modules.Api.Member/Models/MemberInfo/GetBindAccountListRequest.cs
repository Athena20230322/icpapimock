using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICP.Modules.Api.Member.Models.MemberInfo
{
    public class GetBindAccountListRequest
    {
        /// <summary>
        /// 綁定帳戶類別
        /// 1 = 連結扣款
        /// 2 = 餘額提領轉入
        /// </summary>
        public int AccountType { get; set; }
    }
}
