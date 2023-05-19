using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ICP.Host.APIService.Models
{
    public class AddSMS
    {
        /// <summary>
        /// 手機號碼
        /// </summary>
        public string Phone { get; set; }

        /// <summary>
        /// 簡訊內容
        /// </summary>
        public string MsgData { get; set; }

        /// <summary>
        /// 國內外簡訊
        /// 0 = 國內
        /// 1 = 國外
        /// </summary>
        public byte SmsType { get; set; }

        /// <summary>
        /// 店信閘道
        /// 0 = 三竹
        /// 1 = 遠傳
        /// </summary>
        public int Gateway { get; set; }

        /// <summary>
        /// 發送者
        /// </summary>
        public string Sender { get; set; }
    }
}