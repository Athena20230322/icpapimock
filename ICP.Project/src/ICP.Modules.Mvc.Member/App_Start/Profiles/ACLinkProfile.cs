using AutoMapper;
using ICP.Modules.Mvc.Member.Models.ACLink;

namespace ICP.Modules.Mvc.Member.App_Start.Profiles
{
    public class ACLinkProfile : Profile
    {
        public ACLinkProfile()
        {
            CreateMap<ChinaTrustACLinkApplyReq, ACLinkBindModel>();
            CreateMap<ACLinkBindModel, Api.Member.Models.ACLink.ACLinkBindReq>();
            CreateMap<ACLinkBindModel, Api.Member.Models.ACLink.ACLinkApplyReq>();
        }
    }
}
