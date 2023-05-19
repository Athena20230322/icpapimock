using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICP.Library.Models.OpenWalletApi.CustomSendApi
{
    /// <summary>
    /// 綁定icash會員
    /// </summary>
    public class BindicashAccountRequest: BaseCustomSendApiRequest
    {
        /// <summary>
        /// Op會員編號
        /// </summary>
        public string OpMemberID { get; set; }

        /// <summary>
        /// 愛金卡帳戶
        /// </summary>
        public string IcashAccount { get; set; }

        /// <summary>
        /// 愛金卡載具
        /// </summary>
        public string ICPCarrierNum { get; set; }

        /// <summary>
        /// 載具類型
        /// </summary>
        public string CarrierType { get; set; }
    }
}
