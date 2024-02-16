using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICP.Modules.Mvc.Admin.Models
{
    using Infrastructure.Core.Models;

    public class CheckUserForgetTokenResult : BaseResult
    {
        public int UserID { get; set; }
    }
}
