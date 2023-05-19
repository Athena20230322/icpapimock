using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ICP.Host.APIService.Commands
{
    using Infrastructure.Core.Models;
    using Models;
    using Services;

    public class SMSCommand
    {
        SMSService _smsService;

        public SMSCommand(SMSService smsService)
        {
            _smsService = smsService;
        }

        /// <summary>
        /// 發送簡訊
        /// </summary>
        /// <param name="Phone"></param>
        /// <param name="MsgData"></param>
        public BaseResult SendSMS(string Phone, string MsgData, byte SMSType, string Sender)
        {
            int gateway = _smsService.GetPriorityGateway();

            var smsModel = new AddSMS
            {
                Phone = Phone,
                MsgData = MsgData,
                SmsType = SMSType,
                Gateway = gateway,
                Sender = Sender
            };

            return _smsService.AddSMS(smsModel);
        }
    }
}