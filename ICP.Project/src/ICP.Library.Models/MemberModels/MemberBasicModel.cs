using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICP.Library.Models.MemberModels
{
    public class MemberBasicModel
    {
        /// <summary>
        /// 會員編號
        /// </summary>
        public long MID { get; set; }

        /// <summary>
        /// ICASH 電支帳號
        /// </summary>
        public string ICPMID { get; set; }

        /// <summary>
        /// OPEN WALLET MID
        /// </summary>
        public string OPMID { get; set; }

        /// <summary>
        /// 登入帳號
        /// </summary>
        public string Account { get; set; }

        /// <summary>
        /// 姓名
        /// </summary>
        public string CName { get; set; }

        /// <summary>
        /// 驗證方式, 採AND運算, 1:手機驗證 2 : P11驗證  4: 綁定 OP 帳號
        /// </summary>
        public short AuthType { get; set; }

        /// <summary>
        /// 會員類型, 採用AND運算   1: 一類會員(個人)  2:二類會員(個人)  8:紙本特店  128:新註冊會員
        /// </summary>
        public short MemberType { get; set; }

        /// <summary>
        /// 狀態 0: 停用 1: 啟用 2: 已刪除
        /// </summary>
        public byte MemberStatus { get; set; }

        /// <summary>
        /// 會員等級: 10 尚未註冊完成會員  12: 一類會員 13 : 二類會員  31: 特店
        /// </summary>
        public byte LevelID { get; set; }

        /// <summary>
        /// 最後登入日期
        /// </summary>
        public DateTime? LastLoginDate { get; set; }

        /// <summary>
        /// 上一次登入日期
        /// </summary>
        public DateTime? PreviousLoginDate { get; set; }

        /// <summary>
        /// 會員註冊來源 0: App注冊 1:後台批次匯入
        /// </summary>
        public byte Source { get; set; }

        /// <summary>
        /// 建立時間
        /// </summary>
        public DateTime CreateDT { get; set; }

        /// <summary>
        /// 修改時間
        /// </summary>
        public DateTime ModifyDT { get; set; }

        /// <summary>
        /// 到期日
        /// </summary>
        public DateTime ExpireDate { get; set; }
    }
}