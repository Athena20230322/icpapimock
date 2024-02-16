using ICP.Infrastructure.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ICP.Host.Middleware.JCIC.Models
{
    public class P33AuthResult : BaseResult
    {
        /// <summary>
        /// 是否通過驗證
        /// 0 = 否
        /// 1 = 是
        /// 2 = 待審
        /// </summary>
        public short IsPass { get; set; }

        /// <summary>
        /// 聯徵回傳的通報案件資料筆數
        /// </summary>
        public int DataCount { get; set; }

        /// <summary>
        /// 聯徵回傳結果
        /// </summary>
        public string DataList { get; set; }
    }
}