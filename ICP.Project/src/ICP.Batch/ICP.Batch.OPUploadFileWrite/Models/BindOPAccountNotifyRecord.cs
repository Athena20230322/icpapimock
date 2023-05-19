using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICP.Batch.OPUploadFileWrite.Models
{
    public class BindOPAccountNotifyRecord
    {
        /// <summary>
        /// 通知記錄編號
        /// </summary>
        public long RecordID { get; set; }

        /// <summary>
        /// 會員編號
        /// </summary>
        public long MID { get; set; }

        /// <summary>
        /// OPEN WALLET MID
        /// </summary>
        public string OPMID { get; set; }

        /// <summary>
        /// 通知類型 0: 綁定 1:解綁
        /// </summary>
        public byte Type { get; set; }

        /// <summary>
        /// ICASH 電支帳號
        /// </summary>
        public string ICPMID { get; set; }

        /// <summary>
        /// 愛金卡會員載具
        /// </summary>
        public string ICPCarrier { get; set; }

        /// <summary>
        /// 載具類型
        /// </summary>
        public string CarrierType { get; set; }
    }
}
