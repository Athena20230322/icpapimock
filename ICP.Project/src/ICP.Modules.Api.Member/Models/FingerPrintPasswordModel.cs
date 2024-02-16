using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ICP.Modules.Api.Member.Models
{
    public class FingerPrintPasswordModel
    {
        /// <summary>
        /// 裝置ID
        /// </summary>
        public string DeviceID { get; set; }

        /// <summary>
        /// 會員編號
        /// </summary>
        public long MID { get; set; }

        /// <summary>
        /// 密碼
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// RealIP
        /// </summary>
        public long RealIP { get; set; }

        /// <summary>
        /// ProxyIP
        /// </summary>
        public long ProxyIP { get; set; }

        /// <summary>
        /// 是否啟用(0: 停用, 1: 啟用)
        /// </summary>
        public bool Status { get; set; }
    }
}