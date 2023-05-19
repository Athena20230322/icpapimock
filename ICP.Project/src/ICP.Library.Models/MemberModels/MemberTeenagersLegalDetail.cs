using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICP.Library.Models.MemberModels
{
    /// <summary>
    /// 會員未成年子女申請資料
    /// </summary>
    public class MemberTeenagersLegalDetail
    {
        /// <summary>
        /// 法定代理人會員編號
        /// </summary>
        public long MID { get; set; }

        /// <summary>
        /// 未成年子女會員編號
        /// </summary>
        public long TeenagersMID { get; set; }

        /// <summary>
        /// 法定代理人的姓名
        /// </summary>
        public string CName { get; set; }

        /// <summary>
        /// 法代類別 1: 父母 2: 非父母 3:單一法定代理人
        /// </summary>
        public byte LegalType { get; set; }

        /// <summary>
        /// 狀態 0:預設 1: 同意未成年申請
        /// </summary>
        public byte Status { get; set; }

        /// <summary>
        /// 同意申請時間
        /// </summary>
        public DateTime? AgreeDate { get; set; }

        /// <summary>
        /// 法代資料審核身份證檔案1
        /// </summary>
        public string IDNOFile1 { get; set; }

        /// <summary>
        /// 法代資料審核身份證檔案2
        /// </summary>
        public string IDNOFile2 { get; set; }

        /// <summary>
        /// 法代資料審核文件檔案1
        /// </summary>
        public string FilePath1 { get; set; }

        /// <summary>
        /// 法代資料審核文件檔案2
        /// </summary>
        public string FilePath2 { get; set; }

        /// <summary>
        /// 法代資料審核文件檔案3
        /// </summary>
        public string FilePath3 { get; set; }

        /// <summary>
        /// 法代資料審核文件檔案3
        /// </summary>
        public string FilePath4 { get; set; }

        /// <summary>
        /// 法代資料審核文件檔案4
        /// </summary>
        public string FilePath5 { get; set; }

        /// <summary>
        /// 法代資料審核文件檔案5
        /// </summary>
        public string FilePath6 { get; set; }

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
