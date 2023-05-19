using AutoMapper;
using ICP.Library.Models.Payment;
using ICP.Modules.Api.Payment.Models.Cashier;
using ICP.Modules.Api.Payment.Models.ChargeBack;
using ICP.Modules.Api.Payment.Models.Pos;
using ICP.Modules.Api.Payment.Models.QueryOnlinecharge;

namespace ICP.Modules.Mvc.Admin.App_Start.Profiles
{
    public class PaymentChargeProfile : Profile
    {
        public PaymentChargeProfile()
        {
            CreateMap<CashierReq, AddTradeDBReq>();
            CreateMap<PaymentCenterRes, UpdateTradeDBReq>();
            CreateMap<UpdateTradeDBRes, CashierRes>();
            CreateMap<CheckOutReq, CashierReq>();
            CreateMap<RefundReq, ChargeBackReq>();
            CreateMap<CancelReq, ChargeBackReq>();
            CreateMap<BarcodeInfoDbRes, QueryMemberInfoRes>();
            CreateMap<TopUpRefundReq, ChargeBackReq>();
            CreateMap<QueryOnlinechargeReq, GetTradeInfoDbReq>();
            CreateMap<GetTradeInfoDbRes, QueryOnlinechargeRes>();
            CreateMap<TopUpReq, CashierReq>();
        }
    }
}
