using ICP.Infrastructure.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICP.Modules.Mvc.Admin.Models
{
    public class TopUpReportQueryCondition : PageModel
    {
        /// <summary>
        /// 日期類型 → 1:訂單日期 2:收款日期 3:傳輸日期
        /// </summary>
        public int DateType { get; set; }

        /// <summary>
        /// 起始日期
        /// </summary>
        public DateTime StartDate { get; set; }

        /// <summary>
        /// 結束日期
        /// </summary>
        public DateTime EndDate { get; set; }

        /// <summary>
        /// 儲值方式 → 0:全部 1:特店 2:虛擬帳號 3:連結扣款帳戶 4:中獎發票
        /// </summary>
        public int TopUpType { get; set; }

        /// <summary>
        /// 電支會員資料類型 → 1:電支會員帳號 2:電支會員姓名 3:電支會員手機
        /// </summary>
        public int MemberDataType { get; set; }

        /// <summary>
        /// 電支會員資料內容
        /// </summary>
        public string MemberDataContent { get; set; }

        /// <summary>
        /// 撥款狀態 → 0:全部 1:儲值成功 2:儲值失敗 3:取消儲值 4:儲值待繳費
        /// </summary>
        public int TopUpStatus { get; set; }

        /// <summary>
        /// 查詢資料類型 → 0:訂單類別-全部 1:icash pay 訂單編號 2:銀行轉帳虛擬帳號 3:超商店號
        /// </summary>
        public int QueryDataType { get; set; }

        /// <summary>
        /// 查詢資料內容
        /// </summary>
        public string QueryDataContent { get; set; }
    }
}
