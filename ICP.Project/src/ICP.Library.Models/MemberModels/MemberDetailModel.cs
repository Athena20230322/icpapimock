using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICP.Library.Models.MemberModels
{
    public class MemberDetailModel
    {
        /// <summary>
        /// 我們系統的唯一識別碼
        /// </summary>
        public long MID { get; set; }

        /// <summary>
        /// 身份證字號
        /// </summary>
        public string IDNO { get; set; }

        /// <summary>
        /// 統一證號/居留證號
        /// </summary>
        public string UniformID { get; set; }

        /// <summary>
        /// 統一編號
        /// </summary>
        public string UnifiedBusinessNo { get; set; }

        /// <summary>
        /// 公司名稱
        /// </summary>
        public string CompanyName { get; set; }

        /// <summary>
        /// 生日(YYYY-MM-DD)
        /// </summary>
        public DateTime? Birthday  { get; set; }

        /// <summary>
        /// AreaID
        /// </summary>
        public string AreaID { get; set; }

        /// <summary>
        /// 郵遞區號
        /// </summary>
        public string ZipCode { get; set; }
        

        public string Address { get; set; }

        public string CellPhone { get; set; }

        public string Email { get; set; }

        public long NationalityID { get; set; }

        /// <summary>
        /// 推薦人
        /// </summary>
        public long ReferrerMID { get; set; }

        /// <summary>
        /// 推薦碼
        /// </summary>
        public string ReferrerCode { get; set; }

        /// <summary>
        /// 暱稱
        /// </summary>
        public string NickName { get; set; }
    }
}
