using AutoMapper;
using ICP.Modules.Api.Payment.Models.ACLink;
using ICP.Modules.Api.Payment.Models.Cashier;
using ICP.Modules.Api.Payment.Models.TopUp;

namespace ICP.Modules.Api.Payment.App_Start.Profiles
{
    public class TopUpProfile : Profile
    {
        public TopUpProfile()
        {
            CreateMap<AutoTopUpConditionReq, AutoTopUpModel>();
            CreateMap<TradeResultModel, TopUpTradeInfoModel>();
            CreateMap<ACLinkTopUpReq, ACLinkTopUpModel>();
            CreateMap<TopUpTradeInfoModel, CashierReq>();
            CreateMap<CashierRes, TopUpTradeInfoModel>();
            CreateMap<ACLinkInfoDbRes, ACLinkModel>();
        }
    }
}
