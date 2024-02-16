using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICP.Modules.Mvc.Admin.Models.MemberModels
{
    using ICP.Infrastructure.Core.Models.Consts;
    using Infrastructure.Core.Models;
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// 未成年審核資料查詢
    /// </summary>
    public class MemberTeenagersQuery: PageModel
    {
        /// <summary>
        /// 查詢日期 啟始
        /// </summary>
        public DateTime CreateDateBegin { get; set; }

        /// <summary>
        /// 查詢日期 結束
        /// </summary>
        public DateTime CreateDateEnd { get; set; }

        /// <summary>
        /// 階段 0:預設 1:完成  2: 法定代理人同意 3: 法定代理人資料通過
        /// </summary>
        public byte? Stage { get; set; }

        /// <summary>
        /// 法代是否同意申請 0: 預設 1: 同意
        /// </summary>
        public byte? LPAgree { get; set; }

        /// <summary>
        /// 法代資料是否審過 0: 待審 1:審核通過 2: 審核失敗
        /// </summary>
        public byte? LPAuth { get; set; }

        /// <summary>
        /// 身份驗證狀態 0: 待審 1: 審核通過 2:審核失敗
        /// </summary>
        public byte? IDNOStatus { get; set; }

        /// <summary>
        /// 電支帳號
        /// </summary>
        [RegularExpression(RegexConst.ICPMID, ErrorMessage = "{0} 格式錯誤")]
        [Display(Name = "電支帳號")] 
        public string ICPMID { get; set; }

        /// <summary>
        /// 個人姓名
        /// </summary>
        [StringLength(60)]
        [RegularExpression(RegexConst.ChineseCName, ErrorMessage = "{0} 格式錯誤")]
        [Display(Name = "個人姓名")]
        public string CName { get; set; }

        /// <summary>
        /// 身分證字號
        /// </summary>
        [RegularExpression(RegexConst.IDNO, ErrorMessage = "{0} 格式錯誤")]
        [Display(Name = "身分證字號")]
        public string IDNO { get; set; }
    }
}
