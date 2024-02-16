using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICP.Batch.ExecSendFETSMS.Models
{
    public class SMSModel
    {
        public long AutoID { get; set; }

        public string Phone { get; set; }

        public string MsgData { get; set; }

        /// <summary>
        /// 0 = 國內簡訊
        /// 1 = 國外簡訊
        /// </summary>
        public int SmsType { get; set; }

        public string CreateDate { get; set; }

        public int States { get; set; }

        public string RtnCode { get; set; }

        public string RtnMsg { get; set; }

        public string SendDate { get; set; }

        /// <summary>
        /// 0 = 內部發送(三竹)
        /// 1 = 外部發送(OMG)
        /// 2 = 遠傳
        /// </summary>
        public int Gateway { get; set; }

        public string MessageId { get; set; }

        public int ChangeStatus { get; set; }
    }
}
