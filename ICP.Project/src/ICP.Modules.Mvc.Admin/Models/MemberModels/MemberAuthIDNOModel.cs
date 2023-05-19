using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICP.Modules.Mvc.Admin.Models.MemberModels
{
    /// <summary>
    /// 會員身份證驗證資料
    /// </summary>
    public class MemberAuthIDNOModel
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
        /// 居留證
        /// </summary>
        public string UniformID { get; set; }

        /// <summary>
        /// 發證日期
        /// </summary>
        public DateTime? IssueDate { get; set; }

        /// <summary>
        /// 領取類別 (1:初發, 2:補發, 3:換發)
        /// </summary>
        public byte ObtainType { get; set; }

        /// <summary>
        /// 發證地點編號
        /// </summary>
        public string IssueLocationID { get; set; }

        /// <summary>
        /// 證上有無照片 (0: 無, 1:有)
        /// </summary>
        public byte IsPicture { get; set; }

        /// <summary>
        /// 驗證類型 & 運算 : 1 : P11  2: 文件審核
        /// </summary>
        public short AuthType { get; set; }

        /// <summary>
        /// 驗證狀態 0:預設值(待驗證), 1:驗證成功, 2:驗證不通過
        /// </summary>
        public byte AuthStatus { get; set; }

        /// <summary>
        /// 文件審核狀態 0:預設值(待驗證), 1:驗證成功, 2:驗證不通過
        /// </summary>
        public byte PaperAuthStatus { get; set; }

        /// <summary>
        /// 客服輸入的驗證訊息
        /// </summary>
        public string AuthMsg { get; set; }

        /// <summary>
        /// 最後審核日期 : 審核失敗或成功時間
        /// </summary>
        public DateTime? AuthDate { get; set; }

        /// <summary>
        /// 文件審核日期
        /// </summary>
        public DateTime? PaperAuthDate { get; set; }

        /// <summary>
        /// 居留證核發日期
        /// </summary>
        public DateTime? UniformIssueDate { get; set; }

        /// <summary>
        /// 居留期限
        /// </summary>
        public DateTime? UniformExpireDate { get; set; }

        /// <summary>
        /// 居留證流水號
        /// </summary>
        public string UniformNumber { get; set; }

        /// <summary>
        /// 證件正面照片
        /// </summary>
        public string FilePath1 { get; set; }

        /// <summary>
        /// 證件反面照片
        /// </summary>
        public string FilePath2 { get; set; }

        /// <summary>
        /// 來源 : 0: 使用者輸入 1: 批次匯入
        /// </summary>
        public byte Source { get; set; }

        /// <summary>
        /// 生日
        /// </summary>
        public DateTime BirthDay { get; set; } 
    }
}