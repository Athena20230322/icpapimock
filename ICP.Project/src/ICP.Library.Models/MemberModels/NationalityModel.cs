using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICP.Library.Models.MemberModels
{
    /// <summary>
    /// 國家
    /// </summary>
    public class NationalityModel
    {
        /// <summary>
        /// 國家流水號
        /// </summary>
        public long NationalityID { get; set; }

        /// <summary>
        /// 國家全名
        /// </summary>
        public string NationalityName { get; set; }

        /// <summary>
        /// 國家簡碼
        /// </summary>
        public string NationalityCode { get; set; }
    }
}