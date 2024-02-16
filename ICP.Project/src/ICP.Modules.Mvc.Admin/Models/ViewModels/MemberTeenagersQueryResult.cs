using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICP.Modules.Mvc.Admin.Models.ViewModels
{
    using Infrastructure.Core.Models;

    /// <summary>
    /// 未成年審核資料查詢結果
    /// </summary>
    public class MemberTeenagersQueryResult : BaseListModel
    {
        /// <summary>
        /// 未成年 會員編號
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
        /// 法代資料是否審過 0: 待審 1:審核通過 2: 審核失敗
        /// </summary>
        public byte LPAuth { get; set; }

        /// <summary>
        /// 身份驗證狀態 0: 待審 1: 審核通過 2:審核失敗
        /// </summary>
        public byte IDNOStatus { get; set; }

        /// <summary>
        /// 到期日期
        /// </summary>
        public DateTime ExpireDate { get; set; }

        /// <summary>
        /// 身份驗證狀態 0: 待審 1: 審核通過 2:審核失敗
        /// </summary>
        public DateTime CreateDate { get; set; }

        /// <summary>
        /// 修改日期
        /// </summary>
        public DateTime? ModifyDate { get; set; }

        /// <summary>
        /// 最後修改人
        /// </summary>
        public string Modifier { get; set; }

        /// <summary>
        /// 電支帳號
        /// </summary>
        public string ICPMID { get; set; }

        /// <summary>
        /// 姓名
        /// </summary>
        public string CName { get; set; }
    }
}