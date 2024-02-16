using ICP.Infrastructure.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICP.Modules.Mvc.Admin.Models.CustomerManager
{
    public class ListOverSeaUserResultModel : BaseListModel
    {
        /// <summary>
        /// OPMID
        /// </summary>
        public string OPMID { get; set; }

        /// <summary>
        /// 會員姓名
        /// </summary>
        public string CName { get; set; }

        /// <summary>
        /// 手機號碼
        /// </summary>
        public string CellPhone { get; set; }

        /// <summary>
        /// 地址
        /// </summary>
        public string Address { get; set; }

        /// <summary>
        /// 電子郵件
        /// </summary>
        public string Email { get; set; }    
        
        /// <summary>
        /// 會員編號
        /// </summary>
        public long MID { get; set; }

        /// <summary>
        /// 居留證字號
        /// </summary>
        public string UniformID { get; set; }

        /// <summary>
        /// 驗證狀態
        /// </summary>
        public int AuthStatus { get; set; }

        /// <summary>
        /// 發證日期
        /// </summary>
        public DateTime UniformIssueDate { get; set; }

        /// <summary>
        /// 發證日期(民國年月日)
        /// </summary>
        public string UniformIssueDateYYYMMDD { get; set; }

        /// <summary>
        /// 居留證到期日期
        /// </summary>
        public DateTime UniformExpireDate { get; set; }

        /// <summary>
        /// 居留證到期日期(民國年月日)
        /// </summary>
        public string UniformExpireDateYYYMMDD { get; set; }

        /// <summary>
        /// 背面流水號
        /// </summary>
        public string UniformNumber { get; set; }

        /// <summary>
        /// 居留證正面
        /// </summary>
        public string IDNO_FilePath1 { get; set; }

        /// <summary>
        /// 居留證背面
        /// </summary>
        public string IDNO_FilePath2 { get; set; }

        /// <summary>
        /// 建立日期
        /// </summary>
        public DateTime CreateDate { get; set; }

        /// <summary>
        /// 最後修改日期
        /// </summary>
        public DateTime ModifyDate { get; set; }

        /// <summary>
        /// 修改者
        /// </summary>
        public string Modifier { get; set; }

        /// <summary>
        /// 銀行代碼
        /// </summary>
        public string BankCode { get; set; }

        /// <summary>
        /// 銀行帳號
        /// </summary>
        public string BankAccount { get; set; }

        /// <summary>
        /// 分行名稱
        /// </summary>
        public string BankName { get; set; }

        /// <summary>
        /// 存摺封面
        /// </summary>
        public string Bank_FilePath1 { get; set; }

        /// <summary>
        /// 國籍名稱
        /// </summary>
        public string NationalityName { get; set; }

        /// <summary>
        /// P33驗證狀態 (1:P33驗證失敗，列入黑名單)
        /// </summary>
        public int BlockStatus { get; set; }

        /// <summary>
        /// 銀行帳號驗證狀態
        /// </summary>
        public int AccountStatus { get; set; }

    }
}
