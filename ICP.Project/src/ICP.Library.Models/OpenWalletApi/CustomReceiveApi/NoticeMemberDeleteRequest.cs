using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICP.Library.Models.OpenWalletApi.CustomReceiveApi
{
    public class NoticeMemberDeleteRequest: BaseCustomReceiveApiRequest
    {
        /// <summary>
        /// OP會員編號
        /// </summary>
        public string mid { get; set; }
    }
}
