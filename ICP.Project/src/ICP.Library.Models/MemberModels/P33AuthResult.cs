using ICP.Infrastructure.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICP.Library.Models.MemberModels
{
    public class P33AuthResult : BaseResult
    {
        public long MID { get; set; }

        public string IDNO { get; set; }

        public byte AuthStatus { get; set; }
    }
}
