using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICP.Modules.Mvc.Admin.Models.ViewModels
{
    using Library.Models.MemberModels;
    using Models.MemberModels;

    public class UpdateTeenagerViewModel: UpdateTeenagerModel
    {
        public long MID { get; set; }

        public List<MemberTeenagersLegalDetail> LegalDetails { get; set; }
    }
}
