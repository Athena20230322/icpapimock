using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICP.Modules.Mvc.Admin.Models.CustomerManager
{
    public class AuthMemberUpgradeListLogVM
    {
        /// <summary>
        /// 電支帳號
        /// </summary>
        public string ICPMID { get; set; }

        /// <summary>
        /// 姓名
        /// </summary>
        public string CName { get; set; }

        /// <summary>
        /// 身分證字號/統編
        /// </summary>
        public string IDNO { get; set; }

        /// <summary>
        /// E-mail
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// 手機號碼
        /// </summary>
        public string CellPhone { get; set; }

        /// <summary>
        /// 轉換類別
        /// 會員等級: 12: 一類會員 13 : 二類會員 
        /// </summary>
        public int LevelID { get; set; }

        /// <summary>
        /// 轉換狀態
        /// 轉換狀態 1:成功, 2:失敗
        /// </summary>
        public int UpgradeStatus { get; set; }

        /// <summary>
        /// 記錄時間
        /// </summary>
        public DateTime CreateDate { get; set; }
    }
}
