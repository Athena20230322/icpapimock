using System;

namespace ICP.Library.Models.MemberModels
{
    public class AuthIDNO
    {
        /// <summary>
        /// 會員編號
        /// </summary>
        public long MID { get; set; }

        /// <summary>
        /// 身分證字號
        /// </summary>
        public string IDNO { get; set; }

        /// <summary>
        /// 發證日期
        /// </summary>
        public DateTime IssueDate { get; set; }

        /// <summary>
        /// 領取類別
        /// 1 = 初發
        /// 2 = 補發
        /// 3 = 換發
        /// </summary>
        public byte ObtainType { get; set; }

        /// <summary>
        /// 證上有無照片
        /// 0 = 無
        /// 1 = 有
        /// </summary>
        public byte IsPicture { get; set; }

        /// <summary>
        /// 發證地點編號
        /// </summary>
        public string IssueLocationID { get; set; }

        /// <summary>
        /// 驗證類型(&運算)
        /// 1 = P11
        /// 2 = 文件審核
        /// </summary>
        public byte AuthType { get; set; }

        /// <summary>
        /// 上傳檔案1
        /// </summary>
        public string FilePath1 { get; set; }

        /// <summary>
        /// 上傳檔案2
        /// </summary>
        public string FilePath2 { get; set; }

        /// <summary>
        /// 來源
        /// 0 = 使用者輸入
        /// 1 = 批次匯入
        /// </summary>
        public byte Source { get; set; }
    }
}
