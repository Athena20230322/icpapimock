using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICP.Library.Models.MemberModels
{
    public class BankDetail
    {
        /// <summary>
        /// 銀行代碼
        /// </summary>
        public string BankCode { get; set; }

        /// <summary>
        /// 銀行名稱
        /// </summary>
        public string BankName { get; set; }

        /// <summary>
        /// 銀行簡稱
        /// </summary>
        public string BankShortName { get; set; }

        /// <summary>
        /// app 顯示名稱
        /// </summary>
        public string BankAppName { get; set; }

        /// <summary>
        /// 銀行的中文簡稱
        /// </summary>
        public string DisplayShortNameTW { get; set; }

        /// <summary>
        /// 銀行的英文簡稱
        /// </summary>
        public string DisplayShortNameEN { get; set; }

        /// <summary>
        /// 是否為合作銀行
        /// </summary>
        public bool isCooperate { get; set; }
    }
}
