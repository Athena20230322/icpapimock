using ICP.Library.Models.AuthorizationApi;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICP.Modules.Api.Member.Models.MemberInfo
{
    public class GetRangeNotifyMessageListRequest : BaseAuthorizationApiRequest
    {
        /// <summary>
        /// 指定取得的ID(包含此ID)
        /// </summary>
        [Required]
        public long MsgID { get; set; }

        /// <summary>
        /// 取得指定取得ID的較新或較舊訊息
        /// 0：較舊
        /// 1：較新
        /// </summary>
        [Range(0, 1)]
        public int? Type { get; set; }

        /// <summary>
        /// 撈取筆數
        /// </summary>
        [Required]
        public int Count { get; set; }
    }
}