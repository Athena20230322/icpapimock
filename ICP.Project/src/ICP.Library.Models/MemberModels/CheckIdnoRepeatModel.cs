using ICP.Infrastructure.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICP.Library.Models.MemberModels
{
    using Infrastructure.Core.Models;

    public class CheckIdnoRepeatModel : BaseResult
    {
        public bool CanUse { get; set; }

        public bool Repeat { get; set; }
    }
}
