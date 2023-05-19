using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICP.Library.Models.OpenWalletApi.CustomSendApi
{
    /// <summary>
    /// 解綁icash會員
    /// </summary>
    public class UnBindicashAccountRequest: BaseCustomSendApiRequest
    {
        /// <summary>
        /// Op會員編號
        /// </summary>
        public string OpMemberID { get; set; }

        /// <summary>
        /// 愛金卡帳戶
        /// </summary>
        public string IcashAccount { get; set; }
    }
}
