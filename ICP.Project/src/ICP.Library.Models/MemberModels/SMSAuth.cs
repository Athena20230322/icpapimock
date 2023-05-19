using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.ComponentModel.DataAnnotations;

namespace ICP.Library.Models.MemberModels
{
    using Infrastructure.Core.Models;

    public class SMSAuth: BaseIPModel
    {
        /// <summary>
        /// 驗證類型 1: 註冊驗證 2:換機驗證 3:修改帳號 4:修改密碼 5: 忘記帳號  6:忘記密碼  7:更換手機  8:忘記圖形鎖發
        /// </summary>
        public byte AuthType { get; set; }

        /// <summary>
        /// 手機號碼，格式：09開頭，共10碼
        /// </summary>
        public string CellPhone { get; set; }

        public long MID { get; set; }
    }
}
