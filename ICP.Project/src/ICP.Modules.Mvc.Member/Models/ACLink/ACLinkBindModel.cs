using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICP.Modules.Mvc.Member.Models.ACLink
{
    public class ACLinkBindModel
    {
        /// <summary>
        /// 會員編號
        /// </summary>        
        public long MID { get; set; }

        /// <summary>
        /// 身分證字號
        /// </summary>
        public string IDNO { get; set; }

        /// <summary>
        /// 銀行帳號
        /// </summary>
        public string BankAccount { get; set; }

        /// <summary>
        /// 銀行代碼
        /// </summary>
        public string BankCode { get; set; }

        /// <summary>
        /// 會員生日
        /// </summary>
        public string Birth { get; set; }

        /// <summary>
        /// 約定條款同意時間
        /// </summary>
        public string AgreeTime { get; set; }

        /// <summary>
        /// 帳號識別碼
        /// </summary>
        public string INDTAccount { get; set; }

        /// <summary>
        /// 時間戳
        /// </summary>
        public string TimeStamp { get; set; }

        /// <summary>
        /// 網頁識別碼
        /// </summary>
        public string AuthId { get; set; }

        /// <summary>
        /// OTP驗證碼
        /// </summary>
        public string Otp { get; set; }

        /// <summary>
        /// 綁定旗標 (Y:送AccountLink綁定)
        /// </summary>
        public string BindFlag { get; set; }

    }
}
