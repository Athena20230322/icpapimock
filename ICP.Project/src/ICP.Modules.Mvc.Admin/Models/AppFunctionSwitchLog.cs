using System;

namespace ICP.Modules.Mvc.Admin.Models
{
    public class AppFunctionSwitchLog
    {
        /// <summary>
        /// 流水號
        /// </summary>
        public long LogID { get; set; }

        /// <summary>
        /// 預約記錄編號
        /// </summary>
        public int RevID { get; set; }

        /// <summary>
        /// APP名稱
        /// </summary>
        public string AppName { get; set; }

        /// <summary>
        /// 功能流水號
        /// </summary>
        public int FunctionID { get; set; }

        /// <summary>
        /// 起始時間
        /// </summary>
        public DateTime StartDate { get; set; }

        /// <summary>
        /// 結束時間
        /// </summary>
        public DateTime EndDate { get; set; }

        /// <summary>
        /// 狀態
        /// 0 = 刪除
        /// 1 = 可使用
        /// </summary>
        public byte Status { get; set; }

        /// <summary>
        /// 建立時間
        /// </summary>
        public DateTime CreateDate { get; set; }

        /// <summary>
        /// 建立人員 : 電支後台為登入帳號,會員為電支帳號
        /// </summary>
        public string CreateUser { get; set; }

        /// <summary>
        /// RealIP
        /// </summary>
        public long RealIP { get; set; }

        /// <summary>
        /// ProxyIP
        /// </summary>
        public long ProxyIP { get; set; }
    }
}
