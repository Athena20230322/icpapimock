using AutoMapper;
using ICP.Modules.Api.Payment.Models;
using ICP.Modules.Api.Payment.Models.Cashier;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICP.Modules.Api.Payment.App_Start.Profiles
{
    public class PaymentProfile : Profile
    {
        public PaymentProfile()
        {
            CreateMap<TopUpATMReq, CashierReq>();
            CreateMap<CashierRes, TopUpATMRes>();
        }
    }
}
