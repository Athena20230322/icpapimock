using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICP.Library.Models.OpenWalletApi.CustomSendApi
{
    /// <summary>
    /// OP會員解戶通知
    /// </summary>
    public class CloseOPAccountRequest
    {
        /// <summary>
        /// Op會員編號
        /// </summary>
        public string OpMemberID { get; set; }
    }
}
