using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICP.Library.Models.MemberModels
{
    /// <summary>
    /// 未成年待審核資料
    /// </summary>
    public class MemberTeenager
    {
        /// <summary>
        /// 會員編號
        /// </summary>
        public long MID { get; set; }

        /// <summary>
        /// 審核狀態 0: 待審 1:審核通過 2:審核失敗
        /// </summary>
        public byte AuthStatus { get; set; }

        /// <summary>
        /// 階段 0:預設 1:完成  2: 法定代理人同意 3: 法定代理人資料通過
        /// </summary>
        public byte Stage { get; set; }

        /// <summary>
        /// 法代是否同意申請 0: 預設 1: 同意
        /// </summary>
        public byte LPAgree { get; set; }

        /// <summary>
        /// 法代同意日期
        /// </summary>
        public DateTime? LPAgreeDate { get; set; }

        /// <summary>
        /// 法代資料是否審過 0: 待審 1:審核通過 2: 審核失敗
        /// </summary>
        public byte LPAuth { get; set; }

        /// <summary>
        /// 法代資料審核日期
        /// </summary>
        public DateTime? LPAuthDate { get; set; }

        /// <summary>
        /// 身份驗證狀態 0: 待審 1: 審核通過 2:審核失敗
        /// </summary>
        public byte IDNOStatus { get; set; }

        /// <summary>
        /// 身份驗證日期
        /// </summary>
        public DateTime? IDNOAuthDate { get; set; }

        /// <summary>
        /// 審核備註
        /// </summary>
        public string Note { get; set; }

        /// <summary>
        /// 到期日期
        /// </summary>
        public DateTime ExpireDate { get; set; }

        /// <summary>
        /// 建立日期
        /// </summary>
        public DateTime CreateDate { get; set; }

        /// <summary>
        /// 修改日期
        /// </summary>
        public DateTime? ModifyDate { get; set; }

        /// <summary>
        /// 最後修改人 電支後台為登入帳號,會員為電支帳號
        /// </summary>
        public string Modifier { get; set; }
    }
}