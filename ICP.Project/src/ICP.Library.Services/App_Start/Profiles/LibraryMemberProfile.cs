using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;

namespace ICP.Library.Services.App_Start.Profiles
{
    using Library.Models.MemberModels;

    public class LibraryMemberProfile : Profile
    {
        public LibraryMemberProfile()
        {
            CreateMap<P11Auth, P11AuthResult>();
        }
    }
}
