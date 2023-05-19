using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICP.Library.Models.MailLibrary
{
    public class MailSendType
    {
        public enum Status
        {
            WaitSend = 0,
            Send = 1,
            SendErr = 2
        }
        public enum Source
        {
            Default = 0,
            Admin = 1,
            Member = 2,
            Payment = 3,
            PaymentCenter = 4,
            MiddleWare = 5
        }
        public enum ErrorMail
        {

            False = 0,
            True = 1
        }
    }
}
