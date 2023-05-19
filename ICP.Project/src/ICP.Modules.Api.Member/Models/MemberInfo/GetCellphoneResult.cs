using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICP.Modules.Api.Member.Models.MemberInfo
{
    using Library.Models.AuthorizationApi;

    public class GetCellphoneResult : BaseAuthorizationApiResult
    {
        /// <summary>
        /// 登入/註冊TokenID
        /// </summary>
        public string LoginTokenID { get; set; }

        /// <summary>
        /// 登入/註冊Token期限
        /// </summary>
        public DateTime LoginTokenExpired { get; set; }

        /// <summary>
        /// 手機號碼，格式：09開頭，共10碼
        /// </summary>
        public string CellPhone { get; set; }

        /// <summary>
        /// 註冊狀態
        /// 1：已完成手機號碼驗證
        /// 2：手機號碼驗證中
        /// 3：尚未驗證手機號碼
        /// </summary>
        public int RegisterStatus { get; set; }
    }
}
