using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICP.Library.Models.MemberModels
{
    /// <summary>
    /// 會員刪除
    /// </summary>
    public class MemberDeleteModel
    {
        /// <summary>
        /// 會員編號
        /// </summary>
        public long? MID { get; set; }

        /// <summary>
        /// OP會員編號
        /// </summary>
        public string OPMID { get; set; }

        /// <summary>
        /// 來源
        /// 1: api 通知
        /// 2: ftp 批次檔
        /// </summary>
        public byte Source { get; set; }
    }
}
