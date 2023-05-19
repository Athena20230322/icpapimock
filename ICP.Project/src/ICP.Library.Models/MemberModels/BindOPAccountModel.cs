using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICP.Library.Models.MemberModels
{
    public class BindOPAccountModel
    {
        /// <summary>
        /// 會員編號
        /// </summary>
        public long MID { get; set; }

        /// <summary>
        /// OPEN WALLET MID
        /// </summary>
        public string OPMID { get; set; }

        /// <summary>
        /// 綁定類型 0: 綁定 1:解綁
        /// </summary>
        public byte Type { get; set; }

        /// <summary>
        /// 來源 : 0: App 1: 電支後台 2:排程
        /// </summary>
        public byte Source { get; set; }
    }
}
