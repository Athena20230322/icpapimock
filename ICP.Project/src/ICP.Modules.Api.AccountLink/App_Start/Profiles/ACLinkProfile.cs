using AutoMapper;
using ICP.Modules.Api.AccountLink.Models;

namespace ICP.Modules.Api.AccountLink.App_Start.Profiles
{
    public class ACLinkProfile : Profile
    {
        public ACLinkProfile()
        {
            CreateMap<ACLinkInfoModel, ACLinkInfoDbRes>();
            CreateMap<ACLinkTradeModel, ACLinkVAccountDbRes>();
        }
    }
}
