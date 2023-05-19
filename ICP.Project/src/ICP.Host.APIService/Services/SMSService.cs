using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ICP.Host.APIService.Services
{
    using Infrastructure.Core.Models;
    using Models;
    using Repositories;

    public class SMSService
    {
        SMSRepository _smsRepository;

        public SMSService(SMSRepository smsRepository)
        {
            _smsRepository = smsRepository;
        }

        /// <summary>
        /// 取得主要閘道
        /// </summary>
        /// <returns></returns>
        public int GetPriorityGateway()
        {
            return _smsRepository.GetPriorityGateway();
        }

        /// <summary>
        /// 新增簡訊內容
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public BaseResult AddSMS(AddSMS model)
        {
            return _smsRepository.AddSMS(model);
        }
    }
}