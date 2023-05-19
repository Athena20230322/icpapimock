using System;
using ICP.Infrastructure.Core.Models;

namespace ICP.Modules.Mvc.Admin.Models.MemberModels
{
    /// <summary>
    /// 裝置ID管理 Result
    /// </summary>
    public class MemberDeviceIdModel : BaseListModel
    {
        /// <summary>
        /// App DeviceID
        /// </summary>
        public string DeviceID { get; set; }

        /// <summary>
        /// 鎖定狀態(0:解鎖, 1:封鎖)，預設值=1
        /// </summary>
        public int Status { get; set; }

        /// <summary>
        /// 封鎖時間
        /// </summary>
        public DateTime LockDate { get; set; }

        /// <summary>
        /// 封鎖時間
        /// </summary>
        public string LockUser { get; set; }

        /// <summary>
        /// 封鎖原因
        /// </summary>
        public string LockMemo { get; set; }

        /// <summary>
        /// 解鎖日期
        /// </summary>
        public DateTime UnLockDate { get; set; }

        /// <summary>
        /// 解鎖者
        /// </summary>
        public string UnLockUser { get; set; }

        /// <summary>
        /// 解鎖原因
        /// </summary>
        public string UnLockMemo { get; set; }

        /// <summary>
        /// 建立日期
        /// </summary>
        public DateTime CreateDate { get; set; }

        /// <summary>
        /// 建立者
        /// </summary>
        public string CreateUser { get; set; }
    }
}