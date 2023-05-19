using ICP.Infrastructure.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICP.Library.Models.AuthorizationApi
{
    public class BaseAuthorizationApiRequest : ValidatableObject
    {
        /// <summary>
        /// 裝置資訊
        /// </summary>
        public string DeviceInfo { get; set; }

        /// <summary>
        /// 手機序號
        /// </summary>
        public string DeviceID { get; set; }

        /// <summary>
        /// 是否模擬器
        /// 0：否
        /// 1：是
        /// </summary>
        public string IsSimulator { get; set; }

        /// <summary>
        /// 手機作業系統
        /// 1：iOS
        /// 2：Android
        /// </summary>
        public string OS { get; set; }

        /// <summary>
        /// 手機作業系統版本
        /// </summary>
        public string OSVersion { get; set; }

        /// <summary>
        /// 時間戳 格式：2019/01/01 00:00:00 UTC+8
        /// </summary>
        public string Timestamp { get; set; }

        /// <summary>
        /// Framework 版本號
        /// </summary>
        public string Vers { get; set; }
    }
}