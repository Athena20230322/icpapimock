using ICP.Infrastructure.Core.Models;
using System;
using System.ComponentModel.DataAnnotations;

namespace ICP.Modules.Mvc.Admin.Models.Announcement
{
    /// <summary>
    /// 訊息公告清單Res
    /// </summary>
    public class ListAnnounceDbRes : BaseListModel
    {
        /// <summary>
        /// 公告編號
        /// </summary>
        public int NID { get; set; }

        /// <summary>
        /// 類別
        /// </summary>
        public string CategoryName { get; set; }

        /// <summary>
        /// 標題
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// 建立時間
        /// </summary>
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd}")]
        public DateTime CreateDate { get; set; }

        public string CreateDateTime
        {
            get
            {
                return CreateDate.ToString("HH:mm");
            }
        }

        /// <summary>
        /// 最後修改時間
        /// </summary>
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd}")]
        public DateTime ModifyDate { get; set; }

        public string ModifyDateTime
        {
            get
            {
                return ModifyDate.ToString("HH:mm");
            }
        }

        /// <summary>
        /// 公告上架日期
        /// </summary>
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd}")]
        public DateTime StartDate { get; set; }

        public string StartDateTime
        {
            get
            {
                return StartDate.ToString("HH:mm");
            }
        }

        /// <summary>
        /// 是否置頂
        /// </summary>
        /// <remarks>0: 預設值, 1: 頂置</remarks>
        public int IsTop { get; set; }

        /// <summary>
        /// 置頂開始時間
        /// </summary>
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd}")]
        public DateTime? IsTopStartDate { get; set; }

        public string IsTopStartDateTime
        {
            get
            {
                if (IsTopStartDate != null)
                {
                    return IsTopStartDate.Value.ToString("HH:mm");
                }
                else
                {
                    return string.Empty;
                }
            }
        }

        /// <summary>
        /// 置頂結束時間
        /// </summary>
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd}")]
        public DateTime? IsTopEndDate { get; set; }

        public string IsTopEndDateTime
        {
            get
            {
                if (IsTopStartDate != null)
                {
                    return IsTopEndDate.Value.ToString("HH:mm");
                }
                else
                {
                    return string.Empty;
                }
            }
        }

        /// <summary>
        /// 審核狀態
        /// </summary>
        /// <remarks>0:待審核 1:審核通過 2: 退件</remarks>
        public int AuthStatus { get; set; }

        /// <summary>
        /// 建立者
        /// </summary>
        public string CreateUser { get; set; }

        /// <summary>
        /// 最後修改者
        /// </summary>
        public string Modifier { get; set; }

        /// <summary>
        /// 審核者
        /// </summary>
        public string AuthUser { get; set; }

        /// <summary>
        /// 審核時間
        /// </summary>
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd}")]
        public DateTime? AuthDate { get; set; }

        public string AuthDateTime
        {
            get
            {
                if (IsTopStartDate != null)
                {
                    return AuthDate.Value.ToString("HH:mm");
                }
                else
                {
                    return string.Empty;
                }
            }
        }
    }
}
