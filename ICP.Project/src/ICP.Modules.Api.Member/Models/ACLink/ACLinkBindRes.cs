using ICP.Infrastructure.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICP.Modules.Api.Member.Models.ACLink
{
    public class ACLinkBindRes
    {
        /// <summary>
        /// AccountLink綁定時轉導的頁面
        /// </summary>
        public string URL { get; set; }

        /// <summary>
        /// 銀行RtnCode
        /// </summary>
        public string ServiceCode { get; set; }

    }
}
