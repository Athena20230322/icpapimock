using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;

namespace ICP.Modules.Mvc.Admin.App_Start.Profiles
{
    using Models.MemberModels;

    public class MemberProfile: Profile
    {
        public MemberProfile()
        {
            CreateMap<MemberAuthIDNOModel, UpdateMemberAuthIDNO>();
        }
    }
}
