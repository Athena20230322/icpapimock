using AutoMapper;
using ICP.Modules.Mvc.Payment.Models.QueryTrade;

namespace ICP.Modules.Mvc.Payment.App_Start.Profiles
{
    public class TradeProfile : Profile
    {
        public TradeProfile()
        {
            CreateMap<TradeReq, TradeDbReq>();
            CreateMap<TopUpACLinkTradeDbRes, TopUpTradeInfo>();
            CreateMap<TopUpATMTradeDbRes, TopUpTradeInfo>();
            CreateMap<TopUpCashTradeDbRes, TopUpTradeInfo>();
            CreateMap<TransferTradeReq, TradeDbReq>();
        }
    }
}
