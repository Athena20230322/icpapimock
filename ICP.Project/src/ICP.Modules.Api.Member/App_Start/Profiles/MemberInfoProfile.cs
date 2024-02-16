using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICP.Modules.Api.Member.App_Start.Profiles
{
    using Library.Models.MemberModels;
    using Models.MemberInfo;

    public class MemberInfoProfile : Profile
    {
        public MemberInfoProfile()
        {
            CreateMap<MemberAppToken, AddMemberAppToken>();
        }
    }
}
