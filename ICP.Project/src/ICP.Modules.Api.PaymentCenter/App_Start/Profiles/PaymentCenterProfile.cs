using AutoMapper;
using ICP.Modules.Api.PaymentCenter.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICP.Modules.Api.PaymentCenter.App_Start.Profiles
{
    class PaymentCenterProfile : Profile
    {
        public PaymentCenterProfile()
        {
            CreateMap<TradeReqModel, AtmNotifyModel>();
            CreateMap<TradeInfoAtm, AtmNotifyModel>();
        }
    }
}
