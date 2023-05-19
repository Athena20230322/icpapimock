using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICP.Modules.Api.AccountLink.Models.First
{
    /// <summary>
    /// / 第一銀行-連結綁定狀態查詢(ACLinkQuery) 接收參數
    /// </summary>
    public class ACLinkBindQryModel: BaseACLinkModel
    {
        /// <summary>
        /// 原訊息序號
        /// </summary>
        public string SerMsgNo { get; set; }
    }
}
