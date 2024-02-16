using AutoMapper;
using ICP.Modules.Api.Member.Models.ACLink;

namespace ICP.Modules.Api.Member.App_Start.Profiles
{
    public class ACLinkProfile : Profile
    {
        public ACLinkProfile()
        {
            CreateMap<ACLinkApplyReq, ACLinkBindModel>();
            CreateMap<ACLinkBindReq, ACLinkBindModel>();
            CreateMap<ACLinkCancelBindReq, ACLinkBindModel>();
            CreateMap<GetACLinkListReq, ACLinkBindModel>();
        }
    }
}
